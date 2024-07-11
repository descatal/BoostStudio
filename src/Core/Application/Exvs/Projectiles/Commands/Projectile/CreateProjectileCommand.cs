using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Hitboxes;
using BoostStudio.Application.Contracts.Projectiles;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Projectiles.Commands.Projectile;

public record CreateProjectileCommand() : ProjectileDetailsDto, IRequest<Guid>;

public class CreateProjectileCommandHandler(IApplicationDbContext applicationDbContext) : IRequestHandler<CreateProjectileCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateProjectileCommand command, CancellationToken cancellationToken)
    {
        var entity = ProjectileMapper.MapToEntity(command);
        applicationDbContext.Projectiles.Add(entity);
        
        if (command.UnitId is not null)
        {
            var unitProjectile = await applicationDbContext.UnitProjectiles
                .FirstOrDefaultAsync(unitProjectile => unitProjectile.GameUnitId == command.UnitId, cancellationToken);
            
            entity.UnitProjectile = unitProjectile;
        }
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
