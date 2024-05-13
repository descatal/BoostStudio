using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.FhmFormat;
using BoostStudio.Application.Common.Models;

namespace BoostStudio.Application.Formats.FhmFormat.Commands;

public record UnpackFhmToDirectory(byte[] File, string OutputDirectory, bool MultipleFiles) : IRequest;

public record UnpackFhm(byte[] File, bool MultipleFiles, CompressionFormats CompressionFormat) : IRequest<byte[]>;

public class UnpackFhmCommandHandler(
    IFormatBinarySerializer<Fhm> formatBinarySerializer,
    IFhmPacker fhmPacker,
    ICompressor compressor
) : IRequestHandler<UnpackFhmToDirectory>, IRequestHandler<UnpackFhm, byte[]>
{
    public async Task Handle(UnpackFhmToDirectory request, CancellationToken cancellationToken)
    {
        await UnpackFhmInternal(request.File, request.MultipleFiles, request.OutputDirectory, cancellationToken);
    }
    
    public async Task<byte[]> Handle(UnpackFhm request, CancellationToken cancellationToken)
    {
        // Temporary folder to hold the unpacked files
        var rootFhmFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        await UnpackFhmInternal(request.File, request.MultipleFiles, rootFhmFolder, cancellationToken);

        // Create zip file, then delete the temp folder
        var archive = await compressor.CompressAsync(rootFhmFolder, request.CompressionFormat, cancellationToken);
        Directory.Delete(rootFhmFolder, true);

        return archive.ToArray();
    }
    
    private async Task UnpackFhmInternal(
        byte[] file, 
        bool multipleFiles, 
        string rootFhmFolder,
        CancellationToken cancellationToken = default)
    {
        await using var fileStream = new MemoryStream(file);
        if (multipleFiles)
        {
            var index = 1;
            while (fileStream.Position != fileStream.Length)
            {
                var fhmFolder = Path.Combine(rootFhmFolder, $"FHM-{index++:D3}");
                var fhm = await formatBinarySerializer.DeserializeAsync(fileStream, cancellationToken);
                await fhmPacker.UnpackAsync(fhm, fhmFolder, cancellationToken);
            }
        }
        else
        {
            var fhm = await formatBinarySerializer.DeserializeAsync(fileStream, cancellationToken);
            await fhmPacker.UnpackAsync(fhm, rootFhmFolder, cancellationToken);
        }
    }
}
