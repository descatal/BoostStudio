using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.TblFormat;
using BoostStudio.Application.Contracts.Metadata.Models;

namespace BoostStudio.Application.Formats.TblFormat.Commands;

public record SerializeTbl(uint CumulativeFileInfoCount, List<TblFileMetadata> FileMetadata, List<string>? PathOrder = null) : IRequest<byte[]>;

public class SerializeTblHandler(
    IFormatBinarySerializer<Tbl> tblBinarySerializer,
    ITblMetadataSerializer tblMetadataSerializer
) : IRequestHandler<SerializeTbl, byte[]>
{
    public async ValueTask<byte[]> Handle(SerializeTbl request, CancellationToken cancellationToken)
    {
        await using var tblStream = new MemoryStream();
        var tblMetadata = new TblMetadata(request.CumulativeFileInfoCount, request.FileMetadata, request.PathOrder);
        var tbl = await tblMetadataSerializer.DeserializeAsync(
            stream: tblStream,
            data: tblMetadata,
            cancellationToken: cancellationToken);

        var packedTbl = await tblBinarySerializer.SerializeAsync(
            data: tbl,
            cancellationToken: cancellationToken);

        return packedTbl;
    }
}
