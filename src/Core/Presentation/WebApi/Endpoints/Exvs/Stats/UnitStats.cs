using System.Net.Mime;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Stats.UnitStats;
using BoostStudio.Application.Exvs.Stats.Commands.AmmoSlot;
using BoostStudio.Application.Exvs.Stats.Commands.UnitStat;
using BoostStudio.Application.Exvs.Stats.Queries.AmmoSlot;
using BoostStudio.Application.Exvs.Stats.Queries.UnitStat;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ContentType = System.Net.Mime.MediaTypeNames;

namespace BoostStudio.Web.Endpoints.Exvs.Stats;

public class UnitStats : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapSubgroup(this, customTagName: nameof(Stats), areaName: DefinitionNames.Exvs)
            .MapGet(GetUnitStatWithPagination)
            .MapGet(GetUnitStatByUnitId, "{unitId}")
            .MapPost(ImportUnitStat, "import")
            .MapPost(ExportUnitStat, "export")
            .MapPost(ExportUnitStatByPath, "export/path");

        app.MapSubgroup(this, customTagName: nameof(Stats), areaName: DefinitionNames.Exvs)
            .MapGet(GetUnitAmmoSlotByUnitId, "ammo-slot/{unitId}")
            .MapPost(CreateUnitAmmoSlot, "ammo-slot")
            .MapPost(UpdateUnitAmmoSlotById, "ammo-slot/{id}")
            .MapDelete(DeleteUnitAmmoSlotById, "ammo-slot/{id}");
    }

    private static async Task<Ok<PaginatedList<UnitStatDto>>> GetUnitStatWithPagination(
        ISender sender,
        [AsParameters] GetUnitStatWithPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        var paginatedList = await sender.Send(request, cancellationToken);
        return TypedResults.Ok(paginatedList);
    }

    private static async Task<Ok<UnitStatDto>> GetUnitStatByUnitId(
        ISender sender,
        [FromRoute] uint unitId,
        CancellationToken cancellationToken
    )
    {
        var vm = await sender.Send(new GetUnitStatByUnitIdQuery(unitId));
        return TypedResults.Ok(vm);
    }

    private static async Task<Created> ImportUnitStat(
        ISender sender,
        [FromForm] IFormFileCollection files,
        CancellationToken cancellationToken
    )
    {
        var fileStreams = files.Select(formFile => formFile.OpenReadStream()).ToArray();
        await sender.Send(new ImportUnitStatCommand(fileStreams), cancellationToken);

        foreach (var fileStream in fileStreams)
            await fileStream.DisposeAsync();

        return TypedResults.Created();
    }

    [ProducesResponseType(
        type: typeof(FileContentHttpResult),
        statusCode: StatusCodes.Status200OK,
        contentType: MediaTypeNames.Application.Octet
    )]
    private static async Task<FileContentHttpResult> ExportUnitStat(
        ISender sender,
        ExportUnitStatCommand command,
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

    private static async Task<NoContent> ExportUnitStatByPath(
        ISender sender,
        ExportUnitStatByPathCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<Ok<List<UnitAmmoSlotDto>>> GetUnitAmmoSlotByUnitId(
        ISender sender,
        [FromRoute] uint unitId,
        CancellationToken cancellationToken
    )
    {
        var vm = await sender.Send(new GetUnitAmmoSlotByUnitIdQuery(unitId), cancellationToken);
        return TypedResults.Ok(vm);
    }

    private static async Task<Created> CreateUnitAmmoSlot(
        ISender sender,
        CreateUnitAmmoSlotCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return TypedResults.Created();
    }

    private static async Task<NoContent> UpdateUnitAmmoSlotById(
        ISender sender,
        Guid id,
        UpdateUnitAmmoSlotCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> DeleteUnitAmmoSlotById(
        ISender sender,
        Guid id,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(new DeleteUnitAmmoSlotCommand(id), cancellationToken);
        return TypedResults.NoContent();
    }
}
