using System.Net.Mime;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Units;
using BoostStudio.Application.Exvs.Units.Commands;
using BoostStudio.Application.Exvs.Units.Commands.PlayableCharacters;
using BoostStudio.Application.Exvs.Units.Queries;
using BoostStudio.Application.Exvs.Units.Queries.PlayableCharacters;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
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
            .MapPost(UpsertPlayableCharacterByUnitId, "{unitId}/playable-character")
            .MapDelete(DeletePlayableCharacterByUnitId, "{unitId}/playable-character")
            .MapPost(ImportPlayableCharacters, "playable-characters/import")
            .MapPost(ExportPlayableCharacters, "playable-characters/export");
    }

    private static async Task<Ok<PaginatedList<UnitSummaryVm>>> GetUnit(
        ISender sender,
        [AsParameters] GetUnitSummaryQuery summaryQuery,
        CancellationToken cancellationToken
    )
    {
        var paginatedList = await sender.Send(summaryQuery, cancellationToken);
        return TypedResults.Ok(paginatedList);
    }

    private static async Task<Ok<UnitSummaryVm>> GetUnitByUnitId(
        ISender sender,
        uint unitId,
        CancellationToken cancellationToken
    )
    {
        var vm = await sender.Send(new GetUnitQueryByUnitId(unitId), cancellationToken);
        return TypedResults.Ok(vm);
    }

    private static async Task<Ok<PlayableCharacterDto>> GetPlayableCharactersByUnitId(
        ISender sender,
        uint unitId,
        CancellationToken cancellationToken
    )
    {
        var vm = await sender.Send(new GetPlayableCharactersQuery(unitId), cancellationToken);
        return TypedResults.Ok(vm);
    }

    private static async Task<Created> CreateUnit(
        ISender sender,
        CreateUnitCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return TypedResults.Created();
    }

    private static async Task<Created> BulkCreateUnit(
        ISender sender,
        BulkCreateUnitCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return TypedResults.Created();
    }

    private static async Task<NoContent> UpdateUnit(
        ISender sender,
        uint unitId,
        UpdateUnitCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> UpsertPlayableCharacterByUnitId(
        ISender sender,
        uint unitId,
        UpsertPlayableCharacterCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> DeletePlayableCharacterByUnitId(
        ISender sender,
        uint unitId,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(new DeletePlayableCharacterCommand(unitId), cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<Created> ImportPlayableCharacters(
        ISender sender,
        [FromForm] IFormFile file,
        CancellationToken cancellationToken
    )
    {
        var fileStream = file.OpenReadStream();
        await sender.Send(new ImportPlayableCharactersCommand(fileStream), cancellationToken);
        await fileStream.DisposeAsync();

        return TypedResults.Created();
    }

    [ProducesResponseType(
        type: typeof(FileContentHttpResult),
        statusCode: StatusCodes.Status200OK,
        contentType: MediaTypeNames.Application.Octet
    )]
    private static async Task<FileContentHttpResult> ExportPlayableCharacters(
        ISender sender,
        ExportPlayableCharactersCommand command,
        CancellationToken cancellationToken
    )
    {
        var fileInfo = await sender.Send(command, cancellationToken);
        return TypedResults.File(
            fileInfo.Data,
            fileInfo.MediaTypeName ?? MediaTypeNames.Application.Octet,
            fileInfo.FileName
        );
    }
}
