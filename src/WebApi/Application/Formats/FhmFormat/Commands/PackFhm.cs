using System.ComponentModel.DataAnnotations;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.FhmFormat;
using BoostStudio.Application.Common.Models;

namespace BoostStudio.Application.Formats.FhmFormat.Commands;

public record PackFhm(byte[] File) : IRequest<byte[]>
{
    [Required]
    public byte[] File { get; } = File;
}

public class PackFhmCommandHandler(
    IFormatSerializer<Fhm> formatSerializer,
    IFhmPacker fhmPacker,
    ICompressor compressor) : IRequestHandler<PackFhm, byte[]>
{
    private readonly IFormatSerializer<Fhm> _formatSerializer = formatSerializer;
    private readonly IFhmPacker _fhmPacker = fhmPacker;
    private readonly ICompressor _compressor = compressor;

    public async Task<byte[]> Handle(PackFhm request, CancellationToken cancellationToken)
    {
        // Temporary folder to hold the extracted files
        var extractFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        using var stream = new MemoryStream();
        await _compressor.DecompressAsync(request.File, extractFolder, cancellationToken);
        var packedFhm = await _fhmPacker.PackAsync(stream, extractFolder, cancellationToken);

        var serializedFhm = await _formatSerializer.SerializeAsync(packedFhm, cancellationToken);

        Directory.Delete(extractFolder, true);
        return serializedFhm.ToArray();
    }
}
