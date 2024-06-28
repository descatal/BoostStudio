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
        app.MapGroup(this, DefinitionNames.Exvs)
            .MapGet(GetHitboxGroupsWithPagination)
            .MapGet(GetHitboxGroupByHash, "hash/{hash}")
            .MapGet(GetHitboxGroupByUnitId, "unitId/{unitId}")
            .MapPost(CreateHitboxGroup)
            .MapPost(UpdateHitboxGroup, "{hash}")
            .MapPost(ImportHitboxGroups, "import")
            .MapPost(ExportHitboxGroups, "export")
            .MapPost(ExportHitboxGroupByHash, "export/hash/{hash}")
            .MapPost(ExportHitboxGroupByUnitId, "export/unitId/{unitId}");
    }
    
    [Produces(ContentType.Application.Json)]
    [ProducesResponseType(typeof(PaginatedList<HitboxGroupDto>), StatusCodes.Status200OK)]
    private static Task<PaginatedList<HitboxGroupDto>> GetHitboxGroupsWithPagination(
        ISender sender, 
        [AsParameters] GetHitboxGroupWithPaginationQuery request)
    {
        return sender.Send(request);
    }
    
    [Produces(ContentType.Application.Json)]
    [ProducesResponseType(typeof(HitboxGroupDto), StatusCodes.Status200OK)]
    private static Task<HitboxGroupDto> GetHitboxGroupByHash(ISender sender, [FromRoute] uint hash)
    {
        return sender.Send(new GetHitboxGroupByHashQuery(hash));
    }
    
    [Produces(ContentType.Application.Json)]
    [ProducesResponseType(typeof(HitboxGroupDto), StatusCodes.Status200OK)]
    private static Task<HitboxGroupDto> GetHitboxGroupByUnitId(ISender sender, [FromRoute] uint unitId)
    {
        return sender.Send(new GetHitboxGroupByUnitIdQuery(unitId));
    }
    
    [ProducesResponseType(StatusCodes.Status201Created)]
    private static async Task<IResult> CreateHitboxGroup(ISender sender, CreateHitboxGroupCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.Created();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> UpdateHitboxGroup(ISender sender, uint hash, UpdateHitboxGroupCommand command, CancellationToken cancellationToken)
    {
        if (hash != command.Hash) return Results.BadRequest();
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
    
    [ProducesResponseType(StatusCodes.Status201Created)]
    private static async Task<IResult> ImportHitboxGroups(ISender sender, [FromForm] IFormFile file, uint? unitId, CancellationToken cancellationToken)
    {
        // var importHitboxGroupDetailsArray = data
        //     .Select(request => new ImportHitboxGroupDetails(request.File.OpenReadStream(), request.UnitId))
        //     .ToArray();
        var import = new ImportHitboxGroupDetails(file.OpenReadStream(), unitId);
        
        await sender.Send(new ImportHitboxGroupCommand([import]), cancellationToken);

        foreach (var fileStream in new [] { import })
            await fileStream.File.DisposeAsync();

        return Results.Created();
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> ExportHitboxGroups(ISender sender, ExportHitboxGroupCommand groupCommand, CancellationToken cancellationToken)
    {
        var fileInfo = await sender.Send(groupCommand, cancellationToken);
        return Results.File(fileInfo.Data, fileInfo.MediaTypeName ?? ContentType.Application.Octet, fileInfo.FileName);
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> ExportHitboxGroupByHash(ISender sender, [FromRoute] uint hash, CancellationToken cancellationToken)
    {
        var fileInfo = await sender.Send(new ExportHitboxGroupByHashCommand(hash), cancellationToken);
        return Results.File(fileInfo.Data, fileInfo.MediaTypeName ?? ContentType.Application.Octet, fileInfo.FileName);
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> ExportHitboxGroupByUnitId(ISender sender, [FromRoute] uint unitId, CancellationToken cancellationToken)
    {
        var fileInfo = await sender.Send(new ExportHitboxGroupByUnitIdCommand(unitId), cancellationToken);
        return Results.File(fileInfo.Data, fileInfo.MediaTypeName ?? ContentType.Application.Octet, fileInfo.FileName);
    }
}
