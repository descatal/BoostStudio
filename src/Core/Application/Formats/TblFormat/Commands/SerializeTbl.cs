﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.TblFormat;
using BoostStudio.Application.Contracts.Metadata.Models;

namespace BoostStudio.Application.Formats.TblFormat.Commands;

public record SerializeTbl(TblMetadata TblMetadata) : IRequest<byte[]>
{
    [Required]
    public TblMetadata TblMetadata { get; } = TblMetadata;
}

public class SerializeTblHandler(
    IFormatSerializer<Tbl> tblSerializer,
    ITblMetadataSerializer tblMetadataSerializer) : IRequestHandler<SerializeTbl, byte[]>
{
    public async Task<byte[]> Handle(SerializeTbl request, CancellationToken cancellationToken)
    {
        await using var tblStream = new MemoryStream();
        var tbl = await tblMetadataSerializer.DeserializeAsync(
            stream: tblStream,
            data: request.TblMetadata,
            cancellationToken: cancellationToken);

        var packedTbl = await tblSerializer.SerializeAsync(
            data: tbl,
            cancellationToken: cancellationToken);

        return packedTbl;
    }
}
