using BoostStudio.Application.Exvs2.Audio.Commands;
using BoostStudio.Application.Exvs2.Audio.Commands.Nus3Audio;
using BoostStudio.Web.Constants;

namespace BoostStudio.Web.Endpoints.Exvs2;

public class Audio : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs2)
            .MapGet(UnpackBnsfPath, "nus3audio/unpack-path")
            .MapGet(PackBnsfPath, "nus3audio/pack-path")
            .MapGet(PackBnsfPath, "nus3bank/unpack-path")
            .MapGet(PackBnsfPath, "nus3bank/pack-path");
    }

    private async Task PackBnsfPath(ISender sender, [AsParameters] PackNus3AudioPathCommand pathCommand, CancellationToken cancellationToken)
    {
        await sender.Send(pathCommand, cancellationToken);
    }

    private async Task UnpackBnsfPath(ISender sender, [AsParameters] UnpackNus3AudioPathCommand request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
}
