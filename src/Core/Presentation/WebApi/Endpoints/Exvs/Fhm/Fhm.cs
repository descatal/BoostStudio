using System.Net.Mime;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Exvs.Fhm.Commands;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints.Exvs.Fhm;

public class Fhm : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs)
            .MapGet(PackFhmPath, "pack-path")
            .MapGet(UnpackFhmPath, "unpack-path")
            .MapPost(PackFhm, "pack")
            .MapPost(UnpackFhm, "unpack")
            .MapPost(PackFhmAsset, "pack/asset")
            .MapPost(UnpackFhmAsset, "unpack/asset");
    }

    private static async Task<NoContent> PackFhmPath(
        ISender sender,
        [AsParameters] PackFhmByPathCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> UnpackFhmPath(
        ISender sender,
        [AsParameters] UnpackFhmByPathCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.NoContent();
    }

    // Needed for OpenApi to recognize the return type as FileContentHttpResult, which will be converted to Blob
    [ProducesResponseType(
        type: typeof(FileContentHttpResult),
        statusCode: StatusCodes.Status200OK,
        contentType: MediaTypeNames.Application.Octet
    )]
    private static async Task<FileContentHttpResult> UnpackFhm(
        ISender sender,
        IFormFile file,
        CancellationToken cancellationToken,
        [FromQuery] CompressionFormats compressionFormat = CompressionFormats.Tar
    )
    {
        await using var stream = file.OpenReadStream();
        using BinaryReader binaryReader = new(stream);
        var inputBytes = binaryReader.ReadBytes((int)stream.Length);

        var unpackedFile = await sender.Send(
            new UnpackFhmCommand(inputBytes, false, compressionFormat),
            cancellationToken
        );

        string fileName = Path.ChangeExtension(
            Path.GetFileNameWithoutExtension(file.FileName),
            compressionFormat.ToString().ToLower()
        );
        return compressionFormat switch
        {
            CompressionFormats.Zip => TypedResults.File(
                unpackedFile,
                MediaTypeNames.Application.Zip,
                fileName
            ),
            _ => TypedResults.File(unpackedFile, MediaTypeNames.Application.Octet, fileName),
        };
    }

    // Needed for OpenApi to recognize the return type as FileContentHttpResult, which will be converted to Blob
    [ProducesResponseType(
        type: typeof(FileContentHttpResult),
        statusCode: StatusCodes.Status200OK,
        contentType: MediaTypeNames.Application.Octet
    )]
    private async Task<FileContentHttpResult> PackFhm(
        ISender sender,
        IFormFile file,
        CancellationToken cancellationToken
    )
    {
        await using var stream = file.OpenReadStream();
        using BinaryReader binaryReader = new(stream);
        var inputBytes = binaryReader.ReadBytes((int)stream.Length);

        var packedFile = await sender.Send(new PackFhmCommand(inputBytes), cancellationToken);

        string fhmFileName = Path.ChangeExtension(
            Path.GetFileNameWithoutExtension(file.FileName),
            "fhm"
        );

        // This will automatically embed the filename in response's Content-Disposition header, with the body returned as octet-stream binary
        return TypedResults.File(packedFile, MediaTypeNames.Application.Octet, fhmFileName);
    }

    private static async Task<NoContent> PackFhmAsset(
        ISender sender,
        PackFhmByAssetCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> UnpackFhmAsset(
        ISender sender,
        UnpackFhmByAssetCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.NoContent();
    }
}
