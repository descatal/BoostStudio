using System.Formats.Tar;
using System.IO.Compression;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using SharpCompress.Common;
using SharpCompress.Readers;
using FileInfo=BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Infrastructure.Compressor;

public class Compressor : ICompressor
{
    public async Task<byte[]> CompressAsync(List<FileInfo> files, CompressionFormats compressionFormat, CancellationToken cancellationToken)
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(workingDirectory);

        foreach (var fileInfo in files)
        {
            var filePath = Path.Combine(workingDirectory, fileInfo.FileName);
            await File.WriteAllBytesAsync(filePath, fileInfo.Data, cancellationToken);
        }
        
        try
        {
            return compressionFormat switch
            {
                CompressionFormats.Zip => await CompressZipAsync(workingDirectory, cancellationToken: cancellationToken),
                CompressionFormats.Tar => await CompressTarAsync(workingDirectory, cancellationToken: cancellationToken),
                _ => await CompressZipAsync(workingDirectory, cancellationToken: cancellationToken)
            };
        }
        finally
        {
            // Will still throw exception, just want to make sure the directory is cleaned
            Directory.Delete(workingDirectory, true);
        }
    }
    
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
