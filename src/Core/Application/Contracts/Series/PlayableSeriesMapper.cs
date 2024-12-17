using BoostStudio.Application.Common.Models.Options;
using BoostStudio.Domain.Entities.Exvs.Series;
using BoostStudio.Formats;
using Riok.Mapperly.Abstractions;

namespace BoostStudio.Application.Contracts.Series;

[Mapper]
public static partial class PlayableSeriesMapper
{
    public static partial Domain.Entities.Exvs.Series.Series MapToEntity(PlayableSeriesDto dto);

    [MapProperty(nameof(ListInfoBinaryFormat.SeriesInfo.SeriesId), nameof(Domain.Entities.Exvs.Series.Series.Id))]
    public static partial Domain.Entities.Exvs.Series.Series MapToEntity(ListInfoBinaryFormat.SeriesInfo binary);

    public static partial void UpdateEntity(Domain.Entities.Exvs.Series.Series source, Domain.Entities.Exvs.Series.Series target);

    public static partial void UpdateEntityMetadata(SeriesMetadataOption source, Domain.Entities.Exvs.Series.Series target);

    public static partial PlayableSeriesDto MapToDto(Domain.Entities.Exvs.Series.Series entity);

    public static partial IQueryable<PlayableSeriesDto> ProjectToDto(IQueryable<Domain.Entities.Exvs.Series.Series> entity);
}
