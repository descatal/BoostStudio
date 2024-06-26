using System.Text.Json;
using BoostStudio.Application.Contracts.Metadata.Models;
using BoostStudio.Application.Formats.TblFormat.Commands;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Mvc;
using ContentType = System.Net.Mime.MediaTypeNames;

namespace BoostStudio.Web.Endpoints.Exvs;

/// <summary>
/// Serialize / Deserialize Tbl files
/// </summary>
public class Tbl : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs)
            .MapGet(DeserializeTblFilePath, "deserialize-path")
            .MapPost(DeserializeTblFileStream, "deserialize")
            .MapGet(SerializeTblFilePath, "serialize-path")
            .MapPost(SerializeTbl, "serialize");
    }

    public async Task<TblMetadata> DeserializeTblFilePath(ISender sender, string filePath, CancellationToken cancellationToken)
    {
        var inputBytes = await File.ReadAllBytesAsync(filePath, cancellationToken);
        return await sender.Send(new DeserializeTbl(inputBytes), cancellationToken);
    }
    
    public async Task<TblMetadata> DeserializeTblFileStream(ISender sender, IFormFile file, CancellationToken cancellationToken)
    {
        await using Stream stream = file.OpenReadStream();
        using BinaryReader binaryReader = new(stream);
        var inputBytes = binaryReader.ReadBytes((int)stream.Length);

        return await sender.Send(new DeserializeTbl(inputBytes), cancellationToken);
    }
    
    public async Task<IResult> SerializeTblFilePath(ISender sender, [FromQuery] string filePath, CancellationToken cancellationToken)
    {
        var jsonPayload = await File.ReadAllTextAsync(filePath, cancellationToken);
        var command = JsonSerializer.Deserialize<SerializeTbl>(jsonPayload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        
        if (command is null)
            return Results.BadRequest();
        
        var serializedFile = await sender.Send(command, cancellationToken);
        return Results.File(serializedFile, ContentType.Application.Octet, "PATCH.TBL");
    }
    
    /// <summary>
    /// Serialize json metadata to a TBL file.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Consumes(ContentType.Application.Json)]
    public async Task<IResult> SerializeTbl(ISender sender, SerializeTbl command, CancellationToken cancellationToken)
    {
        var serializedFile = await sender.Send(command, cancellationToken);
        return Results.File(serializedFile, ContentType.Application.Octet, "PATCH.TBL");
    }
}
