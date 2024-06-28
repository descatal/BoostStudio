using System.Net.Mime;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Stats.UnitStats;
using BoostStudio.Application.Exvs.Stats.Commands.UnitStat;
using BoostStudio.Application.Exvs.Stats.Queries.AmmoSlot;
using BoostStudio.Application.Exvs.Stats.Queries.UnitStat;
using BoostStudio.Application.Exvs.UnitAmmoSlots.Commands;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Mvc;
using ContentType=System.Net.Mime.MediaTypeNames;

namespace BoostStudio.Web.Endpoints.Exvs.Stats;

public class UnitStats : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs)
            .MapGet(GetUnitStatWithPagination)
            .MapGet(GetUnitStatById, "{unitId}")
            .MapPost(ImportUnitStat, "import")
            .MapPost(ExportUnitStat, "export")
            .MapPost(ExportUnitStatById, "export/{unitId}")
            .MapGet(GetUnitAmmoSlotByUnitId, "ammo-slot/{unitId}")
            .MapPost(CreateUnitAmmoSlot, "ammo-slot")
            .MapPost(UpdateUnitAmmoSlot, "ammo-slot/{id}");
    }

    [Produces(ContentType.Application.Json)]
    [ProducesResponseType(typeof(PaginatedList<UnitStatDto>), StatusCodes.Status200OK)]
    private static Task<PaginatedList<UnitStatDto>> GetUnitStatWithPagination(ISender sender, [AsParameters] GetUnitStatWithPaginationQuery request)
    {
        return sender.Send(request);
    }

    [Produces(ContentType.Application.Json)]
    [ProducesResponseType(typeof(UnitStatDto), StatusCodes.Status200OK)]
    private static Task<UnitStatDto> GetUnitStatById(ISender sender, [FromRoute] uint unitId)
    {
        return sender.Send(new GetUnitStatByUnitIdQuery(unitId));
    }

    private static async Task<IResult> ImportUnitStat(ISender sender, [FromForm] IFormFileCollection files, CancellationToken cancellationToken)
    {
        var fileStreams = files.Select(formFile => formFile.OpenReadStream()).ToArray();
        await sender.Send(new ImportUnitStatCommand(fileStreams), cancellationToken);

        foreach (var fileStream in fileStreams)
            await fileStream.DisposeAsync();

        return Results.Created();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> ExportUnitStat(ISender sender, ExportUnitStatCommand command, CancellationToken cancellationToken)
    {
        var fileInfo = await sender.Send(command, cancellationToken);
        return Results.File(fileInfo.Data, fileInfo.MediaTypeName ?? MediaTypeNames.Application.Octet, fileInfo.FileName);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task<IResult> ExportUnitStatById(ISender sender, [FromRoute] uint unitId, CancellationToken cancellationToken)
    {
        var fileInfo = await sender.Send(new ExportUnitStatByIdCommand(unitId), cancellationToken);
        return Results.File(fileInfo.Data, fileInfo.MediaTypeName ?? MediaTypeNames.Application.Octet, fileInfo.FileName);
    }

    [Produces(ContentType.Application.Json)]
    [ProducesResponseType(typeof(List<UnitAmmoSlotDto>), StatusCodes.Status200OK)]
    private static Task<List<UnitAmmoSlotDto>> GetUnitAmmoSlotByUnitId(ISender sender, [FromRoute] uint unitId)
    {
        return sender.Send(new GetUnitAmmoSlotByUnitIdQuery(unitId));
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    private static async Task<IResult> CreateUnitAmmoSlot(ISender sender, CreateUnitAmmoSlotCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.Created();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> UpdateUnitAmmoSlot(ISender sender, Guid id, UpdateUnitAmmoSlotCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
}
