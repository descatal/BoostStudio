using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Formats;
using Kaitai;

namespace BoostStudio.Infrastructure.Formats.NudFormat;

public class NudBinarySerializer : INudBinarySerializer
{
    public Task<NudBinaryFormat> DeserializeAsync(
        Stream data,
        CancellationToken cancellationToken = default
    )
    {
        var kaitaiStream = new KaitaiStream(data);
        var deserializedObject = new NudBinaryFormat(kaitaiStream);
        return Task.FromResult(deserializedObject);
    }
}
