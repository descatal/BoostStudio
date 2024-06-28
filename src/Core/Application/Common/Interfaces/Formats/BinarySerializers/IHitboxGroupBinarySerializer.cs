using BoostStudio.Domain.Entities.Unit.Hitboxes;
using BoostStudio.Formats;

namespace BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;

public interface IHitboxGroupBinarySerializer
{
    Task<byte[]> SerializeAsync(HitboxGroup data, CancellationToken cancellationToken);

    Task<HitboxBinaryFormat> DeserializeAsync(Stream data, CancellationToken cancellationToken);
}
