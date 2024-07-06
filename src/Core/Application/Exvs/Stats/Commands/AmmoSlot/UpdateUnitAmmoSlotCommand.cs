using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Stats.Commands.AmmoSlot;

public record UpdateUnitAmmoSlotCommand(Guid Id, uint UnitId, int? SlotOrder = null, uint? AmmoHash = null) : IRequest;

public class UpdateUnitAmmoSlotCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdateUnitAmmoSlotCommand>
{
    public async Task Handle(UpdateUnitAmmoSlotCommand command, CancellationToken cancellationToken)
    {
        var existingEntity = await applicationDbContext.UnitAmmoSlots
            .FirstOrDefaultAsync(statSet => statSet.Id == command.Id, cancellationToken: cancellationToken);
        Guard.Against.NotFound(command.Id, existingEntity);
        
        existingEntity.SlotOrder = command.SlotOrder ?? 0; 
        existingEntity.AmmoHash = command.AmmoHash ?? 0; 
        
        if (existingEntity.UnitStat?.GameUnitId != command.UnitId)
        {
            var unitStat = await applicationDbContext.UnitStats
                .FirstOrDefaultAsync(unitStat => unitStat.GameUnitId == command.UnitId, cancellationToken);
            
            existingEntity.UnitStat = unitStat; 
        }
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
