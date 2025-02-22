﻿using System.Diagnostics;
using System.Reflection;
using System.Text;
using BoostStudio.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace BoostStudio.Infrastructure.Scex;

public class ScexCompiler(ILogger<ScexCompiler> logger) : IScexCompiler
{
    public async Task DecompileAsync(
        string sourcePath,
        string destinationPath,
        CancellationToken cancellationToken = default)
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(workingDirectory);

        logger.LogInformation("Creating temporary working directory: {workingDirectory}", workingDirectory);

        try
        {
            if (!File.Exists(sourcePath))
            {
                logger.LogInformation("{sourcePath} is not a valid file, exiting decompiling operation", sourcePath);
                return;
            }

            var tempExecutablePath = await InitializeDecompilerExecutableAsync(workingDirectory, cancellationToken);

            var arguments = $"\"{sourcePath}\" -o \"{destinationPath}\"";
            logger.LogInformation("Executing scex-decompiler.exe with: {arguments}", arguments);

            // Execute process
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                Arguments = arguments,
                CreateNoWindow = true,
                FileName = tempExecutablePath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            process.Start();

            // Synchronously read the standard output of the spawned process.
            var stdOut = process.StandardOutput;
            var stdErr = process.StandardError;

            while (await stdOut.ReadLineAsync(cancellationToken) is {} outputLine)
                logger.LogInformation("{outputLine}", outputLine);

            var errorOutput = new StringBuilder();
            while (await stdErr.ReadLineAsync(cancellationToken) is {} outputLine)
                errorOutput.Append(outputLine);
        
            await process.WaitForExitAsync(cancellationToken);

            if (!File.Exists(destinationPath) || process.ExitCode != 0)
                throw new Exception($"Failed to decompile scex. Error: {errorOutput}");

            logger.LogInformation("Successfully decompiled scex script on: {destinationPath}", destinationPath);
        }
        finally
        {
            logger.LogInformation("Cleaning up temporary working directory...");

            // Cleanup regardless
            Directory.Delete(workingDirectory, true);
        }
    }
    
    public async Task CompileAsync(
        string sourcePath,
        string destinationPath,
        CancellationToken cancellationToken = default)
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(workingDirectory);

        logger.LogInformation("Creating temporary working directory: {workingDirectory}", workingDirectory);

        try
        {
            if (!File.Exists(sourcePath))
            {
                logger.LogInformation("{sourcePath} is not a valid file, exiting compiling operation", sourcePath);
                return;
            }

            var tempExecutablePath = await InitializeCompilerExecutableAsync(workingDirectory, cancellationToken);

            var arguments = $"\"{sourcePath}\" -o \"{destinationPath}\" -i";
            logger.LogInformation("Executing scex-compiler.exe with: {arguments}", arguments);

            // Execute process
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                Arguments = arguments,
                CreateNoWindow = true,
                FileName = tempExecutablePath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            process.Start();

            // Synchronously read the standard output of the spawned process.
            var stdOut = process.StandardOutput;
            var stdErr = process.StandardError;

            while (await stdOut.ReadLineAsync(cancellationToken) is {} outputLine)
                logger.LogInformation("{outputLine}", outputLine);

            var errorOutput = new StringBuilder();
            while (await stdErr.ReadLineAsync(cancellationToken) is {} outputLine)
                errorOutput.Append(outputLine);
        
            await process.WaitForExitAsync(cancellationToken);

            if (!File.Exists(destinationPath) || process.ExitCode != 0)
                throw new Exception($"Failed to compile scex. Error: {errorOutput}");

            // Weird patching
            BinaryPatch(sourcePath, destinationPath);

            logger.LogInformation("Successfully compiled scex script on: {destinationPath}", destinationPath);
        }
        finally
        {
            logger.LogInformation("Cleaning up temporary working directory...");

            // Cleanup regardless
            Directory.Delete(workingDirectory, true);
        }
    }

    private async Task<string> InitializeCompilerExecutableAsync(string workingDirectory, CancellationToken cancellationToken)
    {
        logger.LogInformation("Initializing scex-compiler.exe in: {workingDirectory}", workingDirectory);

        var workingPath = Path.Combine(workingDirectory, "scex-compiler.exe");

        // Extracting executable from resource to a temp location.
        await using var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BoostStudio.Infrastructure.Resources.scex-compiler.exe");

        if (resourceStream is null)
            throw new FileNotFoundException("scex-compiler resource not found.");

        await using var fileStream = File.Create(workingPath);
        await resourceStream.CopyToAsync(fileStream, cancellationToken);
        fileStream.Close();

        return workingPath;
    }
    
    private async Task<string> InitializeDecompilerExecutableAsync(string workingDirectory, CancellationToken cancellationToken)
    {
        logger.LogInformation("Initializing scex-decompiler.exe in: {workingDirectory}", workingDirectory);

        var workingPath = Path.Combine(workingDirectory, "scex-decompiler.exe");

        // Extracting executable from resource to a temp location.
        await using var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BoostStudio.Infrastructure.Resources.scex-decompiler.exe");

        if (resourceStream is null)
            throw new FileNotFoundException("scex-decompiler resource not found.");

        await using var fileStream = File.Create(workingPath);
        await resourceStream.CopyToAsync(fileStream, cancellationToken);
        fileStream.Close();

        return workingPath;
    }

    // Old legacy binary patching logic, not sure what it does anymore but just reuse for now
    private static void BinaryPatch(string originalScriptPath, string compiledBinaryPath)
    {
        var originalScript = File.ReadAllText(originalScriptPath);

        var resetCount = 0;

        if (originalScript.Contains(@"sys_2D(0x3, 0xd, var1, func_"))
            resetCount++;

        if (originalScript.Contains(@"sys_2D(0x3, 0xe, var1, func_"))
            resetCount++;

        if (originalScript.Contains(@"sys_2D(0x3, 0xf, var1, func_"))
            resetCount++;

        if (resetCount == 0)
            return;

        using var fs = File.OpenRead(compiledBinaryPath);
        var oms = new MemoryStream();
        fs.Seek(0, SeekOrigin.Begin);
        fs.CopyTo(oms);
        fs.Close();
        oms.Seek(0, SeekOrigin.Begin);

        var fixPosition = Search(oms, new byte[]
        {
            0x8A, 0x00, 0x00, 0x00, 0x03, 0x8A, 0x00, 0x00, 0x00, 0x0D, 0x8B, 0x00, 0x00, 0x01,
        });

        if (fixPosition == -1)
            throw new Exception();

        oms.Seek(fixPosition + 14, SeekOrigin.Begin);

        for (int i = 0; i < resetCount; i++)
        {
            int fixPosition1 = Search(oms, new byte[]
            {
                0x2E
            }, (int)oms.Position);
            if (fixPosition1 == -1)
                throw new Exception();

            oms.Seek(fixPosition1, SeekOrigin.Begin);
            oms.Write(new byte[]
            {
                0xAE
            }, 0, 1);
        }

        oms.Seek(0, SeekOrigin.Begin);

        var ofs = File.OpenWrite(compiledBinaryPath);
        oms.CopyTo(ofs);
        ofs.Close();
        oms.Close();
    }

    private static int Search(Stream srcMs, IReadOnlyList<byte> pattern, int offset)
    {
        srcMs.Seek(offset, SeekOrigin.Begin);
        var cutMs = new MemoryStream();
        srcMs.CopyTo(cutMs);

        var src = cutMs.ToArray();
        var c = src.Length - pattern.Count + 1;
        for (var i = 0; i < c; i++)
        {
            if (src[i] != pattern[0])
                continue;

            int j;
            for (j = pattern.Count - 1; j >= 1 && src[i + j] == pattern[j]; j--) {}
            if (j == 0)
                return i + offset;
        }
        return -1;
    }

    private static int Search(MemoryStream srcMs, IReadOnlyList<byte> pattern)
    {
        var src = srcMs.ToArray();
        var c = src.Length - pattern.Count + 1;
        for (var i = 0; i < c; i++)
        {
            if (src[i] != pattern[0]) continue;
            int j;
            for (j = pattern.Count - 1; j >= 1 && src[i + j] == pattern[j]; j--) {}
            if (j == 0) return i;
        }
        return -1;
    }
}
