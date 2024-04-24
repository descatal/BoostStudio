using BoostStudio.Domain.Entities.Unit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations;

public class AmmoConfiguration : IEntityTypeConfiguration<Ammo>
{
    public void Configure(EntityTypeBuilder<Ammo> builder)
    {
        builder.Ignore(t => t.Id);
        builder.HasKey(t => t.Hash);
    }
}
