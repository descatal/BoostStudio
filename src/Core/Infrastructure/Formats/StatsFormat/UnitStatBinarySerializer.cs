using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Formats;
using Kaitai;

namespace BoostStudio.Infrastructure.Formats.StatsFormat;

public class UnitStatBinarySerializer : IFormatBinarySerializer<StatsBinaryFormat>
{
    public async Task<byte[]> SerializeAsync(StatsBinaryFormat data, CancellationToken cancellationToken)
    {
        return [];
    }

    public async Task<StatsBinaryFormat> DeserializeAsync(Stream data, CancellationToken cancellationToken)
    {
        var kaitaiStream = new KaitaiStream(data);
        var deserializedObject = new StatsBinaryFormat(kaitaiStream);
        return deserializedObject;
    }
}
