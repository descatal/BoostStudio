using System.Net.Mime;
using BoostStudio.Application.Exvs.Nud.Commands;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints.Exvs.Nud;

public class Nud : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs).MapPost(ConvertAsync);
        // .MapPost(ConvertToVmAsync, "json")
    }

    // Needed for OpenApi to recognize the return type as FileContentHttpResult, which will be converted to Blob
    [ProducesResponseType(
        type: typeof(FileContentHttpResult),
        statusCode: StatusCodes.Status200OK,
        contentType: MediaTypeNames.Application.Octet
    )]
    private static async Task<FileContentHttpResult> ConvertAsync(
        ISender sender,
        IFormFile nudFile,
        IFormFile? vbnFile,
        CancellationToken cancellationToken
    )
    {
        var fileName = Path.GetFileNameWithoutExtension(nudFile.FileName);
        await using var nudFileStream = nudFile.OpenReadStream();
        await using var vbnFileStream = vbnFile?.OpenReadStream();

        var convertedFile = await sender.Send(
            new ConvertNudCommand(nudFileStream, VbnFile: vbnFileStream, FileName: fileName),
            cancellationToken
        );

        return TypedResults.File(convertedFile.Data, fileDownloadName: convertedFile.FileName);
    }

    // [ProducesResponseType(
    //     type: typeof(FileContentHttpResult),
    //     statusCode: StatusCodes.Status200OK,
    //     contentType: MediaTypeNames.Application.Octet
    // )]
    // private static async Task<FileContentHttpResult> ConvertToVmAsync(
    //     ISender sender,
    //     IFormFile nudFile,
    //     IFormFile vbnFile,
    //     CancellationToken cancellationToken
    // )
    // {
    //     await using var nudFileStream = nudFile.OpenReadStream();
    //     await using var vbnFileStream = vbnFile.OpenReadStream();
    //
    //     var file = await sender.Send(
    //         new ConvertNudToJsonCommand(nudFileStream, VbnFile: vbnFileStream),
    //         cancellationToken
    //     );
    //
    //     return TypedResults.File(file.Data, fileDownloadName: file.FileName);
    // }
}
