using System.ComponentModel.DataAnnotations;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.Fhm;
using BoostStudio.Application.Common.Models;

namespace BoostStudio.Application.Formats.Fhm.Commands;

public record UnpackFhm(byte[] File, bool MultipleFiles, CompressionFormats CompressionFormat) : IRequest<byte[]>
{
    [Required] 
    public byte[] File { get; } = File;

    public bool MultipleFiles { get; } = MultipleFiles;

    public CompressionFormats CompressionFormat { get; } = CompressionFormat;
}

public class UnpackFhmCommandHandler(
    IFormatSerializer<Contracts.Fhm> formatSerializer, 
    IFhmPacker fhmPacker, 
    ICompressor compressor) 
    : IRequestHandler<UnpackFhm, byte[]>
{
    private readonly IFormatSerializer<Contracts.Fhm> _formatSerializer = formatSerializer;
    private readonly IFhmPacker _fhmPacker = fhmPacker;
    private readonly ICompressor _compressor = compressor;

    public async Task<byte[]> Handle(UnpackFhm request, CancellationToken cancellationToken)
    {
        // Temporary folder to hold the unpacked files
        var rootFhmFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        if (request.MultipleFiles)
        {
            await using var fileStream = new MemoryStream(request.File);
            var index = 1;
            while (fileStream.Position != fileStream.Length)
            {
                var fhmFolder = Path.Combine(rootFhmFolder, $"FHM-{index++:D3}");
                var fhm = await _formatSerializer.DeserializeAsync(fileStream, cancellationToken);
                await _fhmPacker.UnpackAsync(fhm, fhmFolder, cancellationToken);
            }
        }
        else
        {
            var fhm = await _formatSerializer.DeserializeAsync(request.File, cancellationToken);
            await _fhmPacker.UnpackAsync(fhm, rootFhmFolder, cancellationToken);
        }
        
        // Create zip file, then delete the temp folder
        var archive = await _compressor.CompressAsync(rootFhmFolder, CompressionFormats.Zip, cancellationToken);
        Directory.Delete(rootFhmFolder, true);
        
        return archive.ToArray();
    }
}
