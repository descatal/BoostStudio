using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Mappers;
using BoostStudio.Application.Contracts.Units;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.UnitStats.Queries;

public record GetUnitStatQuery(
    int Page = 1,
    int PerPage = 10,
    uint[]? GameUnitIds = null
) : IRequest<PaginatedList<UnitStatDto>>;

public class GetUnitStatQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetUnitStatQuery, PaginatedList<UnitStatDto>>
{
    public async Task<PaginatedList<UnitStatDto>> Handle(GetUnitStatQuery request, CancellationToken cancellationToken)
    {
        var unitStatsQueryable = applicationDbContext.UnitStats
            .AsQueryable();

        if (request.GameUnitIds is not null && request.GameUnitIds.Length != 0)
            unitStatsQueryable = unitStatsQueryable.Where(unitStat => request.GameUnitIds.Contains(unitStat.GameUnitId));
        
        var mappedQueryable = UnitStatsMapper.ProjectToDto(unitStatsQueryable);
        
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
