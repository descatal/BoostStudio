using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Stats.UnitStats;

namespace BoostStudio.Application.Exvs.Stats.Queries.UnitStat;

public record GetUnitStatWithPaginationQuery(
    int Page = 1,
    int PerPage = 10,
    uint[]? UnitIds = null
) : IRequest<PaginatedList<UnitStatDto>>;

public class GetUnitStatWithPainationQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetUnitStatWithPaginationQuery, PaginatedList<UnitStatDto>>
{
    public async Task<PaginatedList<UnitStatDto>> Handle(GetUnitStatWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var unitStatsQueryable = applicationDbContext.UnitStats
            .AsQueryable();

        if (request.UnitIds is not null && request.UnitIds.Length != 0)
            unitStatsQueryable = unitStatsQueryable.Where(unitStat => request.UnitIds.Contains(unitStat.GameUnitId));
        
        var mappedQueryable = UnitStatMapper.ProjectToDto(unitStatsQueryable);
        var result = await PaginatedList<UnitStatDto>
            .CreateAsync(mappedQueryable, request.Page, request.PerPage);
        
        foreach (var unitStatDto in result.Items)
        {
            unitStatDto.Ammo = unitStatDto.Ammo.OrderBy(ammo => ammo.Order).ToList();
            unitStatDto.AmmoSlots = unitStatDto.AmmoSlots.OrderBy(ammo => ammo.SlotOrder).ToList();
        }

        return result;
    }
}
