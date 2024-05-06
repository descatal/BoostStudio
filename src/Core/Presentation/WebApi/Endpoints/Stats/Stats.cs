using BoostStudio.Application.Exvs.Stats.Commands;

namespace BoostStudio.Web.Endpoints.Stats;

public class Stats : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(CreateStat)
            .MapPost(UpdateStat, "{id}");
    }

    private static async Task CreateStat(ISender sender, CreateStatCommand request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
    
    private static async Task<IResult> UpdateStat(ISender sender, Guid id, UpdateStatCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
}
