using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Ammo;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Ammo.Commands;

public record UpdateAmmoCommand(uint AmmoHash, AmmoDto Ammo, Guid? UnitStatId = null) : IRequest;

public class UpdateAmmoCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdateAmmoCommand>
{
    public async Task Handle(UpdateAmmoCommand command, CancellationToken cancellationToken)
    {
        var existingEntity = await applicationDbContext.Ammo
            .FirstOrDefaultAsync(ammo => ammo.Hash == command.AmmoHash, cancellationToken: cancellationToken);
        Guard.Against.NotFound(command.AmmoHash, existingEntity);

        var ammoMapper = new AmmoMapper();
        existingEntity = ammoMapper.AmmoDtoToAmmo(command.Ammo);
        existingEntity.UnitStatId = command.UnitStatId; 
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
