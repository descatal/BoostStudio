using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Ammo;
using Microsoft.EntityFrameworkCore;
using AmmoMapper=BoostStudio.Application.Contracts.Ammo.AmmoMapper;

namespace BoostStudio.Application.Exvs.Ammo.Commands;

public record UpdateAmmoCommand(uint Hash) : AmmoDetailsDto, IRequest;

public class UpdateAmmoCommandHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<UpdateAmmoCommand>
{
    public async ValueTask<Unit> Handle(UpdateAmmoCommand command, CancellationToken cancellationToken)
    {
        var existingEntity = await applicationDbContext.Ammo
            .Include(ammo => ammo.UnitStat)
            .FirstOrDefaultAsync(ammo => ammo.Hash == command.Hash, cancellationToken: cancellationToken);
        
        Guard.Against.NotFound(command.Hash, existingEntity);
        AmmoMapper.MapToEntity(command.Hash, command, existingEntity);

        if (existingEntity.UnitStat?.GameUnitId != command.UnitId)
        {
            var unitStat = await applicationDbContext.UnitStats
                .FirstOrDefaultAsync(unitStat => unitStat.GameUnitId == command.UnitId, cancellationToken);
            
            existingEntity.UnitStat = unitStat;
        }
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        return default;
    }
}
