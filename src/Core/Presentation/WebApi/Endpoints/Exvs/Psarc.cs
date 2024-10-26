using BoostStudio.Application.Exvs.Psarc.Commands;
using BoostStudio.Application.Formats.PsarcFormat;
using BoostStudio.Web.Constants;

namespace BoostStudio.Web.Endpoints.Exvs;

/// <summary>
/// Serialize / Deserialize Psarc files
/// </summary>
public class Psarc : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs)
            .MapGet(PackPsarcByPath, "pack/path")
            .MapGet(UnpackPsarcByPath, "unpack/path")
            .MapGet(PackPsarcByPatchFiles, "pack/patch-files")
            .MapGet(UnpackPsarcByPatchFiles, "unpack/patch-files");
    }

    private async Task PackPsarcByPath(
        ISender sender, 
        [AsParameters] PackPsarcByPathCommand request, 
        CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }

    private async Task UnpackPsarcByPath(
        ISender sender, 
        [AsParameters] UnpackPsarcByPathCommand request, 
        CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
    
    private async Task PackPsarcByPatchFiles(
        ISender sender, 
        [AsParameters] PackPsarcByPatchFilesCommand request, 
        CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
    
    private async Task UnpackPsarcByPatchFiles(
        ISender sender, 
        [AsParameters] UnpackPsarcByPatchFilesCommand request, 
        CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
}
