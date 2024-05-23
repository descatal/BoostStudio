using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using BoostStudio.Application.Common.Interfaces.Formats.AudioFormats;
using FFMpegCore;
using FFMpegCore.Pipes;

namespace BoostStudio.Infrastructure.Formats.AudioFormats.Wav;

public partial class Riff : IRiff
{
    public async Task<byte[]> RiffToPcmAsync(
        byte[] riffBinary, 
        CancellationToken cancellationToken = default)
    {
        using var riffStream = new MemoryStream(riffBinary);
        using var pcmStream = new MemoryStream();
                
        // Convert wav to PCM file
        await FFMpegArguments
            .FromPipeInput(new StreamPipeSource(riffStream))
            .OutputToPipe(new StreamPipeSink(pcmStream), opts => opts
                .ForceFormat("s16le")
                .WithAudioCodec("pcm_s16le"))
            .ProcessAsynchronously();

        return pcmStream.ToArray();
    }
    
    public async Task<uint?> GetSampleSizeAsync(byte[] riffBinary, CancellationToken cancellationToken)
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(workingDirectory);
        
        try
        {
            var riffFilePath = Path.Combine(workingDirectory, "input.wav");
            await File.WriteAllBytesAsync(riffFilePath, riffBinary, cancellationToken);
            
            var vgmstreamCliPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "vgmstream", "vgmstream-cli.exe");
            var arguments = $"-m \"{riffFilePath}\"";
        
            // Execute process
            using var psarcProcess = new Process();
            psarcProcess.StartInfo = new ProcessStartInfo
            {
                Arguments = arguments,
                CreateNoWindow = true,
                FileName = vgmstreamCliPath,
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
                return null;

            var output = standardOutput.ToString();
            var match = TotalSampleRegex().Match(output);
            if (match.Success && uint.TryParse(match.Groups[1].Value, out var value))
                return value;

            return null;
        }
        finally
        {
            // Cleanup regardless
            Directory.Delete(workingDirectory, true);
        }
    }
    
    [GeneratedRegex(@"stream total samples: (\d+)")]
    private static partial Regex TotalSampleRegex();
}
