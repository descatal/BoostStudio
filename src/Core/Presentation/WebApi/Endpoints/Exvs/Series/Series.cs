using System.Net.Mime;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Series;
using BoostStudio.Application.Exvs.Projectiles.Queries.UnitProjectile;
using BoostStudio.Application.Exvs.Series.Commands;
using BoostStudio.Application.Exvs.Series.Queries;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints.Exvs.Series;

public class Series : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapSubgroup(this, customTagName: nameof(Exvs.Series), areaName: DefinitionNames.Exvs)
            .MapGet(GetSeriesWithPagination)
            .MapGet(GetSeriesUnitsWithPagination, "/units")
            .MapPost(CreatePlayableSeries)
            .MapPost(ImportPlayableSeries, "import")
            .MapPost(ExportPlayableSeries, "export");
    }

    private static async Task<Ok<PaginatedList<SeriesDto>>> GetSeriesWithPagination(
        ISender sender,
        [AsParameters] GetSeriesWithPaginationQuery request
    )
    {
        var paginatedList = await sender.Send(request);
        return TypedResults.Ok(paginatedList);
    }

    private static async Task<Ok<PaginatedList<SeriesUnitsVm>>> GetSeriesUnitsWithPagination(
        ISender sender,
        [AsParameters] GetSeriesUnitsWithPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        var paginatedList = await sender.Send(request, cancellationToken);
        return TypedResults.Ok(paginatedList);
    }

    private static async Task<Created> CreatePlayableSeries(
        ISender sender,
        CreateSeriesCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return TypedResults.Created();
    }

    private static async Task<Created> ImportPlayableSeries(
        ISender sender,
        [FromForm] IFormFile file,
        CancellationToken cancellationToken
    )
    {
        var fileStream = file.OpenReadStream();
        await sender.Send(new ImportPlayableSeriesCommand(fileStream), cancellationToken);
        await fileStream.DisposeAsync();

        return TypedResults.Created();
    }

    [ProducesResponseType(
        type: typeof(FileContentHttpResult),
        statusCode: StatusCodes.Status200OK,
        contentType: MediaTypeNames.Application.Octet
    )]
    private static async Task<FileContentHttpResult> ExportPlayableSeries(
        ISender sender,
        ExportPlayableSeriesCommand command,
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
