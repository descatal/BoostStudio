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
    IFormatBinarySerializer<Tbl> formatBinarySerializer,
    ITblMetadataSerializer tblMetadataSerializer
) : IRequestHandler<DeserializeTbl, TblMetadata>
{
    public async ValueTask<TblMetadata> Handle(DeserializeTbl request, CancellationToken cancellationToken)
    {
        await using var fileStream = new MemoryStream(request.File);
        var tbl = await formatBinarySerializer.DeserializeAsync(fileStream, cancellationToken);
        return await tblMetadataSerializer.SerializeAsync(tbl, cancellationToken);
    }
}
