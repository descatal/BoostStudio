using System.Net.Mime;
using BoostStudio.Application.Contracts.Units;
using BoostStudio.Application.Exvs.Series.Commands;
using BoostStudio.Application.Exvs.Units.Commands;
using BoostStudio.Application.Exvs.Units.Queries;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints.Exvs.Units;

public class Units : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs)
            .MapGet(GetUnit)
            .MapGet(GetUnitByUnitId, "{unitId}")
            .MapPost(CreateUnit)
            .MapPost(UpdateUnit, "{unitId}")
            .MapPost(BulkCreateUnit, "bulk")
            .MapPost(ImportPlayableCharacters, "playable-characters/import")
            .MapPost(ExportPlayableCharacters, "playable-characters/export");
    }
    
    private static async Task<List<UnitDto>> GetUnit(
        ISender sender, 
        [AsParameters] GetUnitQuery query, 
        CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }
    
    private static async Task<UnitDto> GetUnitByUnitId(
        ISender sender, 
        uint unitId,
        CancellationToken cancellationToken)
    {
        return await sender.Send(new GetUnitQueryByUnitId(unitId), cancellationToken);
    }
    
    private static async Task<IResult> CreateUnit(
        ISender sender, 
        CreateUnitCommand command, 
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.Created();
    }
    
    private static async Task<IResult> BulkCreateUnit(
        ISender sender, 
        BulkCreateUnitCommand command, 
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.Created();
    }
    
    private static async Task<IResult> UpdateUnit(
        ISender sender, 
        uint unitId, 
        UpdateUnitCommand command, 
        CancellationToken cancellationToken)
    {
        if (unitId != command.UnitId) return Results.BadRequest();
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> ImportPlayableCharacters(
        ISender sender,
        [FromForm] IFormFile file,
        CancellationToken cancellationToken)
    {
        var fileStream = file.OpenReadStream();
        await sender.Send(new ImportPlayableCharactersCommand(fileStream), cancellationToken);
        await fileStream.DisposeAsync();

        return Results.Created();
    }

    private static async Task<IResult> ExportPlayableCharacters(
        ISender sender,
        ExportPlayableCharactersCommand command,
        CancellationToken cancellationToken)
    {
        var fileInfo = await sender.Send(command, cancellationToken);
        return Results.File(fileInfo.Data, fileInfo.MediaTypeName ?? MediaTypeNames.Application.Octet, fileInfo.FileName);
    }
}

