using BoostStudio.Application.Contracts.Metadata.Models;
using BoostStudio.Domain.Entities;
using BoostStudio.Domain.Entities.Tbl;
using BoostStudio.Formats;

namespace BoostStudio.Application.Common.Interfaces.Formats.TblFormat;

public interface ITblMetadataSerializer
{
    Task<TblBinaryFormat> DeserializeAsync(Stream stream, TblDto data, CancellationToken cancellationToken);

    public Task<TblDto> SerializeDtoAsync(TblBinaryFormat data, CancellationToken cancellationToken);
    
    Task<List<PatchFile>> SerializeAsync(TblBinaryFormat data, CancellationToken cancellationToken);
}
