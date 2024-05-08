using BoostStudio.Application.Contracts.Units;
using BoostStudio.Application.Exvs.Units.Commands;
using BoostStudio.Application.Exvs.Units.Queries;

namespace BoostStudio.Web.Endpoints.Units;

public class Units : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetUnit)
            .MapPost(CreateUnit)
            .MapPost(UpdateUnit, "{id}");
    }
    
    private static async Task<List<UnitDto>> GetUnit(ISender sender, [AsParameters] GetUnitQuery query, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }
    
    private static async Task<IResult> CreateUnit(ISender sender, CreateUnitCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.Created();
    }
    
    private static async Task<IResult> UpdateUnit(ISender sender, uint gameId, UpdateUnitCommand command, CancellationToken cancellationToken)
    {
        if (gameId != command.GameUnitId) return Results.BadRequest();
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
}

