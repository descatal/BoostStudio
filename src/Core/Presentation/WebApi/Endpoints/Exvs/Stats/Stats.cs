using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Stats;
using BoostStudio.Application.Exvs.Stats.Commands.Stat;
using BoostStudio.Application.Exvs.Stats.Queries.Stat;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints.Exvs.Stats;

public class Stats : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs)
            .MapGet(GetStatByPagination)
            .MapGet(GetStatById, "{id}")
            .MapPost(CreateStat)
            .MapPost(UpdateStat, "{id}")
            .MapDelete(DeleteStat, "{id}");
    }

    private static async Task<Ok<PaginatedList<StatDto>>> GetStatByPagination(
        ISender sender,
        [AsParameters] GetStatWithPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        var paginatedList = await sender.Send(request, cancellationToken);
        return TypedResults.Ok(paginatedList);
    }

    private static async Task<Ok<StatDto>> GetStatById(
        ISender sender,
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var vm = await sender.Send(new GetStatByIdQuery(id), cancellationToken);
        return TypedResults.Ok(vm);
    }

    private static async Task<Created> CreateStat(
        ISender sender,
        CreateStatCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.Created();
    }

    private static async Task<NoContent> UpdateStat(
        ISender sender,
        [FromRoute] Guid id,
        UpdateStatCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> DeleteStat(
        ISender sender,
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(new DeleteStatCommand(id), cancellationToken);
        return TypedResults.NoContent();
    }
}
