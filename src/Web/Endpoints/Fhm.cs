using System.Net.Mime;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Formats.Fhm.Commands;
using BoostStudio.Web.Contracts.Fhm;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints;

public class Fhm : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(Unpack, "unpack", true)
            .MapPost(Pack, "pack", true);
    }

    public async Task<IResult> Unpack(ISender sender, IFormFile file, CancellationToken cancellationToken)
    {
        var compressionFormat = CompressionFormats.Zip;
        await using Stream stream = file.OpenReadStream();
        using BinaryReader binaryReader = new(stream);
        var inputBytes = binaryReader.ReadBytes((int)stream.Length);

        var unpackedFile = await sender.Send(new UnpackFhm(inputBytes, false, compressionFormat), cancellationToken);

        string zipFileName = Path.ChangeExtension(Path.GetFileNameWithoutExtension(file.FileName), compressionFormat.ToString().ToLower());
        return Results.File(unpackedFile, MediaTypeNames.Application.Zip, zipFileName);
    }
    
    public async Task<IResult> Pack(ISender sender, IFormFile file, CancellationToken cancellationToken)
    {
        await using Stream stream = file.OpenReadStream();
        using BinaryReader binaryReader = new(stream);
        var inputBytes = binaryReader.ReadBytes((int)stream.Length);
        
        var packedFile = await sender.Send(new PackFhm(inputBytes), cancellationToken);

        string zipFileName = Path.ChangeExtension(Path.GetFileNameWithoutExtension(file.FileName), "fhm");
        return Results.File(packedFile, MediaTypeNames.Application.Zip, zipFileName);
    }
}
