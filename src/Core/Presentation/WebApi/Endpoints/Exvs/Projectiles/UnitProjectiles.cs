using System.Net.Mime;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Projectiles.UnitProjectiles;
using BoostStudio.Application.Exvs.Projectiles.Commands.UnitProjectile;
using BoostStudio.Application.Exvs.Projectiles.Queries.UnitProjectile;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Mvc;
using ContentType=System.Net.Mime.MediaTypeNames;

namespace BoostStudio.Web.Endpoints.Exvs.Projectiles;

public class UnitProjectiles : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapSubgroup(this, customTagName: nameof(Projectiles), areaName: DefinitionNames.Exvs)
            .MapGet(GetUnitProjectilesWithPagination)
            .MapGet(GetUnitProjectileByUnitId, "{unitId}")
            .MapPost(ImportUnitProjectiles, "import")
            .MapPost(ImportUnitProjectilesByPath, "import/path")
            .MapPost(ExportUnitProjectiles, "export")
            .MapPost(ExportUnitProjectilesByPath, "export/path");
    }
    
    [Produces(ContentType.Application.Json)]
    // [ProducesResponseType(typeof(PaginatedList<UnitProjectileDto>), StatusCodes.Status200OK)]
    private static async Task<PaginatedList<UnitProjectileDto>> GetUnitProjectilesWithPagination(ISender sender, [AsParameters] GetUnitProjectileWithPaginationQuery request)
    {
        return await sender.Send(request);
    }
    
    [Produces(ContentType.Application.Json)]
    // [ProducesResponseType(typeof(UnitProjectileDto), StatusCodes.Status200OK)]
    private static async Task<UnitProjectileDto> GetUnitProjectileByUnitId(ISender sender, [FromRoute] uint unitId)
    {
        return await sender.Send(new GetUnitProjectileByUnitIdQuery(unitId));
    }
    
    // [ProducesResponseType(StatusCodes.Status201Created)]
    private static async Task<IResult> ImportUnitProjectiles(ISender sender, [FromForm] IFormFileCollection files, CancellationToken cancellationToken)
    {
        var fileStreams = files.Select(formFile => formFile.OpenReadStream()).ToArray();
        await sender.Send(new ImportUnitProjectileCommand(fileStreams), cancellationToken);

        foreach (var fileStream in fileStreams)
            await fileStream.DisposeAsync();

        return Results.Created();
    }
    
    // [ProducesResponseType(StatusCodes.Status201Created)]
    private static async Task<IResult> ImportUnitProjectilesByPath(
        ISender sender, 
        string directoryPath,
        CancellationToken cancellationToken)
    {
        List<Stream> import = [];
        var files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);
        import.AddRange(files.Select(File.OpenRead));

        await sender.Send(new ImportUnitProjectileCommand(import.ToArray()), cancellationToken);

        foreach (var fileStream in import)
            await fileStream.DisposeAsync();

        return Results.Created();
    }
    
    // [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> ExportUnitProjectiles(
        ISender sender, 
        ExportUnitProjectileCommand command, 
        CancellationToken cancellationToken)
    {
        var fileInfo = await sender.Send(command, cancellationToken);
        return Results.File(fileInfo.Data, fileInfo.MediaTypeName ?? MediaTypeNames.Application.Octet, fileInfo.FileName);
    }
    
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> ExportUnitProjectilesByPath(
        ISender sender, 
        ExportUnitProjectileByPathCommand command, 
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
}
