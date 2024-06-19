using Riok.Mapperly.Abstractions;
using UnitStat = BoostStudio.Domain.Entities.Unit.UnitStat;

namespace BoostStudio.Application.Contracts.Stats.UnitStats;

[Mapper]
public static partial class UnitStatMapper
{
    public static partial IQueryable<UnitStatDto> ProjectToDto(IQueryable<UnitStat> unitStats);
    
    [MapProperty([nameof(UnitStat.Unit), nameof(UnitStat.Unit.Name)], [nameof(UnitStatDto.NameEnglish)])]
    [MapProperty([nameof(UnitStat.Unit), nameof(UnitStat.Unit.NameChinese)], [nameof(UnitStatDto.NameChinese)])]
    [MapProperty([nameof(UnitStat.Unit), nameof(UnitStat.Unit.NameJapanese)], [nameof(UnitStatDto.NameJapanese)])]
    [MapperIgnoreTarget(nameof(StatDetailsDto.UnitId))]
    private static partial UnitStatDto MapToDto(UnitStat entity);
}
