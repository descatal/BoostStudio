using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Domain.Entities.Unit.Assets;
using BoostStudio.Infrastructure.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Assets;

public class AssetFileConfiguration : IEntityTypeConfiguration<AssetFile>
{
    public void Configure(EntityTypeBuilder<AssetFile> builder)
    {
        builder.Property(entity => entity.Id)
            .HasValueGenerator<UUIDv7Generator>()
            .ValueGeneratedOnAdd();
        
        builder.Property(file => file.Index).ValueGeneratedOnAdd();
        
        builder.HasOne<Unit>(asset => asset.Unit)
            .WithOne()
            .HasForeignKey<AssetFile>(asset => asset.GameUnitId)
            .HasPrincipalKey<Unit>(unit => unit.GameUnitId)
            .IsRequired();
    }
}
