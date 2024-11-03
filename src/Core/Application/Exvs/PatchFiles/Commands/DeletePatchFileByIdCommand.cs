using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.PatchFiles.Commands;

public record DeletePatchFileByIdCommand(Guid Id) : IRequest;

public class DeletePatchFileByIdCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<DeletePatchFileByIdCommand>
{
    public async ValueTask<Unit> Handle(DeletePatchFileByIdCommand command, CancellationToken cancellationToken)
    {
        var entity = await applicationDbContext.PatchFiles
            .FirstOrDefaultAsync(entity => entity.Id == command.Id, cancellationToken);

        Guard.Against.NotFound(command.Id, entity);

        applicationDbContext.PatchFiles.Remove(entity);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
