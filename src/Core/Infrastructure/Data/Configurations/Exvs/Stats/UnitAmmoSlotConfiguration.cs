﻿using BoostStudio.Domain.Entities.Exvs.Units;
using BoostStudio.Infrastructure.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoostStudio.Infrastructure.Data.Configurations.Exvs.Stats;

public class UnitAmmoSlotConfiguration : IEntityTypeConfiguration<UnitAmmoSlot>
{
    public void Configure(EntityTypeBuilder<UnitAmmoSlot> builder)
    {
        // A unit can have multiple number of StatSets, with each one representing a new "mode" of the unit
        builder.HasOne(unitAmmoSlot => unitAmmoSlot.UnitStat)
            .WithMany(unitStat => unitStat.AmmoSlots)
            .HasForeignKey(unitAmmoSlot => unitAmmoSlot.UnitStatId)
            .IsRequired();
        
        // Ammo table itself should not know the context that it is related to, this allow Ammo table to be singularly serialized
        builder.HasOne(unitAmmoSlot => unitAmmoSlot.Ammo)
            .WithMany()
            .HasForeignKey(unitAmmoSlot => unitAmmoSlot.AmmoHash)
            .HasPrincipalKey(ammo => ammo.Hash)
            .IsRequired();
        
        builder.Property(entity => entity.Id)
            .HasValueGenerator<UuiDv7Generator>()
            .ValueGeneratedOnAdd();
    }
}
