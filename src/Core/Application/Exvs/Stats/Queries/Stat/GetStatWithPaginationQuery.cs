using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Stats;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Stats.Queries.Stat;

public record GetStatWithPaginationQuery(
    int Page = 1,
    int PerPage = 10,
    Guid[]? Ids = null,
    uint[]? UnitIds = null
) : IRequest<PaginatedList<StatDto>>;

public class GetStatWithPaginationQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetStatWithPaginationQuery, PaginatedList<StatDto>>
{
    public async ValueTask<PaginatedList<StatDto>> Handle(GetStatWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = applicationDbContext.Stats
            .Include(stat => stat.UnitStat)
            .AsQueryable();
        
        if (request.Ids is not null && request.Ids.Length > 0)
            query = query.Where(stat => request.Ids.Contains(stat.Id));

        if (request.UnitIds is not null && request.UnitIds.Length > 0)
            query = query.Where(stat => stat.UnitStat != null && request.UnitIds.Contains(stat.UnitStat.GameUnitId));
        
        var queryableDto = StatMapper.ProjectToDto(query);
        return await PaginatedList<StatDto>.CreateAsync(queryableDto, request.Page, request.PerPage);
    }
}
