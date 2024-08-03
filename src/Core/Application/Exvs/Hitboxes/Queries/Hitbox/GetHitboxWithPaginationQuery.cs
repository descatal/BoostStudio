using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Hitboxes;
using BoostStudio.Application.Contracts.Projectiles;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Hitboxes.Queries.Hitbox;

public record GetHitboxWithPaginationQuery(
    int Page = 1,
    int PerPage = 10,
    uint[]? Hashes = null,
    uint[]? UnitIds = null,
    string? Search = null
) : IRequest<PaginatedList<HitboxDto>>;

public class GetHitboxWithPaginationQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetHitboxWithPaginationQuery, PaginatedList<HitboxDto>>
{
    public async ValueTask<PaginatedList<HitboxDto>> Handle(GetHitboxWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = applicationDbContext.Hitboxes
            .Include(entity => entity.HitboxGroup)
            .ThenInclude(hitbox => hitbox!.Units)
            .AsQueryable();

        if (request.UnitIds?.Length > 0)
        {
            query = query
                .Where(hitbox => hitbox.HitboxGroup != null)
                .Where(hitbox => hitbox.HitboxGroup!.Units.Any(unit => request.UnitIds.Contains(unit.GameUnitId)));
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(entity => entity.Hash.ToString().ToLower().Contains(request.Search));
        
        if (request.Hashes is not null && request.Hashes.Length > 0)
            query = query.Where(projectile => request.Hashes.Contains(projectile.Hash));

        var queryableDto = HitboxMapper.ProjectToDto(query);
        return await PaginatedList<HitboxDto>.CreateAsync(queryableDto, request.Page, request.PerPage);
    }
}
