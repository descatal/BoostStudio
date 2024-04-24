using System.ComponentModel.DataAnnotations;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.FhmFormat;
using BoostStudio.Application.Common.Models;

namespace BoostStudio.Application.Formats.FhmFormat.Commands;

public record UnpackFhm(byte[] File, bool MultipleFiles, CompressionFormats CompressionFormat) : IRequest<byte[]>;

public class UnpackFhmCommandHandler(
    IFormatBinarySerializer<Fhm> formatBinarySerializer,
    IFhmPacker fhmPacker,
    ICompressor compressor
) : IRequestHandler<UnpackFhm, byte[]>
{
    public async Task<byte[]> Handle(UnpackFhm request, CancellationToken cancellationToken)
    {
        // Temporary folder to hold the unpacked files
        var rootFhmFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        await using var fileStream = new MemoryStream(request.File);
        if (request.MultipleFiles)
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

        // Create zip file, then delete the temp folder
        var archive = await compressor.CompressAsync(rootFhmFolder, request.CompressionFormat, cancellationToken);
        Directory.Delete(rootFhmFolder, true);

        return archive.ToArray();
    }
}
