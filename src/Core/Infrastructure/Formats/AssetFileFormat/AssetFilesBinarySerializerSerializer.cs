using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Formats;
using Kaitai;

namespace BoostStudio.Infrastructure.Formats.AssetFileFormat;

public class AssetFilesBinarySerializerSerializer : IAssetFilesBinarySerializer
{
    public Task<AssetFilesBinaryFormat> DeserializeAsync(Stream data, CancellationToken cancellationToken)
    {
        var kaitaiStream = new KaitaiStream(data);
        var deserializedObject = new AssetFilesBinaryFormat(kaitaiStream);
        return Task.FromResult(deserializedObject);
    }
}
