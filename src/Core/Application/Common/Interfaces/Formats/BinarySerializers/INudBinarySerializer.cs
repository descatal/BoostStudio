using BoostStudio.Formats;

namespace BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;

public interface INudBinarySerializer
{
    Task<NudBinaryFormat> DeserializeAsync(
        Stream data,
        CancellationToken cancellationToken = default
    );
}
