using BoostStudio.Domain.Entities.Exvs.Assets;
using BoostStudio.Domain.Entities.Exvs.Units;
using BoostStudio.Domain.Entities.Exvs.Units.Characters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Exvs.Units;

public class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.Ignore(entity => entity.Id);
        builder.HasKey(entity => entity.GameUnitId);

        builder.OwnsOne(unit => unit.PlayableCharacter, od =>
        {
            od.WithOwner(entity => entity.Unit);

            od.Property(entity => entity.UnitIndex)
                .ValueGeneratedOnAdd();

            od.Property(entity => entity.UnitSelectOrderInSeries)
                .ValueGeneratedOnAdd();

            od.Property(entity => entity.FigurineSpriteIndex)
                .ValueGeneratedOnAdd();

            od.Property(entity => entity.ArcadeSmallSpriteIndex)
                .ValueGeneratedOnAdd();

            od.Property(entity => entity.ArcadeUnitNameSpriteIndex)
                .ValueGeneratedOnAdd();

            od.HasOne<AssetFile>(entity => entity.ArcadeSelectionCostume1SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.ArcadeSelectionCostume1SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.ArcadeSelectionCostume2SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.ArcadeSelectionCostume2SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.ArcadeSelectionCostume3SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.ArcadeSelectionCostume3SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.LoadingLeftCostume1SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.LoadingLeftCostume1SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.LoadingLeftCostume2SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.LoadingLeftCostume2SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.LoadingLeftCostume3SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.LoadingLeftCostume3SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.LoadingRightCostume1SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.LoadingRightCostume1SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.LoadingRightCostume2SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.LoadingRightCostume2SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.LoadingRightCostume3SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.LoadingRightCostume3SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.GenericSelectionCostume1SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.GenericSelectionCostume1SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.GenericSelectionCostume2SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.GenericSelectionCostume2SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.GenericSelectionCostume3SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.GenericSelectionCostume3SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.LoadingTargetUnitSpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.LoadingTargetUnitSpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.LoadingTargetPilotCostume1SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.LoadingTargetPilotCostume1SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.LoadingTargetPilotCostume2SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.LoadingTargetPilotCostume2SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.LoadingTargetPilotCostume3SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.LoadingTargetPilotCostume3SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.InGameSortieAndAwakeningPilotCostume1SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.InGameSortieAndAwakeningPilotCostume1SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.InGameSortieAndAwakeningPilotCostume2SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.InGameSortieAndAwakeningPilotCostume2SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.InGameSortieAndAwakeningPilotCostume3SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.InGameSortieAndAwakeningPilotCostume3SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.SpriteFramesAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.SpriteFramesAssetHash);

            od.HasOne<AssetFile>(entity => entity.ResultSmallUnitSprite)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.ResultSmallUnitSpriteHash);

            od.HasOne<AssetFile>(entity => entity.FigurineSpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.FigurineSpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.LoadingTargetUnitSmallSpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.LoadingTargetUnitSmallSpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.CatalogStorePilotCostume2SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.CatalogStorePilotCostume2SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.CatalogStorePilotCostume3SpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.CatalogStorePilotCostume3SpriteAssetHash);

            od.HasOne<AssetFile>(entity => entity.FigurineSpriteAsset)
                .WithOne()
                .HasForeignKey<PlayableCharacter>(entity => entity.FigurineSpriteAssetHash);
        });
    }
}
