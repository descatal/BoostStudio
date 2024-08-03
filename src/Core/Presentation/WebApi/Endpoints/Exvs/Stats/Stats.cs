using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Stats;
using BoostStudio.Application.Exvs.Stats.Commands.Stat;
using BoostStudio.Application.Exvs.Stats.Queries.Stat;
using BoostStudio.Web.Constants;
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

    [ProducesResponseType(typeof(PaginatedList<StatDto>), StatusCodes.Status200OK)]
    private static async Task<PaginatedList<StatDto>> GetStatByPagination(ISender sender, [AsParameters] GetStatWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await sender.Send(request, cancellationToken);
    }
    
    [ProducesResponseType(typeof(StatDto), StatusCodes.Status200OK)]
    private static async Task<StatDto> GetStatById(ISender sender, Guid id, CancellationToken cancellationToken)
    {
        return await sender.Send(new GetStatByIdQuery(id), cancellationToken);
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    private static async Task<IResult> CreateStat(ISender sender, CreateStatCommand request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
        return Results.Created();
    }
    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> UpdateStat(ISender sender, [FromRoute]Guid id, UpdateStatCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> DeleteStat(ISender sender, [FromRoute]Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteStatCommand(id), cancellationToken);
        return Results.NoContent();
    }
}
