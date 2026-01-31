using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Hitboxes.HitboxGroups;
using BoostStudio.Application.Exvs.Hitboxes.Commands.HitboxGroup;
using BoostStudio.Application.Exvs.Hitboxes.Queries.HitboxGroup;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ContentType = System.Net.Mime.MediaTypeNames;

namespace BoostStudio.Web.Endpoints.Exvs.Hitboxes;

public class HitboxGroups : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapSubgroup(this, customTagName: nameof(Hitboxes), areaName: DefinitionNames.Exvs)
            .MapGet(GetHitboxGroupsWithPagination)
            .MapGet(GetHitboxGroupByHash, "hash/{hash}")
            .MapGet(GetHitboxGroupByUnitId, "unitId/{unitId}")
            .MapPost(CreateHitboxGroup)
            .MapPost(UpdateHitboxGroup, "{hash}")
            .MapPost(ImportHitboxGroups, "import")
            .MapPost(ImportHitboxGroupsByPath, "import/path")
            .MapPost(ExportHitboxGroups, "export")
            .MapPost(ExportHitboxGroupsByPath, "export/path");
    }

    private static async Task<Ok<PaginatedList<HitboxGroupDto>>> GetHitboxGroupsWithPagination(
        ISender sender,
        [AsParameters] GetHitboxGroupWithPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        var paginatedList = await sender.Send(request, cancellationToken);
        return TypedResults.Ok(paginatedList);
    }

    private static async Task<Ok<HitboxGroupDto>> GetHitboxGroupByHash(
        ISender sender,
        [FromRoute] uint hash,
        CancellationToken cancellationToken
    )
    {
        var vm = await sender.Send(new GetHitboxGroupByHashQuery(hash), cancellationToken);
        return TypedResults.Ok(vm);
    }

    private static async Task<Ok<HitboxGroupDto>> GetHitboxGroupByUnitId(
        ISender sender,
        [FromRoute] uint unitId,
        CancellationToken cancellationToken
    )
    {
        var vm = await sender.Send(new GetHitboxGroupByUnitIdQuery(unitId), cancellationToken);
        return TypedResults.Ok(vm);
    }

    private static async Task<Created> CreateHitboxGroup(
        ISender sender,
        CreateHitboxGroupCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return TypedResults.Created();
    }

    private static async Task<NoContent> UpdateHitboxGroup(
        ISender sender,
        uint hash,
        UpdateHitboxGroupCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<Created> ImportHitboxGroups(
        ISender sender,
        [FromForm] IFormFile file,
        uint? unitId,
        CancellationToken cancellationToken
    )
    {
        uint[] unitIds = unitId is not null ? [unitId.Value] : [];
        var import = new ImportHitboxGroupDetails(file.OpenReadStream(), unitIds);

        await sender.Send(new ImportHitboxGroupCommand([import]), cancellationToken);

        foreach (var fileStream in new[] { import })
            await fileStream.File.DisposeAsync();

        return TypedResults.Created();
    }

    private static async Task<Created> ImportHitboxGroupsByPath(
        ISender sender,
        string directoryPath,
        CancellationToken cancellationToken
    )
    {
        List<ImportHitboxGroupDetails> import = [];
        var files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            var unitIds = fileName
                .Split('-')
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name =>
                {
                    bool success = uint.TryParse(name.Trim(), out var value);
                    return new { value, success };
                })
                .Where(pair => pair.success)
                .Select(pair => pair.value)
                .ToArray();

            var fileStream = File.OpenRead(file);
            import.Add(new ImportHitboxGroupDetails(fileStream, unitIds));
        }

        await sender.Send(new ImportHitboxGroupCommand(import.ToArray()), cancellationToken);

        foreach (var fileStream in import)
            await fileStream.File.DisposeAsync();

        return TypedResults.Created();
    }

    // Needed for OpenApi to recognize the return type as FileContentHttpResult, which will be converted to Blob
    [ProducesResponseType(
        type: typeof(FileContentHttpResult),
        statusCode: StatusCodes.Status200OK,
        contentType: ContentType.Application.Octet
    )]
    private static async Task<FileContentHttpResult> ExportHitboxGroups(
        ISender sender,
        ExportHitboxGroupCommand groupCommand,
        CancellationToken cancellationToken
    )
    {
        var fileInfo = await sender.Send(groupCommand, cancellationToken);
        return TypedResults.File(
            fileInfo.Data,
            fileInfo.MediaTypeName ?? ContentType.Application.Octet,
            fileInfo.FileName
        );
    }

    private static async Task<NoContent> ExportHitboxGroupsByPath(
        ISender sender,
        ExportHitboxGroupByPathCommand groupCommand,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(groupCommand, cancellationToken);
        return TypedResults.NoContent();
    }
}
