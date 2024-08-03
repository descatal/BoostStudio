using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Projectiles;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Projectiles.Commands.Projectile;

public record UpdateProjectileByIdCommand(uint Hash) : ProjectileDetailsDto, IRequest;

public class UpdateProjectileCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdateProjectileByIdCommand>
{
    public async ValueTask<Unit> Handle(UpdateProjectileByIdCommand byIdCommand, CancellationToken cancellationToken)
    {
        var existingEntity = await applicationDbContext.Projectiles
            .Include(projectile => projectile.UnitProjectile)
            .FirstOrDefaultAsync(projectileSet => projectileSet.Hash == byIdCommand.Hash, cancellationToken: cancellationToken);
        
        Guard.Against.NotFound(byIdCommand.Hash, existingEntity);
        ProjectileMapper.MapToEntity(byIdCommand.Hash, byIdCommand, existingEntity);

        if (existingEntity.UnitProjectile?.GameUnitId != byIdCommand.UnitId)
        {
            var unitProjectile = await applicationDbContext.UnitProjectiles
                .FirstOrDefaultAsync(unitProjectile => unitProjectile.GameUnitId == byIdCommand.UnitId, cancellationToken);
            
            existingEntity.UnitProjectile = unitProjectile;
        }
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        return default;
    }
}
