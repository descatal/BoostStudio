using BoostStudio.Application.Exvs.Psarc.Commands;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BoostStudio.Web.Endpoints.Exvs.Psarc;

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

    private static async Task<NoContent> PackPsarcByPath(
        ISender sender,
        PackPsarcByPathCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> UnpackPsarcByPath(
        ISender sender,
        UnpackPsarcByPathCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> PackPsarcByPatchFiles(
        ISender sender,
        PackPsarcByPatchFilesCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> UnpackPsarcByPatchFiles(
        ISender sender,
        UnpackPsarcByPatchFilesCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.NoContent();
    }
}
