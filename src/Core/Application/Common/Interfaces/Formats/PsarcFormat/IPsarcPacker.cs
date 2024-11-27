using BoostStudio.Domain.Enums;

namespace BoostStudio.Application.Common.Interfaces.Formats.PsarcFormat;

public interface IPsarcPacker
{
    Task PackAsync(string sourcePath, string destinationPath, CompressionType compressionType, int compressionLevel, CancellationToken cancellationToken);

    Task UnpackAsync(string sourceFilePath, string destinationPath, CancellationToken cancellationToken);
}
