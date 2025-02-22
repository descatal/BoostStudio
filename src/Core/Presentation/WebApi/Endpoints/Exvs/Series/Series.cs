﻿using System.Net.Mime;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Series;
using BoostStudio.Application.Exvs.Projectiles.Queries.UnitProjectile;
using BoostStudio.Application.Exvs.Series.Commands;
using BoostStudio.Application.Exvs.Series.Queries;
using BoostStudio.Web.Constants;
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

    private static async Task<PaginatedList<SeriesDto>> GetSeriesWithPagination(
        ISender sender,
        [AsParameters] GetSeriesWithPaginationQuery request)
    {
        return await sender.Send(request);
    }

    private static async Task<PaginatedList<SeriesUnitsVm>> GetSeriesUnitsWithPagination(
        ISender sender,
        [AsParameters] GetSeriesUnitsWithPaginationQuery request)
    {
        return await sender.Send(request);
    }


    private static async Task<IResult> CreatePlayableSeries(
        ISender sender,
        CreateSeriesCommand command,
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.Created();
    }
    
    private static async Task<IResult> ImportPlayableSeries(
        ISender sender,
        [FromForm] IFormFile file,
        CancellationToken cancellationToken)
    {
        var fileStream = file.OpenReadStream();
        await sender.Send(new ImportPlayableSeriesCommand(fileStream), cancellationToken);
        await fileStream.DisposeAsync();

        return Results.Created();
    }

    private static async Task<IResult> ExportPlayableSeries(
        ISender sender,
        ExportPlayableSeriesCommand command,
        CancellationToken cancellationToken)
    {
        var fileInfo = await sender.Send(command, cancellationToken);
        return Results.File(fileInfo.Data, fileInfo.MediaTypeName ?? MediaTypeNames.Application.Octet, fileInfo.FileName);
    }
}
