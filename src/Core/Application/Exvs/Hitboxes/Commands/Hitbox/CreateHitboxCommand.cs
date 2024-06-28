using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Hitboxes;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Hitboxes.Commands.Hitbox;

public record CreateHitboxCommand() : HitboxDetailsDto, IRequest<Guid>;

public class CreateProjectileCommandHandler(IApplicationDbContext applicationDbContext) : IRequestHandler<CreateHitboxCommand, Guid>
{
    public async Task<Guid> Handle(CreateHitboxCommand command, CancellationToken cancellationToken)
    {
        var entity = HitboxMapper.MapToEntity(command);
        applicationDbContext.Hitboxes.Add(entity);
        
        if (command.UnitId is not null)
        {
            var hitboxGroup = await applicationDbContext.HitboxGroups
                .FirstOrDefaultAsync(unit => unit.GameUnitId == command.UnitId, cancellationToken);
            
            entity.HitboxGroup = hitboxGroup;
        }
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
