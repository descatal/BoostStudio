using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Projectiles;
using BoostStudio.Application.Exvs.Projectiles.Commands.Projectile;
using BoostStudio.Application.Exvs.Projectiles.Commands.UnitProjectile;
using BoostStudio.Application.Exvs.Projectiles.Queries.Projectile;
using BoostStudio.Application.Exvs.Stats.Queries.Stat;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints.Exvs.Projectiles;

public class Projectiles : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs)
            .MapGet(GetStatByPagination)
            .MapGet(GetStatById, "{hash}")
            .MapPost(CreateProjectile)
            .MapPost(UpdateProjectile, "{hash}");
    }

    [ProducesResponseType(typeof(PaginatedList<ProjectileDto>), StatusCodes.Status200OK)]
    private static async Task<PaginatedList<ProjectileDto>> GetStatByPagination(
        ISender sender,
        [AsParameters] GetProjectileWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        return await sender.Send(request, cancellationToken);
    }

    [ProducesResponseType(typeof(ProjectileDto), StatusCodes.Status200OK)]
    private static async Task<ProjectileDto> GetStatById(ISender sender, uint hash, CancellationToken cancellationToken)
    {
        return await sender.Send(new GetProjectileByHashQuery(hash), cancellationToken);
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    private static async Task<IResult> CreateProjectile(ISender sender, CreateProjectileCommand request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
        return Results.Created();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> UpdateProjectile(ISender sender, [FromRoute] uint hash, UpdateProjectileCommand command, CancellationToken cancellationToken)
    {
        if (hash != command.Hash) return Results.BadRequest();
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
}
