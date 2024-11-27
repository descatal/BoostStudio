using BoostStudio.Application.Contracts.Stats.UnitStats;
using BoostStudio.Domain.Entities.Exvs.Units;
using Riok.Mapperly.Abstractions;

namespace BoostStudio.Application.Contracts.Stats;

[Mapper]
public static partial class StatMapper
{
    public static partial IQueryable<StatDto> ProjectToDto(IQueryable<Stat> entity);
    
    public static partial List<UnitAmmoSlotDto> MapToDto(List<UnitAmmoSlot> entity);
    
    [MapProperty([nameof(Stat.UnitStat), nameof(Stat.UnitStat.GameUnitId)], [nameof(StatDto.UnitId)])]
    public static partial StatDto MapToDto(Stat entity);
    
    public static partial void MapToEntity(StatDetailsDto source, Stat destination);
    
    public static partial Stat MapToEntity(StatDetailsDto dto);
    
    public static Stat MapToEntity(Guid id, StatDetailsDto sourceDto, Stat? destination = null)
    {
        var entity = destination ?? new Stat();
        if (destination is not null)
            MapToEntity(sourceDto, entity);
        else
            entity = MapToEntity(sourceDto);
        
        entity.Id = id;
        return entity;
    }
}
