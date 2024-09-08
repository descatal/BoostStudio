using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Domain.Entities.Unit.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Assets;

public class AssetFileConfiguration : IEntityTypeConfiguration<AssetFile>
{
    public void Configure(EntityTypeBuilder<AssetFile> builder)
    {
        builder.Ignore(assetFile => assetFile.Id);
        builder.HasKey(assetFile => assetFile.Hash);
        
        builder.Property(assetFile => assetFile.Order)
            .ValueGeneratedOnAdd();
        
        builder.HasOne<Unit>(assetFile => assetFile.Unit)
            .WithMany(unit => unit.AssetFiles)
            .HasForeignKey(assetFile => assetFile.GameUnitId)
            .HasPrincipalKey(unit => unit.GameUnitId)
            .IsRequired(false);
    }
}
