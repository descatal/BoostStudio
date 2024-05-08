using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Formats;

namespace BoostStudio.Application.Common.Interfaces.Formats;

public interface IUnitStatBinarySerializer
{
    Task<byte[]> SerializeAsync(UnitStat data, CancellationToken cancellationToken);

    Task<StatsBinaryFormat> DeserializeAsync(Stream data, CancellationToken cancellationToken);
}
