using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Stats.UnitStats;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Stats.Queries.UnitStat;

public record GetUnitStatByUnitIdQuery(uint UnitId) : IRequest<UnitStatDto>;

public class GetUnitStatByUnitIdQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetUnitStatByUnitIdQuery, UnitStatDto>
{
    public async Task<UnitStatDto> Handle(GetUnitStatByUnitIdQuery request, CancellationToken cancellationToken)
    {
        var unitStatsQueryable = applicationDbContext.UnitStats
            .Where(unitStat => request.UnitId == unitStat.GameUnitId);
        
        var mappedQueryable = UnitStatMapper.ProjectToDto(unitStatsQueryable);
        var result = await mappedQueryable.FirstOrDefaultAsync(cancellationToken);
        Guard.Against.NotFound(request.UnitId, result);
    
        result.Ammo = result.Ammo.OrderBy(ammo => ammo.Order).ToList();
        result.AmmoSlots = result.AmmoSlots.OrderBy(ammo => ammo.SlotOrder).ToList();
        return result;
    }
}
