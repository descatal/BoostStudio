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
    public async ValueTask<Unit> Handle(UpdateProjectileByIdCommand command, CancellationToken cancellationToken)
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
        
        return default;
    }
}
