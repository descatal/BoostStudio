using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.UnitAmmoSlots.Commands;

public record UpdateUnitAmmoSlotCommand(Guid Id, uint AmmoHash, Guid UnitStatId) : IRequest;

public class UpdateUnitAmmoSlotCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdateUnitAmmoSlotCommand>
{
    public async Task Handle(UpdateUnitAmmoSlotCommand command, CancellationToken cancellationToken)
    {
        var existingEntity = await applicationDbContext.UnitAmmoSlots
            .FirstOrDefaultAsync(statSet => statSet.Id == command.Id, cancellationToken: cancellationToken);
        Guard.Against.NotFound(command.Id, existingEntity);
        
        existingEntity.AmmoHash = command.AmmoHash; 
        existingEntity.UnitStatId = command.UnitStatId; 
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
