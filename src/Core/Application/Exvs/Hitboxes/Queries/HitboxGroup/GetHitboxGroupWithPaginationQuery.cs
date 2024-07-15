using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Hitboxes.HitboxGroups;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Hitboxes.Queries.HitboxGroup;

public record GetHitboxGroupWithPaginationQuery(
    int Page = 1,
    int PerPage = 10,
    uint[]? Hashes = null,
    uint[]? UnitIds = null
) : IRequest<PaginatedList<HitboxGroupDto>>;

public class GetHitboxGroupWithPaginationQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetHitboxGroupWithPaginationQuery, PaginatedList<HitboxGroupDto>>
{
    public async ValueTask<PaginatedList<HitboxGroupDto>> Handle(GetHitboxGroupWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = applicationDbContext.HitboxGroups
            .Include(group => group.Units)
            .AsQueryable();

        if (request.UnitIds?.Length > 0)
            query = query.Where(group => group.Units.Any(unit => request.UnitIds.Contains(unit.GameUnitId)));
        
        var mappedQueryable = HitboxGroupMapper.ProjectToDto(query);
        var result = await PaginatedList<HitboxGroupDto>
            .CreateAsync(mappedQueryable, request.Page, request.PerPage);

        return result;
    }
}
