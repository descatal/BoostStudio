using System.Net.Mime;
using BoostStudio.Application.Exvs.UnitStats.Commands;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints.Units;

public class UnitStats : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(CreateUnitStat)
            .MapPost(UpdateUnitStat, "{id}")
            .MapPost(ImportUnitStat, "import")
            .MapPost(ExportUnitStat, "export");
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
}


