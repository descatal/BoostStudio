using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using HitboxGroupEntity = BoostStudio.Domain.Entities.Exvs.Hitboxes.HitboxGroup;

namespace BoostStudio.Application.Exvs.Hitboxes.Commands.HitboxGroup;

public record CreateHitboxGroupCommand(uint Hash, uint[]? UnitIds = null) : IRequest;

public class CreateHitboxGroupCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<CreateHitboxGroupCommand>
{
    public async ValueTask<Unit> Handle(CreateHitboxGroupCommand command, CancellationToken cancellationToken)
    {
        var unitIds = command.UnitIds ?? [];
        
        // reverse parental assignment
        var existingUnits = await applicationDbContext.Units
            .Where(x => unitIds.Contains(x.GameUnitId))
            .ToListAsync(cancellationToken);
        
        var entity = new HitboxGroupEntity
        {
            Hash = command.Hash, 
            Units = existingUnits,
        };
        applicationDbContext.HitboxGroups.Add(entity);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        return default;
    }
}

