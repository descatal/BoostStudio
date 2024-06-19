using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Projectiles;

namespace BoostStudio.Application.Exvs.Projectiles.Queries.Projectile;

public record GetProjectileWithPaginationQuery(
    int Page = 1,
    int PerPage = 10,
    uint[]? Hashes = null,
    uint[]? UnitIds = null
) : IRequest<PaginatedList<ProjectileDto>>;

public class GetProjectileWithPaginationQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetProjectileWithPaginationQuery, PaginatedList<ProjectileDto>>
{
    public async Task<PaginatedList<ProjectileDto>> Handle(GetProjectileWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = applicationDbContext.Projectiles.AsQueryable();
        
        if (request.UnitIds is not null && request.UnitIds.Length > 0)
            query = query.Where(projectile => projectile.UnitProjectile != null && request.UnitIds.Contains(projectile.UnitProjectile.GameUnitId));

        if (request.Hashes is not null && request.Hashes.Length > 0)
            query = query.Where(projectile => request.Hashes.Contains(projectile.Hash));

        var queryableDto = ProjectileMapper.ProjectToDto(query);
        return await PaginatedList<ProjectileDto>.CreateAsync(queryableDto, request.Page, request.PerPage);
    }
}
