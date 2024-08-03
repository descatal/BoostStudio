using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Hitboxes;
using BoostStudio.Application.Exvs.Hitboxes.Commands.Hitbox;
using BoostStudio.Application.Exvs.Hitboxes.Queries.Hitbox;
using BoostStudio.Web.Constants;
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

    [ProducesResponseType(typeof(PaginatedList<HitboxDto>), StatusCodes.Status200OK)]
    private static async Task<PaginatedList<HitboxDto>> GetHitboxesByPagination(
        ISender sender,
        [AsParameters] GetHitboxWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        return await sender.Send(request, cancellationToken);
    }

    [ProducesResponseType(typeof(HitboxDto), StatusCodes.Status200OK)]
    private static async Task<HitboxDto> GetHitboxById(ISender sender, uint hash, CancellationToken cancellationToken)
    {
        return await sender.Send(new GetHitboxByHashQuery(hash), cancellationToken);
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    private static async Task<IResult> CreateHitbox(ISender sender, CreateHitboxCommand request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
        return Results.Created();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> UpdateHitboxByHash(ISender sender, [FromRoute] uint hash, UpdateHitboxCommand command, CancellationToken cancellationToken)
    {
        if (hash != command.Hash) return Results.BadRequest();
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> DeleteHitboxByHash(ISender sender, uint hash, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteHitboxByHashCommand(hash), cancellationToken);
        return Results.NoContent();
    }
}
