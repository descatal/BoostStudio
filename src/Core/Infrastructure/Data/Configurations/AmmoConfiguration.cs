using BoostStudio.Domain.Entities.Unit.Ammo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations;

public class AmmoConfiguration : IEntityTypeConfiguration<Ammo>
{
    public void Configure(EntityTypeBuilder<Ammo> builder)
    {
        builder.HasAlternateKey(ammo => ammo.Hash);
    }
}
