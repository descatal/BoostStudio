using BoostStudio.Application.Common.Models;
using FileInfo=BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Common.Interfaces;

public interface ICompressor
{
    Task<byte[]> CompressAsync(List<FileInfo> files, CompressionFormats compressionFormat, CancellationToken cancellationToken);
    
    Task<byte[]> CompressAsync(string sourceDirectory, CompressionFormats compressionFormat, CancellationToken cancellationToken = default);

    Task DecompressAsync(byte[] data, string outputDirectory, CancellationToken cancellationToken = default);

    Task<List<FileInfo>> DecompressAsync(byte[] data, CancellationToken cancellationToken = default);
}
