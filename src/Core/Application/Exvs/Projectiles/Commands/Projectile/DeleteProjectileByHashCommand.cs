using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Projectiles.Commands.Projectile;

public record DeleteProjectileByHashCommand(uint Hash) : IRequest;

public class DeleteProjectileByHashCommandHandler(IApplicationDbContext applicationDbContext)
    : IRequestHandler<DeleteProjectileByHashCommand>
{
    public async ValueTask<Unit> Handle(
        DeleteProjectileByHashCommand request,
        CancellationToken cancellationToken
    )
    {
        var projectiles = await applicationDbContext
            .Projectiles.Where(projectile => projectile.Hash == request.Hash)
            .ToListAsync(cancellationToken: cancellationToken);

        applicationDbContext.Projectiles.RemoveRange(projectiles);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
