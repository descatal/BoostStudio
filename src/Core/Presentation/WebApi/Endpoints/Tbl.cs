using System.Net.Mime;
using BoostStudio.Application.Common.Models;
using BoostStudio.Application.Contracts.Metadata.Models;
using BoostStudio.Application.Formats.FhmFormat.Commands;
using BoostStudio.Application.Formats.TblFormat.Commands;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints;

public class Tbl : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(DeserializeTbl, "deserialize", true)
            .MapPost(SerializeTbl, "serialize", true);
    }

    public async Task<TblMetadata> DeserializeTbl(ISender sender, IFormFile file, CancellationToken cancellationToken)
    {
        await using Stream stream = file.OpenReadStream();
        using BinaryReader binaryReader = new(stream);
        var inputBytes = binaryReader.ReadBytes((int)stream.Length);

        return await sender.Send(new DeserializeTbl(inputBytes), cancellationToken);
    }
    
    public async Task<IResult> SerializeTbl(ISender sender, SerializeTbl command, CancellationToken cancellationToken)
    {
        var serializedFile = await sender.Send(command, cancellationToken);
        return Results.File(serializedFile, MediaTypeNames.Application.Octet, "PATCH.TBL");
    }
}
