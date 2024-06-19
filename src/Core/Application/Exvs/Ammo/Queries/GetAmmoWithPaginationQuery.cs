using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Ammo;
using BoostStudio.Application.Contracts.Stats;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Ammo.Queries;

public record GetAmmoWithPaginationQuery(
    int Page = 1,
    int PerPage = 10,
    uint[]? Hash = null,
    uint[]? UnitIds = null
) : IRequest<PaginatedList<AmmoDto>>;

public class GetAmmoWithPaginationQueryhandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetAmmoWithPaginationQuery, PaginatedList<AmmoDto>>
{
    public async Task<PaginatedList<AmmoDto>> Handle(GetAmmoWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = applicationDbContext.Ammo
            .Include(stat => stat.UnitStat)
            .OrderBy(ammo => ammo.Order)
            .AsQueryable();
        
        if (request.Hash is not null && request.Hash.Length > 0)
            query = query.Where(stat => request.Hash.Contains(stat.Hash));
        
        if (request.UnitIds is not null && request.UnitIds.Length > 0)
            query = query.Where(stat => stat.UnitStat != null && request.UnitIds.Contains(stat.UnitStat.GameUnitId));
        
        var queryableDto = AmmoMapper.ProjectToDto(query);
        return await PaginatedList<AmmoDto>.CreateAsync(queryableDto, request.Page, request.PerPage);
    }
}
