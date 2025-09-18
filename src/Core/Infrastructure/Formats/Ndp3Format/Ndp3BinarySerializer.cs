using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Formats;
using Kaitai;

namespace BoostStudio.Infrastructure.Formats.Ndp3Format;

public class Ndp3BinarySerializer : INdp3BinarySerializer
{
    public Task<Ndp3BinaryFormat> DeserializeAsync(
        Stream data,
        CancellationToken cancellationToken = default
    )
    {
        var kaitaiStream = new KaitaiStream(data);
        var deserializedObject = new Ndp3BinaryFormat(kaitaiStream);
        return Task.FromResult(deserializedObject);
    }
}
