using System.Net.Mime;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Assets;
using BoostStudio.Application.Contracts.Tbl.PatchFiles;
using BoostStudio.Application.Exvs.Assets.Commands;
using BoostStudio.Application.Exvs.Assets.Queries;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints.Exvs.Assets;

public class Assets : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapSubgroup(this, customTagName: nameof(Assets), areaName: DefinitionNames.Exvs)
            .MapPost(CreateAssetFile)
            .MapGet(GetAssetFilesWithPagination)
            .MapGet(GetAssetFileByHash, "{hash}")
            .MapPost(UpdateAssetFileByHash, "{hash}")
            .MapDelete(DeleteAssetFileByHash, "{hash}")
            .MapPost(ImportAssetFiles, "import");
    }

    private static async Task<Ok<PaginatedList<AssetFileVm>>> GetAssetFilesWithPagination(
        ISender sender,
        [AsParameters] GetAssetFilesWithPagination request,
        CancellationToken cancellationToken
    )
    {
        var paginatedList = await sender.Send(request, cancellationToken);
        return TypedResults.Ok(paginatedList);
    }

    private static async Task<Ok<AssetFileVm>> GetAssetFileByHash(
        ISender sender,
        uint hash,
        CancellationToken cancellationToken
    )
    {
        var vm = await sender.Send(new GetAssetFilesByHashQuery(hash), cancellationToken);
        return TypedResults.Ok(vm);
    }

    private static async Task<Created> CreateAssetFile(
        ISender sender,
        CreateAssetFileCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.Created();
    }

    private static async Task<NoContent> UpdateAssetFileByHash(
        ISender sender,
        uint hash,
        UpdateAssetFileByHashCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> DeleteAssetFileByHash(
        ISender sender,
        uint hash,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(new DeleteAssetFileByHashCommand(hash), cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<Created> ImportAssetFiles(
        ISender sender,
        [FromForm] IFormFileCollection files,
        CancellationToken cancellationToken
    )
    {
        var fileStreams = files.Select(formFile => formFile.OpenReadStream()).ToArray();
        await sender.Send(new ImportAssetFilesCommand(fileStreams), cancellationToken);

        foreach (var fileStream in fileStreams)
            await fileStream.DisposeAsync();

        return TypedResults.Created();
    }
}
