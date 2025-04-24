using System.Net.Mime;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Stats.UnitStats;
using BoostStudio.Application.Exvs.Stats.Commands.AmmoSlot;
using BoostStudio.Application.Exvs.Stats.Commands.UnitStat;
using BoostStudio.Application.Exvs.Stats.Queries.AmmoSlot;
using BoostStudio.Application.Exvs.Stats.Queries.UnitStat;
using BoostStudio.Web.Constants;
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

    private static async Task<PaginatedList<UnitStatDto>> GetUnitStatWithPagination(
        ISender sender,
        [AsParameters] GetUnitStatWithPaginationQuery request
    )
    {
        return await sender.Send(request);
    }

    private static async Task<UnitStatDto> GetUnitStatByUnitId(
        ISender sender,
        [FromRoute] uint unitId
    )
    {
        return await sender.Send(new GetUnitStatByUnitIdQuery(unitId));
    }

    private static async Task<IResult> ImportUnitStat(
        ISender sender,
        [FromForm] IFormFileCollection files,
        CancellationToken cancellationToken
    )
    {
        var fileStreams = files.Select(formFile => formFile.OpenReadStream()).ToArray();
        await sender.Send(new ImportUnitStatCommand(fileStreams), cancellationToken);

        foreach (var fileStream in fileStreams)
            await fileStream.DisposeAsync();

        return Results.Created();
    }

    private static async Task<IResult> ExportUnitStat(
        ISender sender,
        ExportUnitStatCommand command,
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

    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> ExportUnitStatByPath(
        ISender sender,
        ExportUnitStatByPathCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<List<UnitAmmoSlotDto>> GetUnitAmmoSlotByUnitId(
        ISender sender,
        [FromRoute] uint unitId
    )
    {
        return await sender.Send(new GetUnitAmmoSlotByUnitIdQuery(unitId));
    }

    // [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    private static async Task<Guid> CreateUnitAmmoSlot(
        ISender sender,
        CreateUnitAmmoSlotCommand command,
        CancellationToken cancellationToken
    )
    {
        return await sender.Send(command, cancellationToken);
    }

    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> UpdateUnitAmmoSlotById(
        ISender sender,
        Guid id,
        UpdateUnitAmmoSlotCommand command,
        CancellationToken cancellationToken
    )
    {
        if (id != command.Id)
            return Results.BadRequest();
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }

    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> DeleteUnitAmmoSlotById(
        ISender sender,
        Guid id,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(new DeleteUnitAmmoSlotCommand(id), cancellationToken);
        return Results.NoContent();
    }
}
