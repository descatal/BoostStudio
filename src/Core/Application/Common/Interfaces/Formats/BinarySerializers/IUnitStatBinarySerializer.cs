using BoostStudio.Domain.Entities.Exvs.Stats;
using BoostStudio.Formats;

namespace BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;

public interface IUnitStatBinarySerializer
{
    Task<byte[]> SerializeAsync(UnitStat data, CancellationToken cancellationToken);

    Task<StatsBinaryFormat> DeserializeAsync(Stream data, CancellationToken cancellationToken);
}
