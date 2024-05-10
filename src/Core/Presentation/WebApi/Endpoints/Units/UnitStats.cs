using System.Net.Mime;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Units;
using BoostStudio.Application.Exvs.UnitAmmoSlots.Commands;
using BoostStudio.Application.Exvs.UnitStats.Commands;
using BoostStudio.Application.Exvs.UnitStats.Queries;
using Microsoft.AspNetCore.Mvc;
using ContentType=System.Net.Mime.MediaTypeNames;

namespace BoostStudio.Web.Endpoints.Units;

public class UnitStats : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetUnitStatsWithPagination)
            .MapPost(CreateUnitStat)
            .MapPost(UpdateUnitStat, "{id}")
            .MapPost(ImportUnitStat, "import")
            .MapPost(ExportUnitStat, "export")
            .MapPost(CreateUnitAmmoSlot, "ammo-slot")
            .MapPost(UpdateUnitAmmoSlot, "ammo-slot/{id}");
    }
    
    [Produces(ContentType.Application.Json)]
    [ProducesResponseType(typeof(PaginatedList<UnitStatDto>), StatusCodes.Status200OK)]
    private static Task<PaginatedList<UnitStatDto>> GetUnitStatsWithPagination(ISender sender, [AsParameters] GetUnitStatQuery query)
    {
        return sender.Send(query);
    }
    
    private static async Task<IResult> CreateUnitStat(ISender sender, CreateUnitStatCommand command, CancellationToken cancellationToken)
    {
        return Results.Ok(await sender.Send(command, cancellationToken));
    }
    
    private static async Task<IResult> UpdateUnitStat(ISender sender, Guid id, UpdateUnitStatCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
    
    private static async Task ImportUnitStat(ISender sender, [FromForm] IFormFileCollection files, CancellationToken cancellationToken)
    {
        var fileStreams = files.Select(formFile => formFile.OpenReadStream()).ToArray();
        await sender.Send(new ImportUnitStatCommand(fileStreams), cancellationToken);

        foreach (var fileStream in fileStreams)
            await fileStream.DisposeAsync();
    }
    
    private static async Task<IResult> ExportUnitStat(ISender sender, ExportUnitStatCommand command, CancellationToken cancellationToken)
    {
        var fileInfo = await sender.Send(command, cancellationToken);
        return Results.File(fileInfo.Data, fileInfo.MediaTypeName ?? MediaTypeNames.Application.Octet, fileInfo.FileName);
    }
    
    private static async Task<IResult> CreateUnitAmmoSlot(ISender sender, CreateUnitAmmoSlotCommand command, CancellationToken cancellationToken)
    {
        return Results.Ok(await sender.Send(command, cancellationToken));
    }
    
    private static async Task<IResult> UpdateUnitAmmoSlot(ISender sender, Guid id, UnitAmmoSlotDetails details, CancellationToken cancellationToken)
    {
        await sender.Send(new UpdateUnitAmmoSlotCommand(id, details), cancellationToken);
        return Results.NoContent();
    }
}
