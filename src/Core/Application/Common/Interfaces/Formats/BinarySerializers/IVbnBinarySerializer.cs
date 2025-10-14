using BoostStudio.Formats;

namespace BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;

public interface IVbnBinarySerializer
{
    Task<VbnBinaryFormat> DeserializeAsync(
        Stream data,
        CancellationToken cancellationToken = default
    );
}
