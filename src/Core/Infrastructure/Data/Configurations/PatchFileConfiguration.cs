using BoostStudio.Domain.Entities;
using BoostStudio.Domain.Entities.Tbl;
using BoostStudio.Infrastructure.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations;

public class PatchFileConfiguration : IEntityTypeConfiguration<PatchFile>
{
    public void Configure(EntityTypeBuilder<PatchFile> builder)
    {
        builder.HasKey(file => file.Id);
        
        builder.Property(entity => entity.Id)
            .HasValueGenerator<UUIDv7Generator>()
            .ValueGeneratedOnAdd();
    }
}
