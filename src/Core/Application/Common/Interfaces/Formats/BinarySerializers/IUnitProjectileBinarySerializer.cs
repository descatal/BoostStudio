using BoostStudio.Domain.Entities.Exvs.Projectiles;
using BoostStudio.Formats;

namespace BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;

public interface IUnitProjectileBinarySerializer
{
    Task<byte[]> SerializeAsync(UnitProjectile data, CancellationToken cancellationToken);

    Task<ProjectileBinaryFormat> DeserializeAsync(Stream data, CancellationToken cancellationToken);
}
