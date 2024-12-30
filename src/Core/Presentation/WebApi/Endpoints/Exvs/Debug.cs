using BoostStudio.Application.Debug;
using BoostStudio.Web.Constants;

namespace BoostStudio.Web.Endpoints.Exvs;

public class Debug: EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs)
            .MapPost(DebugCommand);
    }

    private static async Task<IResult> DebugCommand(
        ISender sender,
        DebugCommand command,
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
}
