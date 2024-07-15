using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Hitboxes;
using BoostStudio.Application.Contracts.Projectiles;
using BoostStudio.Domain.Entities.Unit.Projectiles;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Hitboxes.Commands.Hitbox;

public record UpdateHitboxCommand(uint Hash) : HitboxDetailsDto, IRequest;

public class UpdateHitboxCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdateHitboxCommand>
{
    public async ValueTask<Unit> Handle(UpdateHitboxCommand command, CancellationToken cancellationToken)
    {
        var existingEntity = await applicationDbContext.Hitboxes
            .Include(hitbox => hitbox.HitboxGroup)
            .FirstOrDefaultAsync(projectileSet => projectileSet.Hash == command.Hash, cancellationToken: cancellationToken);
        
        Guard.Against.NotFound(command.Hash, existingEntity);
        HitboxMapper.MapToEntity(command.Hash, command, existingEntity);
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        return default;
    }
}
