using System.Formats.Tar;
using System.IO.Compression;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace BoostStudio.Infrastructure.Compressor;

public class Compressor : ICompressor
{
    public Task<byte[]> CompressAsync(string sourceDirectory, CompressionFormats compressionFormat, CancellationToken cancellationToken)
    {
        return compressionFormat switch
        {
            CompressionFormats.Zip => CompressZipAsync(sourceDirectory, cancellationToken: cancellationToken),
            CompressionFormats.Tar => CompressTarAsync(sourceDirectory, cancellationToken: cancellationToken),
            _ => CompressZipAsync(sourceDirectory, cancellationToken: cancellationToken)
        };
    }

    private static Task<byte[]> CompressZipAsync(string sourceDirectory, CancellationToken cancellationToken)
    {
        using var archive = new MemoryStream();
        ZipFile.CreateFromDirectory(sourceDirectory, archive);
        return Task.FromResult(archive.ToArray());
    }

    private static Task<byte[]> CompressTarAsync(string sourceDirectory, CancellationToken cancellationToken)
    {
        using var archive = new MemoryStream();
        TarFile.CreateFromDirectory(sourceDirectory, archive, false);
        return Task.FromResult(archive.ToArray());
    }

    public Task DecompressAsync(byte[] data, string outputDirectory, CancellationToken cancellationToken)
    {
        if (!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);

        using var stream = new MemoryStream(data);
        using var reader = ReaderFactory.Open(stream);
        while (reader.MoveToNextEntry())
        {
            if (reader.Entry.IsDirectory)
                continue;

            reader.WriteEntryToDirectory(outputDirectory, new ExtractionOptions()
            {
                ExtractFullPath = true, Overwrite = true
            });
        }

        return Task.CompletedTask;
    }
}
