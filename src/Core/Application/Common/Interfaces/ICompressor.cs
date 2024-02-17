using BoostStudio.Application.Common.Models;

namespace BoostStudio.Application.Common.Interfaces;

public interface ICompressor
{
    Task<byte[]> CompressAsync(string sourceDirectory, CompressionFormats compressionFormat, CancellationToken cancellationToken = default);

    Task DecompressAsync(byte[] data, string outputDirectory, CancellationToken cancellationToken = default);
}
