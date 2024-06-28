using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Hitboxes;
using BoostStudio.Application.Contracts.Projectiles;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Projectiles.Commands.Projectile;

public record UpdateProjectileCommand(uint Hash) : ProjectileDetailsDto, IRequest;

public class UpdateProjectileCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdateProjectileCommand>
{
    public async Task Handle(UpdateProjectileCommand command, CancellationToken cancellationToken)
    {
        var existingEntity = await applicationDbContext.Projectiles
            .Include(projectile => projectile.UnitProjectile)
            .FirstOrDefaultAsync(projectileSet => projectileSet.Hash == command.Hash, cancellationToken: cancellationToken);
        
        Guard.Against.NotFound(command.Hash, existingEntity);
        ProjectileMapper.MapToEntity(command.Hash, command, existingEntity);

        if (existingEntity.UnitProjectile?.GameUnitId != command.UnitId)
        {
            var unitProjectile = await applicationDbContext.UnitProjectiles
                .FirstOrDefaultAsync(unitProjectile => unitProjectile.GameUnitId == command.UnitId, cancellationToken);
            
            existingEntity.UnitProjectile = unitProjectile;
        }
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
