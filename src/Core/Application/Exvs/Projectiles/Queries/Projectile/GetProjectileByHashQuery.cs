using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Hitboxes;
using BoostStudio.Application.Contracts.Projectiles;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Projectiles.Queries.Projectile;

public record GetProjectileByHashQuery(uint Hash) : IRequest<ProjectileDto>;

public class GetProjectileByHashQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetProjectileByHashQuery, ProjectileDto>
{
    public async Task<ProjectileDto> Handle(GetProjectileByHashQuery request, CancellationToken cancellationToken)
    {
        var projectiles = applicationDbContext.Projectiles
            .Where(projectile => projectile.Hash == request.Hash);
        
        var queryableDto = ProjectileMapper.ProjectToDto(projectiles);
        var dto = await queryableDto.FirstOrDefaultAsync(cancellationToken);
        Guard.Against.NotFound(request.Hash, dto);
        
        return dto;
    }
}
