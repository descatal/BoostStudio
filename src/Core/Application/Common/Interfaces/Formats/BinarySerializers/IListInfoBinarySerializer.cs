using BoostStudio.Domain.Entities.Exvs.Series;
using BoostStudio.Formats;

namespace BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;

public interface IListInfoBinarySerializer
{
    Task<byte[]> SerializeSeriesAsync(List<Series> data, CancellationToken cancellationToken);

    Task<ListInfoBinaryFormat> DeserializeAsync(Stream data, CancellationToken cancellationToken);
}
