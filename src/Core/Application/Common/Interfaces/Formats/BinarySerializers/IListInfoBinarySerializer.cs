using BoostStudio.Domain.Entities.Exvs.Series;
using BoostStudio.Formats;
using Unit=BoostStudio.Domain.Entities.Exvs.Units.Unit;

namespace BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;

public interface IListInfoBinarySerializer
{
    Task<byte[]> SerializePlayableSeriesAsync(List<Series> data, CancellationToken cancellationToken = default);

    Task<byte[]> SerializePlayableCharactersAsync(List<Unit> data, CancellationToken cancellationToken = default);

    Task<ListInfoBinaryFormat> DeserializeAsync(Stream data, CancellationToken cancellationToken = default);
}
