using BoostStudio.Application.Contracts.Units;
using BoostStudio.Domain.Entities.Unit;
using Riok.Mapperly.Abstractions;

namespace BoostStudio.Application.Contracts.Mappers;

[Mapper]
public partial class UnitStatsMapper
{
    public static partial IQueryable<UnitStatDto> ProjectToDto(IQueryable<UnitStat> unitStats);

    [MapProperty([nameof(UnitStat.Unit), nameof(UnitStat.Unit.Name)], [nameof(UnitStatDto.NameEnglish)])]
    [MapProperty([nameof(UnitStat.Unit), nameof(UnitStat.Unit.NameChinese)], [nameof(UnitStatDto.NameChinese)])]
    [MapProperty([nameof(UnitStat.Unit), nameof(UnitStat.Unit.NameJapanese)], [nameof(UnitStatDto.NameJapanese)])]
    private static partial UnitStatDto Map(UnitStat unitStats);
}
