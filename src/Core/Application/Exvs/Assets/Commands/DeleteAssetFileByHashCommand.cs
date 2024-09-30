using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Assets.Commands;

public record DeleteAssetFileByHashCommand(uint Hash) : IRequest;

public class DeleteAssetFileByHashCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<DeleteAssetFileByHashCommand>
{
    public async ValueTask<Unit> Handle(DeleteAssetFileByHashCommand command, CancellationToken cancellationToken)
    {
        var entity = await applicationDbContext.AssetFiles
            .FirstOrDefaultAsync(entity => entity.Hash == command.Hash, cancellationToken);

        Guard.Against.NotFound(command.Hash.ToString(), entity);

        applicationDbContext.AssetFiles.Remove(entity);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
