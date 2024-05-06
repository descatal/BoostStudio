using BoostStudio.Application.Exvs.Units.Commands;

namespace BoostStudio.Web.Endpoints.Units;

public class Units : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(CreateUnit)
            .MapPost(UpdateUnit, "{id}")
            .MapPost(UpdateUnitByGameUnitId, "GameUnitId/{id}");
    }
    
    private static async Task CreateUnit(ISender sender, CreateUnitCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
    }
    
    private static async Task<IResult> UpdateUnit(ISender sender, Guid id, UpdateUnitCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
    
    private static async Task<IResult> UpdateUnitByGameUnitId(ISender sender, uint gameId, UpdateUnitByGameUnitIdCommand command, CancellationToken cancellationToken)
    {
        if (gameId != command.GameUnitId) return Results.BadRequest();
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
}

