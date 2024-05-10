using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.UnitAmmoSlots.Commands;

public record UnitAmmoSlotDetails(int? SlotOrder = null, uint? AmmoHash = null, Guid? UnitStatId = null);

public record UpdateUnitAmmoSlotCommand(Guid Id, UnitAmmoSlotDetails Details) : IRequest;

public class UpdateUnitAmmoSlotCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdateUnitAmmoSlotCommand>
{
    public async Task Handle(UpdateUnitAmmoSlotCommand command, CancellationToken cancellationToken)
    {
        var existingEntity = await applicationDbContext.UnitAmmoSlots
            .FirstOrDefaultAsync(statSet => statSet.Id == command.Id, cancellationToken: cancellationToken);
        Guard.Against.NotFound(command.Id, existingEntity);
        
        if (command.Details.SlotOrder is not null)
            existingEntity.SlotOrder = command.Details.SlotOrder.Value; 
        
        if (command.Details.AmmoHash is not null)
            existingEntity.AmmoHash = command.Details.AmmoHash.Value; 
        
        if (command.Details.UnitStatId is not null)
            existingEntity.UnitStatId = command.Details.UnitStatId.Value; 
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
