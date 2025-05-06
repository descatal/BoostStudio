using BoostStudio.Domain.Entities.Exvs.Assets;
using Riok.Mapperly.Abstractions;
using Unit = BoostStudio.Domain.Entities.Exvs.Units.Unit;

namespace BoostStudio.Application.Contracts.Assets;

[Mapper]
public static partial class AssetFileMapper
{
    public static partial IQueryable<AssetFileVm> ProjectToVm(IQueryable<AssetFile> entity);

    [MapProperty(
        nameof(AssetFile.Units),
        nameof(AssetFileVm.UnitIds),
        Use = nameof(MapUnitToUnitIds)
    )]
    public static partial AssetFileVm ToVm(AssetFile entity);

    public static partial AssetFile ToEntity(AssetFileDto dto);

    public static partial void Update(AssetFileDto source, AssetFile target);

    private static uint[] MapUnitToUnitIds(ICollection<Unit> units) =>
        units.Select(unit => unit.GameUnitId).ToArray();
}
