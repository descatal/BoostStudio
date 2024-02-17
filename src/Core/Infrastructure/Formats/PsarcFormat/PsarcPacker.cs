using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.PsarcFormat;
using BoostStudio.Application.Common.Models;
using Microsoft.Extensions.Logging;
using CompressionType=BoostStudio.Domain.Entities.PsarcFormat.CompressionType;

namespace BoostStudio.Infrastructure.Formats.PsarcFormat;

// TODO rework to Kaitai struct implementation
public class PsarcPacker(ICompressor compressor, ILogger<PsarcPacker> logger) : IPsarcPacker
{
    public async Task<byte[]> UnpackAsync(byte[] sourceFile, CancellationToken cancellationToken)
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(workingDirectory);
        
        var extractDirectory = Path.Combine(workingDirectory, "extract");
        var sourceFilePath = Path.Combine(workingDirectory, "source.psarc");

        await File.WriteAllBytesAsync(sourceFilePath, sourceFile, cancellationToken);

        var tempPsarcExePath = await InitializeExecutableAsync(workingDirectory, cancellationToken);
        
        // Execute process
        using var psarcProcess = new Process();
        psarcProcess.StartInfo = new ProcessStartInfo
        {
            Arguments = $"extract --input={sourceFilePath} --to={extractDirectory}",
            CreateNoWindow = true,
            FileName = tempPsarcExePath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        psarcProcess.Start();

        // Synchronously read the standard output of the spawned process.
        var reader = psarcProcess.StandardOutput;
        var output = await reader.ReadToEndAsync(cancellationToken);

        logger.LogInformation("{}", output);
        await psarcProcess.WaitForExitAsync(cancellationToken);

        if (!Directory.Exists(extractDirectory))
            throw new Exception(output);
        
        // Return tar file
        return await compressor.CompressAsync(extractDirectory, CompressionFormats.Tar, cancellationToken);
    }
    
    public async Task<byte[]> PackAsync(
        string sourcePath, 
        CompressionType compressionType, 
        int compressionLevel, 
        CancellationToken cancellationToken)
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(workingDirectory);
        
        try
        {
            return await PackFhm(sourcePath, compressionType, compressionLevel, workingDirectory, cancellationToken);
        }
        finally
        {
            // Cleanup regardless
            Directory.Delete(workingDirectory, true);
        }
    }

    private static async Task<string> InitializeExecutableAsync(string workingDirectory, CancellationToken cancellationToken)
    {
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

    private async Task<byte[]> PackFhm(
        string sourcePath, 
        CompressionType compressionType, 
        int compressionLevel, 
        string workingDirectory, 
        CancellationToken cancellationToken)
    {
        if (!Directory.Exists(sourcePath))
            return Array.Empty<byte>();
        
        var tempMetadataPath = Path.Combine(workingDirectory, "metadata.xml");
        var tempPsarcOutputPath = Path.Combine(workingDirectory, "output.psarc");
        var tempPsarcExePath = await InitializeExecutableAsync(workingDirectory, cancellationToken);
        
        // Construct metadata xml: adding all of the files to be packed.
        var fileSystemsPath = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
        
        var create = new XElement("create",
            new XAttribute("overwrite", "true"),
            new XAttribute("archive", $"{tempPsarcOutputPath}"));

        if (compressionType != CompressionType.None)
        {
            create.Add(new XElement("compression",
                new XAttribute("type", $"{compressionType.ToString().ToLower()}"),
                new XAttribute("level", $"{compressionLevel}"),
                new XAttribute("enabled", "true")));
        }

        create.Add(fileSystemsPath.Select(path => new XElement("file", new XAttribute("path", path))));
        
        var metadataDoc = new XDocument(
            new XDeclaration("1.0", "UTF-8", null),
            new XElement("psarc", create));

        await using var writer = XmlWriter.Create(tempMetadataPath, new XmlWriterSettings
        {
            Encoding = new UTF8Encoding(false), Async = true
        });
        await metadataDoc.SaveAsync(writer, cancellationToken);
        writer.Close();

        // Execute process
        using var psarcProcess = new Process();
        psarcProcess.StartInfo = new ProcessStartInfo
        {
            Arguments = $"create --xml {tempMetadataPath}",
            CreateNoWindow = true,
            FileName = tempPsarcExePath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        psarcProcess.Start();

        // Synchronously read the standard output of the spawned process.
        var reader = psarcProcess.StandardOutput;
        var output = await reader.ReadToEndAsync(cancellationToken);

        logger.LogInformation("{}", output);
        await psarcProcess.WaitForExitAsync(cancellationToken);

        if (!File.Exists(tempPsarcOutputPath))
            throw new Exception($"{output}");

        return await File.ReadAllBytesAsync(tempPsarcOutputPath, cancellationToken);
    }
}
