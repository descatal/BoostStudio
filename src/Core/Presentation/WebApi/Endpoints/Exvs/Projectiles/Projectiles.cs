using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Projectiles;
using BoostStudio.Application.Exvs.Projectiles.Commands.Projectile;
using BoostStudio.Application.Exvs.Projectiles.Commands.UnitProjectile;
using BoostStudio.Application.Exvs.Projectiles.Queries.Projectile;
using BoostStudio.Application.Exvs.Stats.Queries.Stat;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints.Exvs.Projectiles;

public class Projectiles : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs)
            .MapGet(GetProjectileByPagination)
            .MapGet(GetProjectileById, "{hash}")
            .MapPost(CreateProjectile)
            .MapPost(UpdateProjectileByHash, "{hash}")
            .MapDelete(DeleteProjectileById, "{hash}");
    }

    private static async Task<Ok<PaginatedList<ProjectileDto>>> GetProjectileByPagination(
        ISender sender,
        [AsParameters] GetProjectileWithPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        return TypedResults.Ok(await sender.Send(request, cancellationToken));
    }

    private static async Task<ProjectileDto> GetProjectileById(
        ISender sender,
        uint hash,
        CancellationToken cancellationToken
    )
    {
        return await sender.Send(new GetProjectileByHashQuery(hash), cancellationToken);
    }

    private static async Task<Created> CreateProjectile(
        ISender sender,
        CreateProjectileCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.Created();
    }

    private static async Task<IResult> UpdateProjectileByHash(
        ISender sender,
        [FromRoute] uint hash,
        UpdateProjectileByIdCommand byIdCommand,
        CancellationToken cancellationToken
    )
    {
        if (hash != byIdCommand.Hash)
            return Results.BadRequest();

        await sender.Send(byIdCommand, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteProjectileById(
        ISender sender,
        uint hash,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(new DeleteProjectileByHashCommand(hash), cancellationToken);
        return Results.NoContent();
    }
}
