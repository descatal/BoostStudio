using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Interfaces.Formats.TblFormat;
using BoostStudio.Application.Contracts.Metadata.Models;

namespace BoostStudio.Application.Exvs.Tbl.Commands;

public record SerializeTblCommand(
    uint CumulativeFileInfoCount,
    List<PatchFileMetadataDto> FileMetadata,
    List<string>? PathOrder = null
) : IRequest<byte[]>;

public class SerializeTblHandler(
    ITblBinarySerializer binarySerializer,
    ITblMetadataSerializer tblMetadataSerializer
) : IRequestHandler<SerializeTblCommand, byte[]>
{
    public async ValueTask<byte[]> Handle(
        SerializeTblCommand request,
        CancellationToken cancellationToken
    )
    {
        await using var tblStream = new MemoryStream();
        var tblMetadata = new TblDto(
            request.CumulativeFileInfoCount,
            request.FileMetadata,
            request.PathOrder
        );
        var tbl = await tblMetadataSerializer.DeserializeDtoAsync(
            stream: tblStream,
            data: tblMetadata,
            cancellationToken: cancellationToken
        );

        var packedTbl = await binarySerializer.SerializeAsync(
            data: tbl,
            cancellationToken: cancellationToken
        );

        return packedTbl;
    }
}
