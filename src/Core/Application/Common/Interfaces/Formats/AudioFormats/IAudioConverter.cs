using BoostStudio.Application.Common.Enums;

namespace BoostStudio.Application.Common.Interfaces.Formats.AudioFormats;

public interface IAudioConverter
{
    Task<byte[]> ConvertAsync(
        byte[] audioBinary,
        AudioFormat targetFormat,
        CancellationToken cancellationToken);

    AudioFormat GetAudioFormat(Stream audioFileStream);
}
