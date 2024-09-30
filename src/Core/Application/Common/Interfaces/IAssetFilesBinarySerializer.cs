using BoostStudio.Formats;

namespace BoostStudio.Application.Common.Interfaces;

public interface IAssetFilesBinarySerializer
{
    Task<AssetFilesBinaryFormat> DeserializeAsync(Stream data, CancellationToken cancellationToken);
}
