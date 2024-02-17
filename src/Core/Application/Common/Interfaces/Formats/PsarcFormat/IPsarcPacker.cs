using BoostStudio.Domain.Entities.PsarcFormat;

namespace BoostStudio.Application.Common.Interfaces.Formats.PsarcFormat;

public interface IPsarcPacker
{
    Task<byte[]> PackAsync(string sourcePath, CompressionType compressionType, int compressionLevel, CancellationToken cancellationToken);

    Task<byte[]> UnpackAsync(byte[] sourceFile, CancellationToken cancellationToken);
}
