using BoostStudio.Domain.Entities.Unit.Hitboxes;
using Riok.Mapperly.Abstractions;

namespace BoostStudio.Application.Contracts.Hitboxes.HitboxGroups;

[Mapper]
public static partial class HitboxGroupMapper
{
    public static partial IQueryable<HitboxGroupDto> ProjectToDto(IQueryable<HitboxGroup> queryable);
    
    [MapProperty([nameof(HitboxGroup.Unit), nameof(HitboxGroup.Unit.GameUnitId)], [nameof(HitboxGroupDto.UnitId)])]
    private static partial HitboxGroupDto MapToDto(HitboxGroup entity);
}
