using System.Diagnostics;
using System.Text;
using BoostStudio.Application.Common.Interfaces.Formats.AudioFormats;

namespace BoostStudio.Infrastructure.Formats.AudioFormats.Nus3Audio;

public class Nus3Audio : INus3Audio
{
    public async Task UnpackNus3AudioAsync(
        string sourcePath, 
        string destinationPath, 
        CancellationToken cancellationToken = default)
    {
        if (!File.Exists(sourcePath))
            return;
        
        Directory.CreateDirectory(destinationPath);
        
        var nus3AudioCliPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "nus3audio", "nus3audio.exe");
        var arguments = $"--extract-name \"{destinationPath}\" -- \"{sourcePath}\"";
        
        // Execute process
        using var psarcProcess = new Process();
        psarcProcess.StartInfo = new ProcessStartInfo
        {
            Arguments = arguments,
            CreateNoWindow = true,
            FileName = nus3AudioCliPath,
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
            throw new Exception($"Failed to unpack nus3audio file, error output: {errorOutput}");
    }
    
    public async Task<byte[]> PackDirectoryToNus3AudioAsync(string sourcePath, CancellationToken cancellationToken = default)
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(workingDirectory);

        try
        {
            var nus3AudioFilePath = Path.Combine(workingDirectory, "output.nus3audio");
            
            var nus3AudioCliPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "nus3audio", "nus3audio.exe");
            var arguments = $"--new --rebuild-name \"{sourcePath}\" --write \"{nus3AudioFilePath}\"";
        
            // Execute process
            using var psarcProcess = new Process();
            psarcProcess.StartInfo = new ProcessStartInfo
            {
                Arguments = arguments,
                CreateNoWindow = true,
                FileName = nus3AudioCliPath,
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

            if (psarcProcess.ExitCode != 0 || !File.Exists(nus3AudioFilePath))
                return [];
        
            return await File.ReadAllBytesAsync(nus3AudioFilePath, cancellationToken);
        }
        finally
        {
            // Cleanup regardless
            Directory.Delete(workingDirectory, true);
        }
    }
}
