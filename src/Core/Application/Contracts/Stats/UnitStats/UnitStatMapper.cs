using Riok.Mapperly.Abstractions;
using UnitStat = BoostStudio.Domain.Entities.Unit.UnitStat;

namespace BoostStudio.Application.Contracts.Stats.UnitStats;

[Mapper]
public static partial class UnitStatMapper
{
    public static partial IQueryable<UnitStatDto> ProjectToDto(IQueryable<UnitStat> unitStats);
    
    [MapProperty([nameof(UnitStat.GameUnitId)], [nameof(UnitStatDto.UnitId)])]
    private static partial UnitStatDto MapToDto(UnitStat entity);
}
