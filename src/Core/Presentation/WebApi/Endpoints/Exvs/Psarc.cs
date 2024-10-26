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
            .MapPost(PackPsarcByPath, "pack/path")
            .MapPost(UnpackPsarcByPath, "unpack/path")
            .MapPost(PackPsarcByPatchFiles, "pack/patch-files")
            .MapPost(UnpackPsarcByPatchFiles, "unpack/patch-files");
    }

    private async Task PackPsarcByPath(
        ISender sender, 
        PackPsarcByPathCommand request, 
        CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }

    private async Task UnpackPsarcByPath(
        ISender sender, 
        UnpackPsarcByPathCommand request, 
        CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
    
    private async Task PackPsarcByPatchFiles(
        ISender sender, 
        PackPsarcByPatchFilesCommand request, 
        CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
    
    private async Task UnpackPsarcByPatchFiles(
        ISender sender, 
        UnpackPsarcByPatchFilesCommand request, 
        CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
}
