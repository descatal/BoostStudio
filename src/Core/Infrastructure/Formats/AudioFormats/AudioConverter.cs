using System.Buffers.Binary;
using BoostStudio.Application.Common.Enums;
using BoostStudio.Application.Common.Interfaces.Formats.AudioFormats;

namespace BoostStudio.Infrastructure.Formats.AudioFormats;

public class AudioConverter(IBnsf bnsf) : IAudioConverter
{
    public async Task<byte[]> ConvertAsync(
        byte[] audioBinary, 
        AudioFormat targetFormat, 
        CancellationToken cancellationToken)
    {
        using var audioFileStream = new MemoryStream(audioBinary);
        var sourceFormat = GetAudioFormat(audioFileStream);
        
        if (sourceFormat == targetFormat)
            return audioBinary;
        
        var baseFormatAudioBinary = await ConvertToBaseFormatAsync(audioBinary, sourceFormat, cancellationToken);

        return targetFormat switch
        {
            // Convert to is14 (G7221) format and construct BNSF headers
            AudioFormat.Bnsf => await bnsf.ConvertFromRiffAsync(baseFormatAudioBinary, cancellationToken: cancellationToken),
            _ => throw new Exception($"Conversion from {sourceFormat} to {targetFormat} is currently unsupported!")
        };
    }

    /// <summary>
    /// Convert audio format regardless of input source to a common format, which is RIFF
    /// </summary>
    /// <returns></returns>
    private async Task<byte[]> ConvertToBaseFormatAsync(
        byte[] audioBinary, 
        AudioFormat audioFormat,
        CancellationToken cancellationToken = default)
    {
        return audioFormat switch
        {
            // Already in Riff, just return
            AudioFormat.Riff => audioBinary,
            _ => throw new Exception($"Conversion from {audioFormat} to base format (PCM) is currently unsupported!")
        };
    }
    
    public AudioFormat GetAudioFormat(Stream audioFileStream)
    {
        var originalPosition = audioFileStream.Position;
        
        var binaryReader = new BinaryReader(audioFileStream);
        var fileMagic = binaryReader.ReadUInt32();
        fileMagic = BinaryPrimitives.ReverseEndianness(fileMagic);
        
        audioFileStream.Seek(originalPosition, SeekOrigin.Begin);
        
        return fileMagic switch
        {
            0x52494646 => AudioFormat.Riff,
            0x424E5346 => AudioFormat.Bnsf,
            _ => AudioFormat.Unknown
        };
    }
}
