using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Stats;
using BoostStudio.Application.Contracts.Stats.UnitStats;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Stats.Queries.AmmoSlot;

public record GetUnitAmmoSlotByUnitIdQuery(uint UnitId) : IRequest<List<UnitAmmoSlotDto>>;

public class GetUnitAmmoSlotByUnitIdQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetUnitAmmoSlotByUnitIdQuery, List<UnitAmmoSlotDto>>
{
    public async ValueTask<List<UnitAmmoSlotDto>> Handle(GetUnitAmmoSlotByUnitIdQuery request, CancellationToken cancellationToken)
    {
        var unitAmmoSlot = await applicationDbContext.UnitAmmoSlots
            .Where(stat => stat.UnitStat != null && stat.UnitStat.GameUnitId == request.UnitId)
            .OrderBy(entity => entity.SlotOrder)
            .ToListAsync(cancellationToken);
        
        return StatMapper.MapToDto(unitAmmoSlot);
    }
}
