using BoostStudio.Application.Contracts.Metadata.Models;

namespace BoostStudio.Application.Common.Interfaces.Formats.TblFormat;

public interface ITblMetadataSerializer
{
    Task<Tbl> DeserializeAsync(Stream stream, TblMetadata data, CancellationToken cancellationToken);

    Task<TblMetadata> SerializeAsync(Tbl data, CancellationToken cancellationToken);
}
