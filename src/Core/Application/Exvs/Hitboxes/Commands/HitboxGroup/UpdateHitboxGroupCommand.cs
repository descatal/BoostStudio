using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Hitboxes.Commands.HitboxGroup;

public record UpdateHitboxGroupCommand(uint Hash, uint[]? UnitIds = null) : IRequest;

public class UpdateHitboxGroupCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdateHitboxGroupCommand>
{
    public async ValueTask<Unit> Handle(UpdateHitboxGroupCommand command, CancellationToken cancellationToken)
    {
        var entity = applicationDbContext.HitboxGroups
            .FirstOrDefault(group => group.Hash == command.Hash);

        Guard.Against.NotFound(command.Hash, entity);

        var unitIds = command.UnitIds ?? [];
        
        // reverse parental assignment
        var existingUnits = await applicationDbContext.Units
            .Where(x => unitIds.Contains(x.GameUnitId))
            .ToListAsync(cancellationToken);
        
        entity.Units = existingUnits;
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        return default;
    }
}

