using System.Net.Mime;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Ammo;
using BoostStudio.Application.Exvs.Ammo.Commands;
using BoostStudio.Application.Exvs.Ammo.Queries;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Mvc;
using ContentType=System.Net.Mime.MediaTypeNames;

namespace BoostStudio.Web.Endpoints.Exvs;

public class Ammo : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs)
            .MapGet(GetAmmoWithPagination)
            .MapGet(GetAmmoByHash, "{hash}")
            .MapGet(GetAmmoOptions, "options")
            .MapPost(CreateAmmo)
            .MapPost(UpdateAmmoByHash, "{hash}")
            .MapDelete(DeleteAmmoByHash, "{hash}")
            .MapPost(ImportAmmo, "import")
            .MapPost(ExportAmmo, "export")
            .MapPost(ExportAmmoByPath, "export/path");
    }
    
    private static async Task<PaginatedList<AmmoDto>> GetAmmoWithPagination(ISender sender, [AsParameters] GetAmmoWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await sender.Send(request, cancellationToken);
    }
    
    [Produces(ContentType.Application.Json)]
    // [ProducesResponseType(typeof(AmmoDto), StatusCodes.Status200OK)]
    private static async Task<AmmoDto> GetAmmoByHash(ISender sender, uint hash, CancellationToken cancellationToken)
    {
        return await sender.Send(new GetAmmoByHashQuery(hash), cancellationToken);
    }
    
    private static async Task<List<uint>> GetAmmoOptions(
        ISender sender, 
        [AsParameters] GetAmmoOptionsQuery request, 
        CancellationToken cancellationToken)
    {
        return await sender.Send(request, cancellationToken);
    }

    // [ProducesResponseType(StatusCodes.Status201Created)]
    private static async Task<IResult> CreateAmmo(ISender sender, CreateAmmoCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.Created();
    }
    
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> UpdateAmmoByHash(ISender sender, [FromRoute] uint hash, UpdateAmmoCommand command, CancellationToken cancellationToken)
    {
        if (hash != command.Hash) return Results.BadRequest();
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
    
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> DeleteAmmoByHash(ISender sender, [FromRoute] uint hash, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteAmmoCommand(hash), cancellationToken);
        return Results.NoContent();
    }
    
    private static async Task ImportAmmo(ISender sender, [FromForm] IFormFile formFile, CancellationToken cancellationToken)
    {
        await using var file = formFile.OpenReadStream();
        await sender.Send(new ImportAmmoCommand(file), cancellationToken);
    }
    
    // [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> ExportAmmo(ISender sender, CancellationToken cancellationToken)
    {
        var fileInfo = await sender.Send(new ExportAmmoCommand(), cancellationToken);
        return Results.File(fileInfo.Data, fileInfo.MediaTypeName ?? MediaTypeNames.Application.Octet, fileInfo.FileName);
    }
    
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> ExportAmmoByPath(
        ISender sender, 
        ExportAmmoByPathCommand command,
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
}
