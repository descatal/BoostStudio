using BoostStudio.Application.Contracts.Stats.UnitStats;
using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Domain.Entities.Unit.Projectiles;
using Riok.Mapperly.Abstractions;

namespace BoostStudio.Application.Contracts.Projectiles.UnitProjectiles;

[Mapper]
public static partial class UnitProjectileMapper
{
    public static partial IQueryable<UnitProjectileDto> ProjectToDto(IQueryable<UnitProjectile> unitStats);
    
    [MapProperty([nameof(UnitProjectile.Unit), nameof(UnitProjectile.Unit.GameUnitId)], [nameof(UnitProjectileDto.UnitId)])]
    private static partial UnitProjectileDto MapToDto(UnitProjectile entity);
}
