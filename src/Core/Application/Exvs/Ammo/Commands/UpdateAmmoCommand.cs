using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Ammo;
using Microsoft.EntityFrameworkCore;
using AmmoMapper=BoostStudio.Application.Contracts.Mappers.AmmoMapper;

namespace BoostStudio.Application.Exvs.Ammo.Commands;

public record AmmoDetails(Guid UnitStatId) : AmmoDetailsDto;

public record UpdateAmmoCommand(uint AmmoHash, AmmoDetails Details) : IRequest;

public class UpdateAmmoCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdateAmmoCommand>
{
    public async Task Handle(UpdateAmmoCommand command, CancellationToken cancellationToken)
    {
        var existingEntity = await applicationDbContext.Ammo
            .AsTracking()
            .FirstOrDefaultAsync(ammo => ammo.Hash == command.AmmoHash, cancellationToken: cancellationToken);
        Guard.Against.NotFound(command.AmmoHash, existingEntity);

        var ammoMapper = new AmmoMapper();
        var mappedEntity = ammoMapper.AmmoDetailsDtoToAmmo(command.Details);
        ammoMapper.AmmoToAmmo(mappedEntity, existingEntity);
        existingEntity.UnitStatId = command.Details.UnitStatId;
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
