using System.Net.Mime;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Assets;
using BoostStudio.Application.Contracts.Tbl.PatchFiles;
using BoostStudio.Application.Exvs.Assets.Commands;
using BoostStudio.Application.Exvs.Assets.Queries;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints.Exvs.Assets;

public class Assets: EndpointGroupBase
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
    
    // [Produces(MediaTypeNames.Application.Json)]
    // [ProducesResponseType(typeof(PaginatedList<PatchFileDto>), StatusCodes.Status200OK)]
    private static async Task<PaginatedList<AssetFileVm>> GetAssetFilesWithPagination(
        ISender sender, 
        [AsParameters] GetAssetFilesWithPagination request,
        CancellationToken cancellationToken)
    {
        return await sender.Send(request, cancellationToken);
    }
    
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<AssetFileVm> GetAssetFileByHash(
        ISender sender, 
        uint hash,
        CancellationToken cancellationToken)
    {
        return await sender.Send(new GetAssetFilesByHashQuery(hash), cancellationToken);
    }
    
    // [ProducesResponseType(StatusCodes.Status200OK)]
    private static async Task CreateAssetFile(
        ISender sender, 
        CreateAssetFileCommand request, 
        CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
    
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> UpdateAssetFileByHash(
        ISender sender, 
        uint hash, 
        UpdateAssetFileByHashCommand request, 
        CancellationToken cancellationToken)
    {
        if (hash != request.Hash) return Results.BadRequest();
        await sender.Send(request, cancellationToken);
        return Results.NoContent();
    }
    
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> DeleteAssetFileByHash(
        ISender sender, 
        uint hash, 
        CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteAssetFileByHashCommand(hash), cancellationToken);
        return Results.NoContent();
    }
    
    // [ProducesResponseType(StatusCodes.Status201Created)]
    private static async Task<IResult> ImportAssetFiles(
        ISender sender, 
        [FromForm] IFormFileCollection files, 
        CancellationToken cancellationToken)
    {
        var fileStreams = files.Select(formFile => formFile.OpenReadStream()).ToArray();
        await sender.Send(new ImportAssetFilesCommand(fileStreams), cancellationToken);

        foreach (var fileStream in fileStreams)
            await fileStream.DisposeAsync();

        return Results.Created();
    }
}

