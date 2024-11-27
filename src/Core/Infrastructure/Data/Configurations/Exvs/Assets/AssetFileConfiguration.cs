using BoostStudio.Domain.Entities.Exvs.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Exvs.Assets;

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

        builder.Property(assetFile => assetFile.FileType)
            .HasConversion<string>();
    }
}
