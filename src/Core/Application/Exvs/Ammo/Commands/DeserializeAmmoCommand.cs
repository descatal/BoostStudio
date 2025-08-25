using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Ammo;
using AmmoMapper = BoostStudio.Application.Contracts.Ammo.AmmoMapper;

namespace BoostStudio.Application.Exvs.Ammo.Commands;

public record DeserializeAmmoCommand(string SourceFilePath) : IRequest<List<AmmoDto>>;

public class DeserializeTblCommandHandler(
    IFormatBinarySerializer<List<Domain.Entities.Exvs.Ammo.Ammo>> ammoBinarySerializer
) : IRequestHandler<DeserializeAmmoCommand, List<AmmoDto>>
{
    public async ValueTask<List<AmmoDto>> Handle(
        DeserializeAmmoCommand request,
        CancellationToken cancellationToken
    )
    {
        var fileContent = await File.ReadAllBytesAsync(request.SourceFilePath, cancellationToken);

        await using var fileStream = new MemoryStream(fileContent);
        var deserializedAmmo = await ammoBinarySerializer.DeserializeAsync(
            fileStream,
            cancellationToken
        );

        var ammoDto = deserializedAmmo.Select(ammo => AmmoMapper.MapToDto(ammo)).ToList();
        return ammoDto;
    }
}
