using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Hitboxes;
using BoostStudio.Application.Exvs.Hitboxes.Commands.Hitbox;
using BoostStudio.Application.Exvs.Hitboxes.Queries.Hitbox;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints.Exvs.Hitboxes;

public class Hitboxes : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs)
            .MapGet(GetHitboxesByPagination)
            .MapGet(GetHitboxById, "{hash}")
            .MapPost(CreateHitbox)
            .MapPost(UpdateHitboxByHash, "{hash}")
            .MapDelete(DeleteHitboxByHash, "{hash}");
    }

    private static async Task<Ok<PaginatedList<HitboxDto>>> GetHitboxesByPagination(
        ISender sender,
        [AsParameters] GetHitboxWithPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        var paginatedList = await sender.Send(request, cancellationToken);
        return TypedResults.Ok(paginatedList);
    }

    private static async Task<Ok<HitboxDto>> GetHitboxById(
        ISender sender,
        uint hash,
        CancellationToken cancellationToken
    )
    {
        var vm = await sender.Send(new GetHitboxByHashQuery(hash), cancellationToken);
        return TypedResults.Ok(vm);
    }

    private static async Task<Created> CreateHitbox(
        ISender sender,
        CreateHitboxCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.Created();
    }

    private static async Task<NoContent> UpdateHitboxByHash(
        ISender sender,
        [FromRoute] uint hash,
        UpdateHitboxCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> DeleteHitboxByHash(
        ISender sender,
        uint hash,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(new DeleteHitboxByHashCommand(hash), cancellationToken);
        return TypedResults.NoContent();
    }
}
