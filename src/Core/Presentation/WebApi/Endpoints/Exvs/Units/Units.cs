using BoostStudio.Application.Contracts.Units;
using BoostStudio.Application.Exvs.Units.Commands;
using BoostStudio.Application.Exvs.Units.Queries;
using BoostStudio.Web.Constants;

namespace BoostStudio.Web.Endpoints.Exvs.Units;

public class Units : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs)
            .MapGet(GetUnit)
            .MapPost(CreateUnit)
            .MapPost(UpdateUnit, "{unitId}");
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
    
    private static async Task<IResult> UpdateUnit(ISender sender, uint unitId, UpdateUnitCommand command, CancellationToken cancellationToken)
    {
        if (unitId != command.UnitId) return Results.BadRequest();
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
}

