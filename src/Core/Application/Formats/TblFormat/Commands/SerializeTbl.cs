using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Interfaces.Formats.TblFormat;
using BoostStudio.Application.Contracts.Metadata.Models;
using BoostStudio.Formats;

namespace BoostStudio.Application.Formats.TblFormat.Commands;

public record SerializeTbl(uint CumulativeFileInfoCount, List<PatchFileDto> FileMetadata, List<string>? PathOrder = null) : IRequest<byte[]>;

public class SerializeTblHandler(
    ITblBinarySerializer binarySerializer,
    ITblMetadataSerializer tblMetadataSerializer
) : IRequestHandler<SerializeTbl, byte[]>
{
    public async ValueTask<byte[]> Handle(SerializeTbl request, CancellationToken cancellationToken)
    {
        await using var tblStream = new MemoryStream();
        var tblMetadata = new TblDto(request.CumulativeFileInfoCount, request.FileMetadata, request.PathOrder);
        var tbl = await tblMetadataSerializer.DeserializeAsync(
            stream: tblStream,
            data: tblMetadata,
            cancellationToken: cancellationToken);

        var packedTbl = await binarySerializer.SerializeAsync(
            data: tbl,
            cancellationToken: cancellationToken);

        return packedTbl;
    }
}
