using System.Net;
using System.Net.Mime;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Exvs.Fhm.Commands;
using BoostStudio.Application.Formats.FhmFormat.Commands;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints.Exvs;

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

    private async Task PackFhmPath(
        ISender sender,
        [AsParameters] PackFhmByPathCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
    }

    private async Task UnpackFhmPath(
        ISender sender,
        [AsParameters] UnpackFhmByPathCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
    }

    // Needed for OpenApi to recognize the return type as FileContentHttpResult, which will be converted to Blob
    [ProducesResponseType(
        type: typeof(FileContentHttpResult),
        statusCode: StatusCodes.Status200OK,
        contentType: MediaTypeNames.Application.Octet
    )]
    private async Task<IResult> UnpackFhm(
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
            new UnpackFhm(inputBytes, false, compressionFormat),
            cancellationToken
        );

        string fileName = Path.ChangeExtension(
            Path.GetFileNameWithoutExtension(file.FileName),
            compressionFormat.ToString().ToLower()
        );
        return compressionFormat switch
        {
            CompressionFormats.Zip => Results.File(
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

        var packedFile = await sender.Send(new PackFhm(inputBytes), cancellationToken);

        string fhmFileName = Path.ChangeExtension(
            Path.GetFileNameWithoutExtension(file.FileName),
            "fhm"
        );

        // This will automatically embed the filename in response's Content-Disposition header, with the body returned as octet-stream binary
        return TypedResults.File(packedFile, MediaTypeNames.Application.Octet, fhmFileName);
    }

    private async Task PackFhmAsset(
        ISender sender,
        PackFhmAssetCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
    }

    private async Task UnpackFhmAsset(
        ISender sender,
        UnpackFhmAssetCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
    }
}
