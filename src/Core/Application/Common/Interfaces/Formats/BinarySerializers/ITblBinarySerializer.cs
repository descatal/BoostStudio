using BoostStudio.Domain.Entities.Exvs.Tbl;
using BoostStudio.Formats;

namespace BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;

public interface ITblBinarySerializer
{
    Task<TblBinaryFormat> DeserializeAsync(Stream data, CancellationToken cancellationToken);

    Task<byte[]> SerializeAsync(TblBinaryFormat data, CancellationToken cancellationToken);
    
    Task<byte[]> SerializeAsync(Tbl data, CancellationToken cancellationToken);
}
