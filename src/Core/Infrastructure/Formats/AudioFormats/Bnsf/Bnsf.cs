using System.Diagnostics;
using System.Text;
using BoostStudio.Application.Common.Interfaces.Formats.AudioFormats;
using BoostStudio.Infrastructure.Common;

namespace BoostStudio.Infrastructure.Formats.AudioFormats.Bnsf;

public class Bnsf(IRiff riff) : IBnsf
{
    public async Task<byte[]> ConvertFromRiffAsync(
        byte[] riffBinary,
        uint sampleRate = 48000,
        uint bandwidth = 14000,
        CancellationToken cancellationToken = default)
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(workingDirectory);

        try
        {
            // Use vgmstream to read the sample size
            var sampleSize = await riff.GetSampleSizeAsync(riffBinary, cancellationToken);
            if (sampleSize is null)
                throw new Exception("Failed to get sample size from RIFF!");

            var pcmBinary = await riff.RiffToPcmAsync(riffBinary, cancellationToken);
            
            var pcmFilePath = Path.Combine(workingDirectory, "input.pcm");
            var is14FilePath = Path.Combine(workingDirectory, "output.is14");

            await File.WriteAllBytesAsync(pcmFilePath, pcmBinary, cancellationToken);
            
            var g7221EncoderCliPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "g7221", "encode.exe");
            var arguments = $"0 \"{pcmFilePath}\" \"{is14FilePath}\" {sampleRate} {bandwidth}";
        
            // Execute process
            using var psarcProcess = new Process();
            psarcProcess.StartInfo = new ProcessStartInfo
            {
                Arguments = arguments,
                CreateNoWindow = true,
                FileName = g7221EncoderCliPath,
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

            if (psarcProcess.ExitCode != 0 || !File.Exists(is14FilePath))
                return [];
        
            var is14Binary = await File.ReadAllBytesAsync(is14FilePath, cancellationToken);
            return await ConstructBnsfHeader(is14Binary, sampleSize.Value, cancellationToken: cancellationToken);
        }
        finally
        {
            // Cleanup regardless
            Directory.Delete(workingDirectory, true);
        }
    }
    
    /// <summary>
    /// Construct Bnsf header for is14 (G7221) audio format
    /// </summary>
    /// <param name="audioData">Encoded audio data stream in is14 (G7221) format.</param>
    /// <param name="sampleSize">Sample size of the audio data.</param>
    /// <param name="channelCount">Number of channels, default to 1.</param>
    /// <param name="sampleRate">Sample rate in Hz, default to 48000.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private static async Task<byte[]> ConstructBnsfHeader(
        byte[] audioData,
        uint sampleSize,
        uint channelCount = 1,
        uint sampleRate = 48000,
        CancellationToken cancellationToken = default)
    {
        await using var metadataStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);

        // the total stream size excluding the magic and the 4 byte to record its size.
        // BNSF header will always have 0x30 in length so 0x30 - 0x8 = 0x28;
        var size = (uint)(audioData.Length + 0x28);
        var dataSize = (uint)(audioData.Length);
        var blockSize = (ushort)(120 * channelCount); // 120 is the singular channel block size
        
        metadataStream.WriteUint(0x424E5346); // Magic, "BNSF"
        metadataStream.WriteUint(size); // Fixed property count, is the actual property count - 1
        metadataStream.WriteUint(0x49533134); // Codec magic, "IS14"
        metadataStream.WriteUint(0x73666D74); // Sfmt magic, "sfmt"
        metadataStream.WriteUint(0x14); // Sfmt size, will always be 0x14 in length
        metadataStream.WriteUint(channelCount);
        metadataStream.WriteUint(sampleRate);
        metadataStream.WriteUint(sampleSize);
        metadataStream.WriteUint(0); // should be related to loop stuff, but omit for now
        metadataStream.WriteUshort(blockSize);
        metadataStream.WriteUshort(640); // block size, always 0x280?
        
        // TODO: Add loop information
        
        metadataStream.WriteUint(0x73646174); // Sdat magic, "sdat"
        metadataStream.WriteUint(dataSize);

        // Concatenate the file metadata stream with the file body stream
        await using var fileStream = new CustomBinaryWriter(new MemoryStream(), Endianness.BigEndian);
        await fileStream.ConcatenateStreamAsync(metadataStream.Stream);
        fileStream.WriteByteArray(audioData);

        return fileStream.ToByteArray();
    }
}
