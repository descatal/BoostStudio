using System.Net.Mime;
using BoostStudio.Application.Contracts.Ammo;
using BoostStudio.Application.Exvs.Ammo.Commands;
using BoostStudio.Application.Exvs.Ammo.Queries;
using BoostStudio.Application.Formats.AmmoFormat.Commands;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints;

public class Ammo : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetAmmo)
            .MapPost(CreateAmmo)
            .MapPost(UpdateAmmo, "{hash}")
            .MapPost(ImportAmmo, "import")
            .MapPost(ExportAmmo, "export");
            // .MapPost(BulkCreateAmmo, "bulk")
            // .MapGet(SerializeAmmo, "serialize-path")
            // .MapGet(DeserializeAmmo, "deserialize-path")
    }
    
    private static async Task<List<AmmoDto>> GetAmmo(ISender sender, [AsParameters] GetAmmoQuery request, CancellationToken cancellationToken)
    {
        return await sender.Send(request, cancellationToken);
    }

    private static async Task<IResult> CreateAmmo(ISender sender, CreateAmmoCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.Created();
    }
    
    private static async Task<IResult> UpdateAmmo(ISender sender, uint hash, AmmoDetails details, CancellationToken cancellationToken)
    {
        await sender.Send(new UpdateAmmoCommand(hash, details), cancellationToken);
        return Results.NoContent();
    }
    
    private static async Task ImportAmmo(ISender sender, [FromForm] IFormFile formFile, CancellationToken cancellationToken)
    {
        await using var file = formFile.OpenReadStream();
        await sender.Send(new ImportAmmoCommand(file), cancellationToken);
    }
    
    private static async Task<IResult> ExportAmmo(ISender sender, CancellationToken cancellationToken)
    {
        var fileInfo = await sender.Send(new ExportAmmoCommand(), cancellationToken);
        return Results.File(fileInfo.Data, fileInfo.MediaTypeName ?? MediaTypeNames.Application.Octet, fileInfo.FileName);
    }
    
    // private static async Task SerializeAmmo(ISender sender, [AsParameters] SerializeAmmoCommand request, CancellationToken cancellationToken)
    // {
    //     await sender.Send(request, cancellationToken);
    // }
    //
    // private static async Task<AmmoView> DeserializeAmmo(ISender sender, [AsParameters] DeserializeAmmoCommand command, CancellationToken cancellationToken)
    // {
    //     return await sender.Send(command, cancellationToken);
    // }
}
