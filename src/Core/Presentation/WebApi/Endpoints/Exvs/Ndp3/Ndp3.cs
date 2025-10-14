using System.Net.Mime;
using BoostStudio.Application.Exvs.Ndp3.Commands;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints.Exvs.Ndp3;

public class Ndp3 : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs)
            .MapPost(ConvertAsync)
            .MapPost(ConvertToVmAsync, "json");
    }

    // Needed for OpenApi to recognize the return type as FileContentHttpResult, which will be converted to Blob
    [ProducesResponseType(
        type: typeof(FileContentHttpResult),
        statusCode: StatusCodes.Status200OK,
        contentType: MediaTypeNames.Application.Octet
    )]
    private static async Task<FileContentHttpResult> ConvertAsync(
        ISender sender,
        IFormFile ndp3File,
        IFormFile? vbnFile,
        CancellationToken cancellationToken
    )
    {
        var fileName = Path.GetFileNameWithoutExtension(ndp3File.FileName);
        await using var ndp3FileStream = ndp3File.OpenReadStream();
        await using var vbnFileStream = vbnFile?.OpenReadStream();

        var convertedFile = await sender.Send(
            new ConvertNdp3Command(ndp3FileStream, VbnFile: vbnFileStream, FileName: fileName),
            cancellationToken
        );

        return TypedResults.File(convertedFile.Data, fileDownloadName: convertedFile.FileName);
    }

    [ProducesResponseType(
        type: typeof(FileContentHttpResult),
        statusCode: StatusCodes.Status200OK,
        contentType: MediaTypeNames.Application.Octet
    )]
    private static async Task<FileContentHttpResult> ConvertToVmAsync(
        ISender sender,
        IFormFile ndp3File,
        IFormFile vbnFile,
        CancellationToken cancellationToken
    )
    {
        await using var ndp3FileStream = ndp3File.OpenReadStream();
        await using var vbnFileStream = vbnFile.OpenReadStream();

        var file = await sender.Send(
            new ConvertNdp3ToJsonCommand(ndp3FileStream, VbnFile: vbnFileStream),
            cancellationToken
        );

        return TypedResults.File(file.Data, fileDownloadName: file.FileName);
    }
}
