using System.Diagnostics;
using System.Text;
using BoostStudio.Application.Common.Interfaces.Formats.AudioFormats;
using Microsoft.Extensions.Logging;

namespace BoostStudio.Infrastructure.Formats.AudioFormats.Nus3;

public class Nus3Bank(ILogger<Nus3Bank> logger) : INus3Bank
{
    public async Task UnpackNus3BankAsync(
        string sourcePath, 
        string destinationPath, 
        CancellationToken cancellationToken = default)
    {
        if (!File.Exists(sourcePath))
            return;
        
        Directory.CreateDirectory(destinationPath);
        
        var nus3BankCliPath = Path.Combine(Path.GetTempPath(), "BoostStudio", "Resources", "nus3bank", "nus3bank.exe");
        var arguments = $"--extract \"{destinationPath}\" -- \"{sourcePath}\"";
        
        // Execute process
        using var psarcProcess = new Process();
        psarcProcess.StartInfo = new ProcessStartInfo
        {
            Arguments = arguments,
            CreateNoWindow = true,
            FileName = nus3BankCliPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        psarcProcess.Start();

        // Synchronously read the standard output of the spawned process.
        var stdOut = psarcProcess.StandardOutput;
        var stdErr = psarcProcess.StandardError;

        var standardOutput = new StringBuilder();
        while (await stdOut.ReadLineAsync(cancellationToken) is {} outputLine)
            standardOutput.Append(outputLine);

        var errorOutput = new StringBuilder();
        while (await stdErr.ReadLineAsync(cancellationToken) is {} outputLine)
            errorOutput.Append(outputLine);
        
        await psarcProcess.WaitForExitAsync(cancellationToken);

        if (psarcProcess.ExitCode != 0)
            throw new Exception($"Failed to unpack nus3bank file, error output: {errorOutput}");
    }
    
    public async Task<byte[]> PackDirectoryToNus3BankAsync(string sourcePath, CancellationToken cancellationToken = default)
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(workingDirectory);

        try
        {
            var nus3BankFilePath = Path.Combine(workingDirectory, "output.nus3bank");
            
            var nus3BankCliPath = Path.Combine(Path.GetTempPath(), "BoostStudio", "Resources", "nus3bank", "nus3bank.exe");
            var arguments = $"--construct \"{sourcePath}\" --write \"{nus3BankFilePath}\"";
        
            // Execute process
            using var psarcProcess = new Process();
            psarcProcess.StartInfo = new ProcessStartInfo
            {
                Arguments = arguments,
                CreateNoWindow = true,
                FileName = nus3BankFilePath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = workingDirectory
            };
            psarcProcess.Start();

            // Synchronously read the standard output of the spawned process.
            var stdOut = psarcProcess.StandardOutput;
            var stdErr = psarcProcess.StandardError;

            var standardOutput = new StringBuilder();
            while (await stdOut.ReadLineAsync(cancellationToken) is {} outputLine)
                standardOutput.Append(outputLine);

            var errorOutput = new StringBuilder();
            while (await stdErr.ReadLineAsync(cancellationToken) is {} outputLine)
                errorOutput.Append(outputLine);
        
            await psarcProcess.WaitForExitAsync(cancellationToken);

            if (psarcProcess.ExitCode != 0 || !File.Exists(nus3BankCliPath))
                return [];
        
            return await File.ReadAllBytesAsync(nus3BankCliPath, cancellationToken);
        }
        finally
        {
            // Cleanup regardless
            Directory.Delete(workingDirectory, true);
        }
    }
}
