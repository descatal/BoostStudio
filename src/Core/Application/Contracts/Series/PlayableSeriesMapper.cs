using BoostStudio.Application.Common.Models.Options;
using BoostStudio.Application.Contracts.Units;
using BoostStudio.Domain.Entities.Exvs.Series;
using Riok.Mapperly.Abstractions;
using SeriesEntity = BoostStudio.Domain.Entities.Exvs.Series.Series;
using SeriesInfoFormat = BoostStudio.Formats.ListInfoBinaryFormat.SeriesInfo;
using Unit=BoostStudio.Domain.Entities.Exvs.Units.Unit;

namespace BoostStudio.Application.Contracts.Series;

[Mapper]
[UseStaticMapper(typeof(UnitMapper2))]
[UseStaticMapper(typeof(PlayableSeriesMapper))]
public static partial class SeriesMapper
{
    public static partial SeriesEntity MapToEntity(SeriesDto dto);

    [MapperIgnoreSource(nameof(SeriesEntity.DomainEvents))]
    public static partial SeriesDto MapToDto(SeriesEntity entity);

    [MapperIgnoreSource(nameof(SeriesEntity.PlayableSeries))]
    [MapperIgnoreSource(nameof(SeriesEntity.DomainEvents))]
    public static partial SeriesVm MapToVm(SeriesEntity entity);

    public static SeriesUnitsVm MapToSeriesUnitsVm(IGrouping<SeriesEntity?, Unit> grouping)
    {
        var seriesVm = MapToSeriesUnitsVm(grouping.Key!);
        var units = grouping.ToList();
        seriesVm.Units = UnitMapper2.MapToVm(units);
        return seriesVm;
    }

    [MapperIgnoreTarget(nameof(SeriesUnitsVm.Units))]
    [MapperIgnoreSource(nameof(SeriesEntity.PlayableSeries))]
    [MapperIgnoreSource(nameof(SeriesEntity.DomainEvents))]
    public static partial SeriesUnitsVm MapToSeriesUnitsVm(SeriesEntity entity);

    public static partial IQueryable<SeriesVm> ProjectToVm(IQueryable<SeriesEntity> entity);

    public static partial IQueryable<SeriesDto> ProjectToDto(IQueryable<SeriesEntity> entity);

    [MapperIgnoreTarget(nameof(SeriesEntity.PlayableSeries))]
    public static partial void UpdateEntityMetadata(SeriesMetadataOption source, SeriesEntity target);

    public static void UpdateEntityDetailsIfNull(SeriesMetadataOption source, SeriesEntity target)
    {
        if (string.IsNullOrWhiteSpace(target.NameEnglish))
            target.NameEnglish = source.NameEnglish;

        if (string.IsNullOrWhiteSpace(target.NameChinese))
            target.NameChinese = source.NameChinese;

        if (string.IsNullOrWhiteSpace(target.NameJapanese))
            target.NameJapanese = source.NameJapanese;

        if (string.IsNullOrWhiteSpace(target.SlugName))
            target.SlugName = source.SlugName;
    }
}

[Mapper]
public static partial class PlayableSeriesMapper
{
    [MapperIgnoreSource(nameof(SeriesInfoFormat.M_Root))]
    [MapperIgnoreSource(nameof(SeriesInfoFormat.M_Parent))]
    [MapperIgnoreSource(nameof(SeriesInfoFormat.M_Io))]
    [MapperIgnoreSource(nameof(SeriesInfoFormat.ReleaseString))]
    [MapperIgnoreSource(nameof(SeriesInfoFormat.ReleaseStringOffset))]
    [MapperIgnoreTarget(nameof(PlayableSeries.MovieAsset))]
    [MapperIgnoreTarget(nameof(PlayableSeries.Series))]
    public static partial PlayableSeries MapToEntity(SeriesInfoFormat binary);

    [MapperIgnoreTarget(nameof(PlayableSeries.SeriesId))]
    [MapperIgnoreTarget(nameof(PlayableSeries.MovieAsset))]
    [MapperIgnoreTarget(nameof(PlayableSeries.Series))]
    public static partial PlayableSeries MapToEntity(PlayableSeriesDetailsDto detailsDto);

    public static PlayableSeries MapToEntity(PlayableSeriesDto dto)
    {
        var mapped = MapToEntity((PlayableSeriesDetailsDto)dto);
        mapped.SeriesId = dto.SeriesId;
        return mapped;
    }

    [MapperIgnoreSource(nameof(PlayableSeries.MovieAsset))]
    [MapperIgnoreSource(nameof(PlayableSeries.Series))]
    public static partial PlayableSeriesDto MapToDto(PlayableSeries entity);

    [MapperIgnoreSource(nameof(PlayableSeries.SeriesId))]
    [MapperIgnoreSource(nameof(PlayableSeries.MovieAsset))]
    [MapperIgnoreSource(nameof(PlayableSeries.Series))]
    public static partial PlayableSeriesDetailsDto MapToDetailsDto(PlayableSeries entity);

    public static partial IQueryable<PlayableSeriesDto> ProjectToDto(IQueryable<PlayableSeries> entity);
}
