using BoostStudio.Application.Formats.PsarcFormat;

namespace BoostStudio.Web.Endpoints;

/// <summary>
/// Serialize / Deserialize Psarc files
/// </summary>
public class Psarc : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(PackPsarc, "pack")
            .MapGet(UnpackPsarc, "unpack");
    }

    public async Task PackPsarc(ISender sender, [AsParameters] PackPsarcCommand request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
    
    public async Task UnpackPsarc(ISender sender, [AsParameters] UnpackPsarcCommand request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
}
