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

        builder.HasMany(assetFile => assetFile.Units)
            .WithMany(unit => unit.AssetFiles);
    }
}
