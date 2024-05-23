using BoostStudio.Application.Exvs2.Audio.Commands;

namespace BoostStudio.Web.Endpoints;

public class Audio : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(UnpackBnsfPath, "bnsf/unpack-path")
            .MapGet(PackBnsfPath, "bnsf/pack-path");
    }

    public async Task PackBnsfPath(ISender sender, [AsParameters] PackNus3AudioPathCommand pathCommand, CancellationToken cancellationToken)
    {
        await sender.Send(pathCommand, cancellationToken);
    }

    public async Task UnpackBnsfPath(ISender sender, [AsParameters] UnpackNus3AudioPathCommand request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
}
