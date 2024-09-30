using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Hitboxes.HitboxGroups;
using BoostStudio.Application.Exvs.Hitboxes.Commands.HitboxGroup;
using BoostStudio.Application.Exvs.Hitboxes.Queries.HitboxGroup;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Mvc;
using ContentType=System.Net.Mime.MediaTypeNames;

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
    
    [Produces(ContentType.Application.Json)]
    // [ProducesResponseType(typeof(PaginatedList<HitboxGroupDto>), StatusCodes.Status200OK)]
    private static async Task<PaginatedList<HitboxGroupDto>> GetHitboxGroupsWithPagination(
        ISender sender, 
        [AsParameters] GetHitboxGroupWithPaginationQuery request)
    {
        return await sender.Send(request);
    }
    
    [Produces(ContentType.Application.Json)]
    // [ProducesResponseType(typeof(HitboxGroupDto), StatusCodes.Status200OK)]
    private static async Task<HitboxGroupDto> GetHitboxGroupByHash(ISender sender, [FromRoute] uint hash)
    {
        return await sender.Send(new GetHitboxGroupByHashQuery(hash));
    }
    
    [Produces(ContentType.Application.Json)]
    // [ProducesResponseType(typeof(HitboxGroupDto), StatusCodes.Status200OK)]
    private static async Task<HitboxGroupDto> GetHitboxGroupByUnitId(ISender sender, [FromRoute] uint unitId)
    {
        return await sender.Send(new GetHitboxGroupByUnitIdQuery(unitId));
    }
    
    // [ProducesResponseType(StatusCodes.Status201Created)]
    private static async Task<IResult> CreateHitboxGroup(ISender sender, CreateHitboxGroupCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.Created();
    }

    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> UpdateHitboxGroup(ISender sender, uint hash, UpdateHitboxGroupCommand command, CancellationToken cancellationToken)
    {
        if (hash != command.Hash) return Results.BadRequest();
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
    
    // [ProducesResponseType(StatusCodes.Status201Created)]
    private static async Task<IResult> ImportHitboxGroups(ISender sender, [FromForm] IFormFile file, uint? unitId, CancellationToken cancellationToken)
    {
        // var importHitboxGroupDetailsArray = data
        //     .Select(request => new ImportHitboxGroupDetails(request.File.OpenReadStream(), request.UnitId))
        //     .ToArray();

        uint[] unitIds = unitId is not null ? [unitId.Value] : [];
        var import = new ImportHitboxGroupDetails(file.OpenReadStream(), unitIds);
        
        await sender.Send(new ImportHitboxGroupCommand([import]), cancellationToken);

        foreach (var fileStream in new [] { import })
            await fileStream.File.DisposeAsync();

        return Results.Created();
    }
    
    // [ProducesResponseType(StatusCodes.Status201Created)]
    private static async Task<IResult> ImportHitboxGroupsByPath(
        ISender sender, 
        string directoryPath,
        CancellationToken cancellationToken)
    {
        List<ImportHitboxGroupDetails> import = [];
        var files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            var unitIds = fileName.Split('-')
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => {
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

        return Results.Created();
    }
    
    // [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> ExportHitboxGroups(
        ISender sender, 
        ExportHitboxGroupCommand groupCommand, 
        CancellationToken cancellationToken)
    {
        var fileInfo = await sender.Send(groupCommand, cancellationToken);
        return Results.File(fileInfo.Data, fileInfo.MediaTypeName ?? ContentType.Application.Octet, fileInfo.FileName);
    }
    
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> ExportHitboxGroupsByPath(
        ISender sender, 
        ExportHitboxGroupByPathCommand groupCommand, 
        CancellationToken cancellationToken)
    {
        await sender.Send(groupCommand, cancellationToken);
        return Results.NoContent();
    }
}
