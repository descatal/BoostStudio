using System.ComponentModel.DataAnnotations;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.FhmFormat;
using BoostStudio.Application.Common.Interfaces.Formats.TblFormat;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Metadata.Models;

namespace BoostStudio.Application.Formats.TblFormat.Commands;

public record DeserializeTbl(byte[] File) : IRequest<TblMetadata>
{
    [Required]
    public byte[] File { get; } = File;
}

public class DeserializeTblCommandHandler(
    IFormatSerializer<Tbl> formatSerializer,
    ITblMetadataSerializer tblMetadataSerializer)
    : IRequestHandler<DeserializeTbl, TblMetadata>
{
    private readonly IFormatSerializer<Tbl> _formatSerializer = formatSerializer;
    private readonly ITblMetadataSerializer _tblMetadataSerializer = tblMetadataSerializer;

    public async Task<TblMetadata> Handle(DeserializeTbl request, CancellationToken cancellationToken)
    {
        await using var fileStream = new MemoryStream(request.File);
        var tbl = await _formatSerializer.DeserializeAsync(fileStream, cancellationToken);
        return await _tblMetadataSerializer.SerializeAsync(tbl, cancellationToken);
    }
}
