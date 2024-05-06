using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Domain.Entities.Unit;

namespace BoostStudio.Application.Exvs.UnitAmmoSlots.Commands;

public record CreateUnitAmmoSlotCommand(uint AmmoHash, Guid UnitStatId, int SlotOrder) : IRequest<Guid>;

public class CreateUnitAmmoSlotCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<CreateUnitAmmoSlotCommand, Guid>
{
    public async Task<Guid> Handle(CreateUnitAmmoSlotCommand command, CancellationToken cancellationToken)
    {
        // Can turn this into an addrange and force users to give us an direct list of ids with orders
        var entity = new UnitAmmoSlot
        {
            AmmoHash = command.AmmoHash,
            UnitStatId = command.UnitStatId,
            SlotOrder = command.SlotOrder
        };
        
        applicationDbContext.UnitAmmoSlots.Add(entity);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
