using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Tbl.PatchFiles;

namespace BoostStudio.Application.Exvs.PatchFiles.Commands;

public record CreatePatchFileCommand() : PatchFileDto, IRequest;

public class CreatePatchFileCommandHandler(IApplicationDbContext applicationDbContext)
    : IRequestHandler<CreatePatchFileCommand>
{
    public async ValueTask<Unit> Handle(
        CreatePatchFileCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = PatchFilesMapper.ToEntity(command);

        if (
            entity.PathInfo is not null
            && command.PathInfo is not null
            && command.PathInfo.Order is null
        )
        {
            var largestPatchPathOrder = applicationDbContext
                .PatchFiles.Where(patchFile => patchFile.TblId == command.TblId)
                .OrderByDescending(patchFile => patchFile.PathInfo!.Order)
                .Select(patchFile => patchFile.PathInfo!.Order)
                .FirstOrDefault();

            entity.PathInfo.Order = largestPatchPathOrder + 1;
        }

        await applicationDbContext.PatchFiles.AddAsync(entity, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
