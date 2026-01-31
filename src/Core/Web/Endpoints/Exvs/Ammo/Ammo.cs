using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Ammo;
using BoostStudio.Application.Exvs.Ammo.Commands;
using BoostStudio.Application.Exvs.Ammo.Queries;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ContentType = System.Net.Mime.MediaTypeNames;

namespace BoostStudio.Web.Endpoints.Exvs.Ammo;

public class Ammo : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs)
            .MapGet(GetAmmoWithPagination)
            .MapGet(GetAmmoByHash, "{hash}")
            // .MapGet(GetAmmoOptions, "options")
            .MapPost(CreateAmmo)
            .MapPost(UpdateAmmoByHash, "{hash}")
            .MapDelete(DeleteAmmoByHash, "{hash}")
            .MapPost(ImportAmmo, "import")
            .MapPost(ExportAmmo, "export")
            .MapPost(ExportAmmoByPath, "export/path");
    }

    private static async Task<Ok<PaginatedList<AmmoDto>>> GetAmmoWithPagination(
        ISender sender,
        [AsParameters] GetAmmoWithPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        var paginatedList = await sender.Send(request, cancellationToken);
        return TypedResults.Ok(paginatedList);
    }

    private static async Task<Ok<AmmoDto>> GetAmmoByHash(
        ISender sender,
        uint hash,
        CancellationToken cancellationToken
    )
    {
        var vm = await sender.Send(new GetAmmoByHashQuery(hash), cancellationToken);
        return TypedResults.Ok(vm);
    }

    private static async Task<Ok<List<uint>>> GetAmmoOptions(
        ISender sender,
        [AsParameters] GetAmmoOptionsQuery request,
        CancellationToken cancellationToken
    )
    {
        var vm = await sender.Send(request, cancellationToken);
        return TypedResults.Ok(vm);
    }

    private static async Task<Created> CreateAmmo(
        ISender sender,
        CreateAmmoCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return TypedResults.Created();
    }

    private static async Task<NoContent> UpdateAmmoByHash(
        ISender sender,
        [FromRoute] uint hash,
        UpdateAmmoCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> DeleteAmmoByHash(
        ISender sender,
        [FromRoute] uint hash,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(new DeleteAmmoCommand(hash), cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<Created> ImportAmmo(
        ISender sender,
        [FromForm] IFormFile formFile,
        CancellationToken cancellationToken
    )
    {
        await using var file = formFile.OpenReadStream();
        await sender.Send(new ImportAmmoCommand(file), cancellationToken);
        return TypedResults.Created();
    }

    [ProducesResponseType(
        type: typeof(FileContentHttpResult),
        statusCode: StatusCodes.Status200OK,
        contentType: ContentType.Application.Octet
    )]
    private static async Task<IResult> ExportAmmo(
        ISender sender,
        ExportAmmoCommand command,
        CancellationToken cancellationToken
    )
    {
        var fileInfo = await sender.Send(command, cancellationToken);
        return TypedResults.File(
            fileInfo.Data,
            fileInfo.MediaTypeName ?? ContentType.Application.Octet,
            fileInfo.FileName
        );
    }

    private static async Task<NoContent> ExportAmmoByPath(
        ISender sender,
        ExportAmmoByPathCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return TypedResults.NoContent();
    }
}
