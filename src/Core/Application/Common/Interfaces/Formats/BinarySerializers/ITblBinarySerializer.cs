using BoostStudio.Formats;

namespace BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;

public interface ITblBinarySerializer
{
    Task<TblBinaryFormat> DeserializeAsync(Stream data, bool useSubfolderFlag, CancellationToken cancellationToken);

    Task<byte[]> SerializeAsync(TblBinaryFormat data, CancellationToken cancellationToken);
}
