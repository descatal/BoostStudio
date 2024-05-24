using BoostStudio.Application.Scex.Commands;

namespace BoostStudio.Web.Endpoints;

public class Scex : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, "EXVS")
            .MapGet(CompileScex, "compile")
            .MapPost(HotReloadScex, "hot-reload");
    }

    public async Task CompileScex(ISender sender, [AsParameters] CompileScex request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
    
    public async Task HotReloadScex(ISender sender, HotReloadScex request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
}
