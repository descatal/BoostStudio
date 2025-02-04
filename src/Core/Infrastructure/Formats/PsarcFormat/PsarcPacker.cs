﻿using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using BoostStudio.Application.Common.Interfaces.Formats.PsarcFormat;
using BoostStudio.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace BoostStudio.Infrastructure.Formats.PsarcFormat;

// TODO rework to Kaitai struct implementation
public class PsarcPacker(ILogger<PsarcPacker> logger) : IPsarcPacker
{
    public async Task UnpackAsync(
        string sourceFilePath, 
        string destinationPath, 
        CancellationToken cancellationToken)
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(workingDirectory);

        try
        {
            await UnpackAsyncInternal(
                sourceFilePath, 
                destinationPath,
                cancellationToken);
        }
        finally
        {
            // Cleanup regardless
            Directory.Delete(workingDirectory, true);
        }
    }
    
    public async Task PackAsync(
        string sourcePath,
        string destinationPath,
        CompressionType compressionType,
        int compressionLevel,
        CancellationToken cancellationToken)
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(workingDirectory);
        
        logger.LogInformation("Creating temporary working directory: {workingDirectory}", workingDirectory);

        try
        {
            await PackAsyncInternal(
                sourcePath, 
                destinationPath, 
                compressionType, 
                compressionLevel, 
                workingDirectory, 
                cancellationToken);
        }
        finally
        {
            logger.LogInformation("Cleaning up temporary working directory...");
            
            // Cleanup regardless
            Directory.Delete(workingDirectory, true);
        }
    }
    
    private async Task UnpackAsyncInternal(
        string sourceFilePath, 
        string destinationPath, 
        CancellationToken cancellationToken)
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(workingDirectory);

        var isLinux = OperatingSystem.IsLinux();
        var tempPsarcExePath = await InitializeExecutableAsync(workingDirectory, cancellationToken);
        var wine = isLinux ? "/usr/bin/wine" : tempPsarcExePath;
        var wineArg = isLinux ? $"\"{tempPsarcExePath}\"" : string.Empty;
        var unpackDirectory = isLinux 
            ? Path.GetRandomFileName()
            : destinationPath;
        
        if (!Directory.Exists(destinationPath))
            Directory.CreateDirectory(destinationPath);
        
        var arguments = $"{wineArg} extract --input=\"{sourceFilePath}\" --to=\"{unpackDirectory}\"";
        logger.LogInformation("Executing psarc.exe with: {arguments}", arguments);
        
        // Execute process
        using var psarcProcess = new Process();
        psarcProcess.StartInfo = new ProcessStartInfo
        {
            Arguments = arguments,
            CreateNoWindow = true,
            FileName = wine,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = workingDirectory
        };
        psarcProcess.Start();

        // Synchronously read the standard output of the spawned process.
        var standardOutputReader = psarcProcess.StandardOutput;
        while (await standardOutputReader.ReadLineAsync(cancellationToken) is {} outputLine)
            logger.LogInformation("{outputLine}", outputLine);
        
        var standardErrorReader = psarcProcess.StandardError;
        while (await standardErrorReader.ReadLineAsync(cancellationToken) is {} outputLine)
            logger.LogWarning("{outputLine}", outputLine);
        
        await psarcProcess.WaitForExitAsync(cancellationToken);

        if (isLinux)
        {
            var unpackWorkingDirectory = Path.Combine(workingDirectory, unpackDirectory);
            var allDirectories = Directory.GetDirectories(unpackWorkingDirectory, "*", SearchOption.AllDirectories);
            foreach (string dir in allDirectories) 
            { 
                string dirToCreate = dir.Replace(unpackWorkingDirectory, destinationPath); 
                Directory.CreateDirectory(dirToCreate);
            }
            
            var allFiles = Directory.GetFiles(unpackWorkingDirectory, "*.*", SearchOption.AllDirectories);
            foreach (string fileName in allFiles)
            {
                var targetPath = fileName.Replace(unpackWorkingDirectory, destinationPath);
                File.Copy(fileName, targetPath, true); 
            } 
        }
        
        if (!Directory.Exists(destinationPath))
            throw new Exception("Failed to unpack psarc archive.");
        
        logger.LogInformation("Successfully unpacked psarc archive on: {destinationPath}", destinationPath);
    }
    
    private async Task PackAsyncInternal(
        string sourcePath,
        string destinationPath,
        CompressionType compressionType,
        int compressionLevel,
        string workingDirectory,
        CancellationToken cancellationToken)
    {
        if (!Directory.Exists(sourcePath))
        {
            logger.LogInformation("{sourcePath} is not a valid directory, exiting pack operation", sourcePath);
            return;
        }

        var tempMetadataPath = Path.Combine(workingDirectory, "metadata.xml");
        var tempPsarcExePath = await InitializeExecutableAsync(workingDirectory, cancellationToken);

        // Construct metadata xml: adding all of the files to be packed.
        var fileSystemsPath = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories).ToList();
        fileSystemsPath = fileSystemsPath.OrderBy(s => s).ToList();

        logger.LogInformation("Number of files to be packed: {fileLength}", fileSystemsPath.Count);
        
        var create = new XElement("create",
            new XAttribute("overwrite", "true"),
            new XAttribute("archive", $"{destinationPath}"));

        if (compressionType != CompressionType.None)
        {
            create.Add(new XElement("compression",
                new XAttribute("type", $"{compressionType.ToString().ToLower()}"),
                new XAttribute("level", $"{compressionLevel}"),
                new XAttribute("enabled", "true")));
        }

        create.Add(fileSystemsPath.Select(path =>
        {
            var repackPathUri = new Uri($"{sourcePath}/");
            var repackFilePathUri = new Uri(path);
            var repackFileRelativeUri = repackPathUri.MakeRelativeUri(repackFilePathUri);
            var archivePath = Uri.UnescapeDataString(repackFileRelativeUri.OriginalString);
            
            var pathAttribute = new XAttribute("path", path);
            var archivePathAttribute = new XAttribute("archivepath", archivePath);
            
            return new XElement("file", pathAttribute, archivePathAttribute);
        }));

        var metadataDoc = new XDocument(
            new XDeclaration("1.0", "UTF-8", null),
            new XElement("psarc", create));

        await using var writer = XmlWriter.Create(tempMetadataPath, new XmlWriterSettings
        {
            Encoding = new UTF8Encoding(false), Async = true
        });
        await metadataDoc.SaveAsync(writer, cancellationToken);
        writer.Close();

        logger.LogInformation("Writing psarc xml metadata to: {tempMetadataPath}", tempMetadataPath);
        
        var tempString = await File.ReadAllTextAsync(tempMetadataPath, cancellationToken);
        tempString = tempString.Replace("&amp;", "&");
        await File.WriteAllTextAsync(tempMetadataPath, tempString, cancellationToken);
        
        logger.LogInformation("Constructed psarc xml metadata: {@tempString}", tempString);
        
        var isLinux = OperatingSystem.IsLinux();
        var wine = isLinux ? "/usr/bin/wine" : tempPsarcExePath;
        var wineArg = isLinux ? $"\"{tempPsarcExePath}\"" : string.Empty;
        
        var arguments = $"{wineArg} create --xml \"{tempMetadataPath}\"";
        logger.LogInformation("Executing psarc.exe with: {arguments}", arguments);
        
        // Execute process
        using var psarcProcess = new Process();
        psarcProcess.StartInfo = new ProcessStartInfo
        {
            Arguments = arguments,
            CreateNoWindow = true,
            FileName = wine,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = workingDirectory
        };
        psarcProcess.Start();

        // Synchronously read the standard output of the spawned process.
        var standardOutputReader = psarcProcess.StandardOutput;
        while (await standardOutputReader.ReadLineAsync(cancellationToken) is {} outputLine)
            logger.LogInformation("{outputLine}", outputLine);
        
        var standardErrorReader = psarcProcess.StandardError;
        while (await standardErrorReader.ReadLineAsync(cancellationToken) is {} outputLine)
            logger.LogWarning("{outputLine}", outputLine);
        
        await psarcProcess.WaitForExitAsync(cancellationToken);

        if (!File.Exists(destinationPath))
            throw new Exception("Failed to create psarc archive.");
        
        logger.LogInformation("Successfully created psarc archive on: {destinationPath}", destinationPath);
    }
    
    private async Task<string> InitializeExecutableAsync(string workingDirectory, CancellationToken cancellationToken)
    {
        logger.LogInformation("Initializing psarc.exe in: {workingDirectory}", workingDirectory);
        
        var workingPath = Path.Combine(workingDirectory, "psarc.exe");

        // Extracting executable from resource to a temp location.
        await using var psarcResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BoostStudio.Infrastructure.Resources.psarc.exe");

        if (psarcResourceStream is null)
            throw new FileNotFoundException("Psarc resource not found.");
        
        await using var fileStream = File.Create(workingPath);
        await psarcResourceStream.CopyToAsync(fileStream, cancellationToken);
        fileStream.Close();

        return workingPath;
    }
}
