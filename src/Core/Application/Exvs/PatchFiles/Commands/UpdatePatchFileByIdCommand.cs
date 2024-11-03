using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Tbl.PatchFiles;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.PatchFiles.Commands;

public record UpdatePatchFileByIdCommand(Guid Id) : PatchFileDto, IRequest;

public class UpdatePatchFileByIdCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdatePatchFileByIdCommand>
{
    public async ValueTask<Unit> Handle(UpdatePatchFileByIdCommand command, CancellationToken cancellationToken)
    {
        var entity = await applicationDbContext.PatchFiles
            .FirstOrDefaultAsync(entity => entity.Id == command.Id, cancellationToken);

        Guard.Against.NotFound(command.Id, entity);
        PatchFilesMapper.Update(command, entity);

        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
