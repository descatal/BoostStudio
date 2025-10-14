using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Formats;
using Kaitai;

namespace BoostStudio.Infrastructure.Formats.VbnFormat;

public class VbnBinarySerializer : IVbnBinarySerializer
{
    public Task<VbnBinaryFormat> DeserializeAsync(
        Stream data,
        CancellationToken cancellationToken = default
    )
    {
        var kaitaiStream = new KaitaiStream(data);
        var deserializedObject = new VbnBinaryFormat(kaitaiStream);
        return Task.FromResult(deserializedObject);
    }
}
