using BoostStudio.Application.Contracts.Metadata.Models;
using BoostStudio.Domain.Entities;
using BoostStudio.Domain.Entities.Tbl;
using BoostStudio.Domain.Enums;
using BoostStudio.Formats;

namespace BoostStudio.Application.Common.Interfaces.Formats.TblFormat;

public interface ITblMetadataSerializer
{
    Task<TblBinaryFormat> DeserializeDtoAsync(Stream stream, TblDto data, CancellationToken cancellationToken = default);

    public Task<TblDto> SerializeDtoAsync(TblBinaryFormat data, CancellationToken cancellationToken = default);

    Task<List<PatchFile>> SerializeAsync(
        Tbl tbl,
        TblBinaryFormat data,
        CancellationToken cancellationToken = default);
}
