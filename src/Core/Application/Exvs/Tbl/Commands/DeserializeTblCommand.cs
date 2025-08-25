using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Interfaces.Formats.TblFormat;
using BoostStudio.Application.Contracts.Metadata.Models;

namespace BoostStudio.Application.Exvs.Tbl.Commands;

public record DeserializeTblCommand(byte[] File) : IRequest<TblDto>;

public class DeserializeTblCommandHandler(
    ITblBinarySerializer binarySerializer,
    ITblMetadataSerializer tblMetadataSerializer
) : IRequestHandler<DeserializeTblCommand, TblDto>
{
    public async ValueTask<TblDto> Handle(
        DeserializeTblCommand request,
        CancellationToken cancellationToken
    )
    {
        await using var fileStream = new MemoryStream(request.File);
        var tbl = await binarySerializer.DeserializeAsync(fileStream, cancellationToken);
        return await tblMetadataSerializer.SerializeDtoAsync(tbl, cancellationToken);
    }
}
