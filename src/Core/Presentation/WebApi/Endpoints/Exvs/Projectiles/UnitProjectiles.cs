using System.Net.Mime;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Projectiles.UnitProjectiles;
using BoostStudio.Application.Contracts.Stats.UnitStats;
using BoostStudio.Application.Contracts.Units;
using BoostStudio.Application.Exvs.Projectiles.Commands.UnitProjectile;
using BoostStudio.Application.Exvs.Projectiles.Queries.UnitProjectile;
using BoostStudio.Application.Exvs.Stats.Queries.UnitStat;
using BoostStudio.Application.Exvs.UnitAmmoSlots.Commands;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Mvc;
using ContentType=System.Net.Mime.MediaTypeNames;

namespace BoostStudio.Web.Endpoints.Exvs.Projectiles;

public class UnitProjectiles : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs)
            .MapGet(GetUnitProjectileWithPagination)
            .MapGet(GetUnitProjectileById, "{unitId}")
            .MapPost(ImportUnitProjectile, "import")
            .MapPost(ExportUnitProjectile, "export")
            .MapPost(ExportUnitProjectileById, "export/{unitId}");
    }
    
    [Produces(ContentType.Application.Json)]
    [ProducesResponseType(typeof(PaginatedList<UnitProjectileDto>), StatusCodes.Status200OK)]
    private static Task<PaginatedList<UnitProjectileDto>> GetUnitProjectileWithPagination(ISender sender, [AsParameters] GetUnitProjectileWithPaginationQuery request)
    {
        return sender.Send(request);
    }
    
    [Produces(ContentType.Application.Json)]
    [ProducesResponseType(typeof(UnitProjectileDto), StatusCodes.Status200OK)]
    private static Task<UnitProjectileDto> GetUnitProjectileById(ISender sender, [FromRoute] uint unitId)
    {
        return sender.Send(new GetUnitProjectileByUnitIdQuery(unitId));
    }
    
    [ProducesResponseType(StatusCodes.Status201Created)]
    private static async Task<IResult> ImportUnitProjectile(ISender sender, [FromForm] IFormFileCollection files, CancellationToken cancellationToken)
    {
        var fileStreams = files.Select(formFile => formFile.OpenReadStream()).ToArray();
        await sender.Send(new ImportUnitProjectileCommand(fileStreams), cancellationToken);

        foreach (var fileStream in fileStreams)
            await fileStream.DisposeAsync();

        return Results.Created();
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> ExportUnitProjectile(ISender sender, ExportUnitProjectileCommand command, CancellationToken cancellationToken)
    {
        var fileInfo = await sender.Send(command, cancellationToken);
        return Results.File(fileInfo.Data, fileInfo.MediaTypeName ?? MediaTypeNames.Application.Octet, fileInfo.FileName);
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> ExportUnitProjectileById(ISender sender, [FromRoute] uint unitId, CancellationToken cancellationToken)
    {
        var fileInfo = await sender.Send(new ExportUnitProjectileByIdCommand(unitId), cancellationToken);
        return Results.File(fileInfo.Data, fileInfo.MediaTypeName ?? MediaTypeNames.Application.Octet, fileInfo.FileName);
    }
}
