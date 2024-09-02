using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Interfaces.Formats.TblFormat;
using BoostStudio.Domain.Enums;

namespace BoostStudio.Application.Exvs.PatchFiles.Commands;

public record ImportPatchFileCommand(
    Stream[] Files, 
    PatchFileVersion Version,
    bool UseSubfolderFlag
) : IRequest;

public class ImportPatchFileCommandHandler(
    ITblBinarySerializer binarySerializer,
    ITblMetadataSerializer tblMetadataSerializer,
    IApplicationDbContext applicationDbContext
) : IRequestHandler<ImportPatchFileCommand>
{
    public async ValueTask<Unit> Handle(ImportPatchFileCommand command, CancellationToken cancellationToken)
    {
        foreach (var fileStream in command.Files)
        {
            var binaryData = await binarySerializer.DeserializeAsync(fileStream, command.UseSubfolderFlag, cancellationToken);
            var patchFiles = await tblMetadataSerializer.SerializeAsync(
                binaryData, 
                command.Version, 
                cancellationToken);

            applicationDbContext.PatchFiles.AddRange(patchFiles);
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return default;
        
        // await using var fileStream = new MemoryStream(request.File);
        // var tbl = await binarySerializer.DeserializeAsync(fileStream, request.UseSubfolderFlag, cancellationToken);
        // var asd = await tblMetadataSerializer.SerializeAsync(tbl, cancellationToken);
    }
}
