using System.Text.Json;
using BoostStudio.Application.Contracts.Tbl;
using BoostStudio.Application.Exvs.PatchFiles.Commands;
using BoostStudio.Application.Exvs.Tbl.Commands;
using BoostStudio.Application.Exvs.Tbl.Queries;
using BoostStudio.Domain.Enums;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ContentType = System.Net.Mime.MediaTypeNames;
using TblDto = BoostStudio.Application.Contracts.Metadata.Models.TblDto;

namespace BoostStudio.Web.Endpoints.Exvs.Tbl;

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
            .MapPost(SerializeTbl, "serialize")
            .MapGet(GetTblById, "{id}")
            .MapPost(ImportTbl, "import")
            .MapPost(ExportTbl, "export");
    }

    private static async Task<Ok<TblDto>> DeserializeTblFilePath(
        ISender sender,
        string filePath,
        CancellationToken cancellationToken
    )
    {
        var inputBytes = await File.ReadAllBytesAsync(filePath, cancellationToken);
        var vm = await sender.Send(new DeserializeTblCommand(inputBytes), cancellationToken);
        return TypedResults.Ok(vm);
    }

    private static async Task<Ok<TblDto>> DeserializeTblFileStream(
        ISender sender,
        IFormFile file,
        CancellationToken cancellationToken
    )
    {
        await using Stream stream = file.OpenReadStream();
        using BinaryReader binaryReader = new(stream);
        var inputBytes = binaryReader.ReadBytes((int)stream.Length);

        var vm = await sender.Send(new DeserializeTblCommand(inputBytes), cancellationToken);
        return TypedResults.Ok(vm);
    }

    private static async Task<IResult> SerializeTblFilePath(
        ISender sender,
        [FromQuery] string filePath,
        CancellationToken cancellationToken
    )
    {
        var jsonPayload = await File.ReadAllTextAsync(filePath, cancellationToken);
        var command = JsonSerializer.Deserialize<SerializeTblCommand>(
            jsonPayload,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );

        if (command is null)
            return Results.BadRequest();

        var serializedFile = await sender.Send(command, cancellationToken);
        return Results.File(serializedFile, ContentType.Application.Octet, "PATCH.TBL");
    }

    [ProducesResponseType(
        type: typeof(FileContentHttpResult),
        statusCode: StatusCodes.Status200OK,
        contentType: ContentType.Application.Octet
    )]
    private static async Task<FileContentHttpResult> SerializeTbl(
        ISender sender,
        SerializeTblCommand command,
        CancellationToken cancellationToken
    )
    {
        var serializedFile = await sender.Send(command, cancellationToken);
        return TypedResults.File(serializedFile, ContentType.Application.Octet, "PATCH.TBL");
    }

    private static async Task<Ok<TblVm>> GetTblById(
        ISender sender,
        PatchFileVersion id,
        CancellationToken cancellationToken
    )
    {
        var vm = await sender.Send(new GetTblByIdQuery(id), cancellationToken);
        return TypedResults.Ok(vm);
    }

    private static async Task<Created> ImportTbl(
        ISender sender,
        [FromForm] IFormFileCollection files,
        CancellationToken cancellationToken
    )
    {
        var fileStreams = files.Select(formFile => formFile.OpenReadStream()).ToArray();
        await sender.Send(new ImportTblCommand(fileStreams), cancellationToken);

        foreach (var fileStream in fileStreams)
            await fileStream.DisposeAsync();

        return TypedResults.Created();
    }

    [ProducesResponseType(
        type: typeof(FileContentHttpResult),
        statusCode: StatusCodes.Status200OK,
        contentType: ContentType.Application.Octet
    )]
    private static async Task<FileContentHttpResult> ExportTbl(
        ISender sender,
        ExportTblCommand command,
        CancellationToken cancellationToken
    )
    {
        var fileInfo = await sender.Send(command, cancellationToken);
        return TypedResults.File(
            fileInfo.Data,
            fileInfo.MediaTypeName ?? ContentType.Application.Octet,
            fileInfo.FileName
        );
    }
}
