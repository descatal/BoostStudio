using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Ammo;
using AmmoMapper=BoostStudio.Application.Contracts.Ammo.AmmoMapper;

namespace BoostStudio.Application.Formats.AmmoFormat.Commands;

public record DeserializeAmmoCommand(string SourceFilePath) : IRequest<AmmoView>;

public class DeserializeTblCommandHandler(
    IFormatBinarySerializer<List<Ammo>> ammoBinarySerializer
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
