using BoostStudio.Formats;

namespace BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;

public interface INdp3BinarySerializer
{
    Task<Ndp3BinaryFormat> DeserializeAsync(
        Stream data,
        CancellationToken cancellationToken = default
    );
}
