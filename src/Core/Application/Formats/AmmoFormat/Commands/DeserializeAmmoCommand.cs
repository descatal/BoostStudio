﻿using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Exvs.Ammo.Models;
using AmmoMapper=BoostStudio.Application.Exvs.Ammo.Mappers.AmmoMapper;

namespace BoostStudio.Application.Formats.AmmoFormat.Commands;

public record DeserializeAmmoCommand(string SourceFilePath) : IRequest<AmmoView>;

public class DeserializeTblCommandHandler(
    IFormatBinarySerializer<List<Domain.Entities.Unit.Ammo>> ammoBinarySerializer
) : IRequestHandler<DeserializeAmmoCommand, AmmoView>
{
    public async Task<AmmoView> Handle(DeserializeAmmoCommand request, CancellationToken cancellationToken)
    {
        var fileContent = await File.ReadAllBytesAsync(request.SourceFilePath, cancellationToken);
        
        await using var fileStream = new MemoryStream(fileContent);
        var deserializedAmmo = await ammoBinarySerializer.DeserializeAsync(fileStream, cancellationToken);

        var mapper = new AmmoMapper();
        var ammoDto = deserializedAmmo.Select(ammo => mapper.AmmoToAmmoDto(ammo)).ToList();
        return new AmmoView(ammoDto);
    }
}