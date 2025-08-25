using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Tbl.PatchFiles;
using BoostStudio.Application.Exvs.PatchFiles.Commands;
using BoostStudio.Application.Exvs.PatchFiles.Queries;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Http.HttpResults;

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

    private static async Task<
        Ok<PaginatedList<PatchFileSummaryVm>>
    > GetPatchFilesSummaryWithPagination(
        ISender sender,
        [AsParameters] GetPatchFilesSummaryWithPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        var paginatedList = await sender.Send(request, cancellationToken);
        return TypedResults.Ok(paginatedList);
    }

    private static async Task<Ok<PaginatedList<PatchFileVm>>> GetPatchFilesWithPagination(
        ISender sender,
        [AsParameters] GetPatchFilesWithPagination request,
        CancellationToken cancellationToken
    )
    {
        var paginatedList = await sender.Send(request, cancellationToken);
        return TypedResults.Ok(paginatedList);
    }

    private static async Task<Ok<PatchFileVm>> GetPatchFileById(
        ISender sender,
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var vm = await sender.Send(new GetPatchFilesByIdQuery(id), cancellationToken);
        return TypedResults.Ok(vm);
    }

    private static async Task<Created> CreatePatchFile(
        ISender sender,
        CreatePatchFileCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return TypedResults.Created();
    }

    private static async Task<NoContent> UpdatePatchFileById(
        ISender sender,
        Guid id,
        UpdatePatchFileByIdCommand byIdCommand,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(byIdCommand, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> DeletePatchFileById(
        ISender sender,
        Guid id,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(new DeletePatchFileByIdCommand(id), cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> ResizePatchFile(
        ISender sender,
        ResizePatchFileCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return TypedResults.NoContent();
    }
}
