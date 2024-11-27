using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Domain.Entities.Exvs.Units;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Stats.Commands.AmmoSlot;

public record CreateUnitAmmoSlotCommand(uint AmmoHash, uint UnitId, int SlotOrder) : IRequest<Guid>;

public class CreateUnitAmmoSlotCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<CreateUnitAmmoSlotCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateUnitAmmoSlotCommand command, CancellationToken cancellationToken)
    {
        var unitStat = await applicationDbContext.UnitStats
            .FirstOrDefaultAsync(unitStat => unitStat.GameUnitId == command.UnitId, cancellationToken);
        
        // can turn this into an addrange and force users to give us an direct list of ids with orders
        var entity = new UnitAmmoSlot
        {
            AmmoHash = command.AmmoHash,
            SlotOrder = command.SlotOrder,
            UnitStat = unitStat,
        };
        
        applicationDbContext.UnitAmmoSlots.Add(entity);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
