using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Hitboxes;
using BoostStudio.Application.Contracts.Projectiles;

namespace BoostStudio.Application.Exvs.Hitboxes.Queries.Hitbox;

public record GetHitboxWithPaginationQuery(
    int Page = 1,
    int PerPage = 10,
    uint[]? Hashes = null,
    uint[]? UnitIds = null
) : IRequest<PaginatedList<HitboxDto>>;

public class GetHitboxWithPaginationQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetHitboxWithPaginationQuery, PaginatedList<HitboxDto>>
{
    public async Task<PaginatedList<HitboxDto>> Handle(GetHitboxWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = applicationDbContext.Hitboxes.AsQueryable();

        if (request.UnitIds is not null && request.UnitIds.Length > 0)
            query = query.Where(hitbox => hitbox.HitboxGroup != null && hitbox.HitboxGroup.GameUnitId != null && request.UnitIds.Contains(hitbox.HitboxGroup.GameUnitId.Value));

        if (request.Hashes is not null && request.Hashes.Length > 0)
            query = query.Where(projectile => request.Hashes.Contains(projectile.Hash));

        var queryableDto = HitboxMapper.ProjectToDto(query);
        return await PaginatedList<HitboxDto>.CreateAsync(queryableDto, request.Page, request.PerPage);
    }
}
