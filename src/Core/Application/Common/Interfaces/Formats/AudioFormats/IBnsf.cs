namespace BoostStudio.Application.Common.Interfaces.Formats.AudioFormats;

public interface IBnsf
{
    Task<byte[]> ConvertFromRiffAsync(
        byte[] riffBinary,
        uint sampleRate = 48000,
        uint bandwidth = 14000,
        CancellationToken cancellationToken = default);
}
