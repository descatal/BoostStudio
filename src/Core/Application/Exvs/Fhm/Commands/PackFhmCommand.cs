using System.ComponentModel.DataAnnotations;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.FhmFormat;

namespace BoostStudio.Application.Exvs.Fhm.Commands;

public record PackFhmCommand(byte[] File) : IRequest<byte[]>
{
    [Required]
    public byte[] File { get; } = File;
}

public class PackFhmCommandHandler(
    IFormatBinarySerializer<BoostStudio.Formats.Fhm> formatBinarySerializer,
    IFhmPacker fhmPacker,
    ICompressor compressor
) : IRequestHandler<PackFhmCommand, byte[]>
{
    public async ValueTask<byte[]> Handle(PackFhmCommand request, CancellationToken cancellationToken)
    {
        // Temporary folder to hold the extracted files
        var extractFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        using var stream = new MemoryStream();
        await compressor.DecompressAsync(request.File, extractFolder, cancellationToken);
        var packedFhm = await fhmPacker.PackAsync(stream, extractFolder, cancellationToken);

        var serializedFhm = await formatBinarySerializer.SerializeAsync(
            packedFhm,
            cancellationToken
        );

        Directory.Delete(extractFolder, true);
        return serializedFhm.ToArray();
    }
}
