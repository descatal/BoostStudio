using System.Net.Mime;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Units;
using BoostStudio.Application.Exvs.Series.Commands;
using BoostStudio.Application.Exvs.Units.Commands;
using BoostStudio.Application.Exvs.Units.Commands.PlayableCharacters;
using BoostStudio.Application.Exvs.Units.Queries;
using BoostStudio.Application.Exvs.Units.Queries.PlayableCharacters;
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
            .MapGet(GetPlayableCharactersByUnitId, "{unitId}/playable-characters")
            .MapPost(CreateUnit)
            .MapPost(UpdateUnit, "{unitId}")
            .MapPost(BulkCreateUnit, "bulk")
            .MapPost(UpsertPlayableCharacterByUnitId, "{unitId}/playable-characters")
            .MapPost(ImportPlayableCharacters, "playable-characters/import")
            .MapPost(ExportPlayableCharacters, "playable-characters/export");
    }

    private static async Task<PaginatedList<UnitSummaryVm>> GetUnit(
        ISender sender,
        [AsParameters] GetUnitSummaryQuery summaryQuery,
        CancellationToken cancellationToken
    )
    {
        return await sender.Send(summaryQuery, cancellationToken);
    }

    private static async Task<UnitSummaryVm> GetUnitByUnitId(
        ISender sender,
        uint unitId,
        CancellationToken cancellationToken
    )
    {
        return await sender.Send(new GetUnitQueryByUnitId(unitId), cancellationToken);
    }

    private static async Task<PlayableCharacterDto> GetPlayableCharactersByUnitId(
        ISender sender,
        uint unitId,
        CancellationToken cancellationToken
    )
    {
        return await sender.Send(new GetPlayableCharactersQuery(unitId), cancellationToken);
    }

    private static async Task<IResult> CreateUnit(
        ISender sender,
        CreateUnitCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return Results.Created();
    }

    private static async Task<IResult> BulkCreateUnit(
        ISender sender,
        BulkCreateUnitCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return Results.Created();
    }

    private static async Task<IResult> UpdateUnit(
        ISender sender,
        uint unitId,
        UpdateUnitCommand command,
        CancellationToken cancellationToken
    )
    {
        if (unitId != command.UnitId)
            return Results.BadRequest();
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> UpsertPlayableCharacterByUnitId(
        ISender sender,
        uint unitId,
        UpsertPlayableCharactersCommand command,
        CancellationToken cancellationToken
    )
    {
        if (unitId != command.UnitId)
            return Results.BadRequest();
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> ImportPlayableCharacters(
        ISender sender,
        [FromForm] IFormFile file,
        CancellationToken cancellationToken
    )
    {
        var fileStream = file.OpenReadStream();
        await sender.Send(new ImportPlayableCharactersCommand(fileStream), cancellationToken);
        await fileStream.DisposeAsync();

        return Results.Created();
    }

    private static async Task<IResult> ExportPlayableCharacters(
        ISender sender,
        ExportPlayableCharactersCommand command,
        CancellationToken cancellationToken
    )
    {
        var fileInfo = await sender.Send(command, cancellationToken);
        return Results.File(
            fileInfo.Data,
            fileInfo.MediaTypeName ?? MediaTypeNames.Application.Octet,
            fileInfo.FileName
        );
    }
}
