using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Stats.Commands.AmmoSlot;

public record DeleteUnitAmmoSlotCommand(Guid Id) : IRequest;

public class DeleteUnitAmmoSlotCommandHandler(IApplicationDbContext applicationDbContext) : IRequestHandler<DeleteUnitAmmoSlotCommand>
{
    public async ValueTask<Unit> Handle(DeleteUnitAmmoSlotCommand request, CancellationToken cancellationToken)
    {
        var entity = await applicationDbContext.UnitAmmoSlots
            .Where(entity => entity.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        
        Guard.Against.NotFound(request.Id, entity);
        
        applicationDbContext.UnitAmmoSlots.Remove(entity);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
