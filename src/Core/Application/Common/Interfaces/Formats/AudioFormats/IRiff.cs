namespace BoostStudio.Application.Common.Interfaces.Formats.AudioFormats;

public interface IRiff
{
    Task<byte[]> RiffToPcmAsync(
        byte[] riffBinary,
        CancellationToken cancellationToken = default);

    Task<uint?> GetSampleSizeAsync(byte[] riffBinary, CancellationToken cancellationToken);
}
