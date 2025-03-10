﻿using System.Net.Mime;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Exvs.Fhm.Commands;
using BoostStudio.Application.Formats.FhmFormat.Commands;
using BoostStudio.Web.Constants;

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
    
    private async Task PackFhmPath(ISender sender, [AsParameters] PackFhmByPathCommand request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
    
    private async Task UnpackFhmPath(ISender sender, [AsParameters] UnpackFhmByPathCommand request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }

    private async Task<IResult> UnpackFhm(ISender sender, IFormFile file, CancellationToken cancellationToken)
    {
        var compressionFormat = CompressionFormats.Tar;
        await using var stream = file.OpenReadStream();
        using BinaryReader binaryReader = new(stream);
        var inputBytes = binaryReader.ReadBytes((int)stream.Length);

        var unpackedFile = await sender.Send(new UnpackFhm(inputBytes, false, compressionFormat), cancellationToken);

        string fileName = Path.ChangeExtension(Path.GetFileNameWithoutExtension(file.FileName), compressionFormat.ToString().ToLower());
        return compressionFormat switch
        {
            CompressionFormats.Zip => Results.File(unpackedFile, MediaTypeNames.Application.Zip, fileName),
            _ => Results.File(unpackedFile, MediaTypeNames.Application.Octet, fileName)
        };
    }

    private async Task<IResult> PackFhm(ISender sender, IFormFile file, CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();
        using BinaryReader binaryReader = new(stream);
        var inputBytes = binaryReader.ReadBytes((int)stream.Length);

        var packedFile = await sender.Send(new PackFhm(inputBytes), cancellationToken);

        string fhmFileName = Path.ChangeExtension(Path.GetFileNameWithoutExtension(file.FileName), "fhm");
        return Results.File(packedFile, MediaTypeNames.Application.Octet, fhmFileName);
    }
    
    private async Task PackFhmAsset(
        ISender sender, 
        PackFhmAssetCommand request, 
        CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
    
    private async Task UnpackFhmAsset(
        ISender sender, 
        UnpackFhmAssetCommand request, 
        CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
}
