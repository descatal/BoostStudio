using System.Net.Mime;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Tbl.PatchFiles;
using BoostStudio.Application.Exvs.PatchFiles.Commands;
using BoostStudio.Application.Exvs.PatchFiles.Queries;
using BoostStudio.Application.Exvs.Tbl.Commands;
using BoostStudio.Application.Exvs.Tbl.Queries;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints.Exvs.Tbl;

public class PatchFiles : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapSubgroup(this, customTagName: nameof(Tbl), areaName: DefinitionNames.Exvs)
            .MapPost(CreatePatchFile)
            .MapGet(GetPatchFilesSummaryWithPagination, "summary")
            .MapGet(GetPatchFilesWithPagination)
            .MapGet(GetPatchFileById, "{id}")
            .MapPost(UpdatePatchFileById, "{id}")
            .MapDelete(DeletePatchFileById, "{id}")
            .MapPost(ResizePatchFile, "resize");
    }

    private static async Task<PaginatedList<PatchFileSummaryVm>> GetPatchFilesSummaryWithPagination(
        ISender sender,
        [AsParameters] GetPatchFilesSummaryWithPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        return await sender.Send(request, cancellationToken);
    }

    // [Produces(MediaTypeNames.Application.Json)]
    // [ProducesResponseType(typeof(PaginatedList<PatchFileVm>), StatusCodes.Status200OK)]
    private static async Task<PaginatedList<PatchFileVm>> GetPatchFilesWithPagination(
        ISender sender,
        [AsParameters] GetPatchFilesWithPagination request,
        CancellationToken cancellationToken
    )
    {
        return await sender.Send(request, cancellationToken);
    }

    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<PatchFileVm> GetPatchFileById(
        ISender sender,
        Guid id,
        CancellationToken cancellationToken
    )
    {
        return await sender.Send(new GetPatchFilesByIdQuery(id), cancellationToken);
    }

    // [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task CreatePatchFile(
        ISender sender,
        CreatePatchFileCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
    }

    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> UpdatePatchFileById(
        ISender sender,
        Guid id,
        UpdatePatchFileByIdCommand byIdCommand,
        CancellationToken cancellationToken
    )
    {
        if (id != byIdCommand.Id)
            return Results.BadRequest();
        await sender.Send(byIdCommand, cancellationToken);
        return Results.NoContent();
    }

    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> DeletePatchFileById(
        ISender sender,
        Guid id,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(new DeletePatchFileByIdCommand(id), cancellationToken);
        return Results.NoContent();
    }

    private static async Task ResizePatchFile(
        ISender sender,
        ResizePatchFileCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
    }
}
