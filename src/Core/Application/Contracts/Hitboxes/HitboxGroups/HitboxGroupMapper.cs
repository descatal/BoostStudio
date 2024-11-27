using BoostStudio.Domain.Entities.Exvs.Hitboxes;
using UnitEntity=BoostStudio.Domain.Entities.Exvs.Units.Unit;
using Riok.Mapperly.Abstractions;

namespace BoostStudio.Application.Contracts.Hitboxes.HitboxGroups;

[Mapper]
public static partial class HitboxGroupMapper
{
    [MapProperty(nameof(HitboxGroup.Units), nameof(HitboxGroupDto.UnitIds), Use = nameof(MapUnitIds))]
    public static partial IQueryable<HitboxGroupDto> ProjectToDto(IQueryable<HitboxGroup> queryable);
    
    [MapProperty(nameof(HitboxGroup.Units), nameof(HitboxGroupDto.UnitIds), Use = nameof(MapUnitIds))]
    public static partial HitboxGroupDto MapToDto(HitboxGroup entity);
    
    [UserMapping(Default = true)]
    private static uint[] MapUnitIds(ICollection<UnitEntity> entity)
        => entity.Select(x => x.GameUnitId).ToArray();
}
