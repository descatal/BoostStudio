using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Formats;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoostStudio.Application.Exvs.Stats.Commands.UnitStat;

public record ImportUnitStatCommand(Stream[] Files) : IRequest;

public class ImportUnitStatCommandHandler(
    IUnitStatBinarySerializer statBinarySerializer,
    IApplicationDbContext applicationDbContext,
    ILogger<ImportUnitStatCommandHandler> logger
) : IRequestHandler<ImportUnitStatCommand>
{
    public async Task Handle(ImportUnitStatCommand command, CancellationToken cancellationToken)
    {
        foreach (var fileStream in command.Files)
        {
            var statsBinaryFormat = await statBinarySerializer.DeserializeAsync(fileStream, cancellationToken);

            var unit = await applicationDbContext.Units
                .FirstOrDefaultAsync(unitStat =>unitStat.GameUnitId == statsBinaryFormat.UnitId, cancellationToken);
            
            if (unit is null)
                continue;
            
            var entity = await applicationDbContext.UnitStats
                .Include(unitStat => unitStat.AmmoSlots)
                .Include(unitStat => unitStat.Stats)
                .FirstOrDefaultAsync(unitStat => unitStat.GameUnitId == statsBinaryFormat.UnitId, cancellationToken);
        
            var isExist = entity is not null;
            
            entity ??= new Domain.Entities.Unit.UnitStat
            {
                GameUnitId = statsBinaryFormat.UnitId
            };

            await MapToUnitStat(entity, statsBinaryFormat, cancellationToken);
            
            if (!isExist)
                await applicationDbContext.UnitStats.AddAsync(entity, cancellationToken);
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    // Manually map the binary format into entity
    private async Task MapToUnitStat(
        Domain.Entities.Unit.UnitStat entity, 
        StatsBinaryFormat statsBinaryFormat, 
        CancellationToken cancellationToken = default)
    {
        // Reset the state of the entity, if it existed in the first place
        entity.Ammo = [];
        applicationDbContext.Stats.RemoveRange(entity.Stats);
        
        // Optionally retain the file magic info
        entity.FileSignature = statsBinaryFormat.Magic;
        
        // Ammo
        var ammoHashes = statsBinaryFormat.AmmoHashes.Hashes;
        if (ammoHashes is not null)
        {
            var queriedAmmo = await applicationDbContext.Ammo
                .Where(ammo => ammoHashes.Contains(ammo.Hash))
                .ToListAsync(cancellationToken);

            // Maintaining the order is important
            var ammoHashOrder = statsBinaryFormat.AmmoHashes.Hashes.ToList();
            queriedAmmo.ForEach(ammo =>
            {
                ammo.Order = ammoHashOrder.IndexOf(ammo.Hash);
            });
            entity.Ammo = queriedAmmo;
        }
        
        // Ammo Slots
        // A special case for the 5th slot, since it is a thing we forced onto the format, it defaults to 0
        // 5th ammo slot can never select anything that's on the 0th slot, which is an acceptable tradeoff
        if (statsBinaryFormat.AmmoSlot5.SlotIndex == 0)
            statsBinaryFormat.AmmoSlot5.SlotIndex = -1;
        
        List<StatsBinaryFormat.AmmoInfoBody> ammoInfoBody =
        [
            statsBinaryFormat.AmmoSlot1,
            statsBinaryFormat.AmmoSlot2,
            statsBinaryFormat.AmmoSlot3,
            statsBinaryFormat.AmmoSlot4,
            statsBinaryFormat.AmmoSlot5
        ];

        var slotIndex = 0;
        var originalAmmoSlot = entity.AmmoSlots.ToList();
        // Clear the list, this will remove any straggling entry that isn't in the new list
        entity.AmmoSlots = [];
        foreach (var ammoInfo in ammoInfoBody)
        {
            // Ignore if the slot is -1, the slot is empty
            if (ammoInfo.SlotIndex == -1)
                continue;
            
            var ammo = entity.Ammo
                .FirstOrDefault(ammo => ammo.Hash == ammoInfo.AmmoHash);
            
            if (ammo is null) 
                continue;
            
            // Upsert
            var existingSlot = originalAmmoSlot
                .FirstOrDefault(unitAmmoSlot => unitAmmoSlot.AmmoHash == ammo.Hash);
            
            existingSlot ??= new UnitAmmoSlot();
            existingSlot.SlotOrder = slotIndex;
            existingSlot.Ammo = ammo;
            
            entity.AmmoSlots.Add(existingSlot);
            slotIndex++;
        }
        
        // StatSets
        var stats = statsBinaryFormat.Sets.Select(setBody =>
        {
            // Use reflection to map the Stats enum to concrete Stat
            var stat = new Domain.Entities.Unit.Stats.Stat
            {
                Order = setBody.SetIndex
            };
            setBody.Stats.ForEach(statsBody =>
            {
                var type = statsBody.PropertyType switch
                {
                    StatsBinaryFormat.PropertyTypeEnum.Float => typeof(float),
                    StatsBinaryFormat.PropertyTypeEnum.Integer => typeof(int),
                    _ => typeof(int)
                };

                var propertyName = statsBody.PropertyName;
                var propertyInfo = typeof(Domain.Entities.Unit.Stats.Stat).GetProperty(propertyName.ToString());
                if (propertyInfo is null || propertyInfo.PropertyType != type)
                    throw new Exception("Property type mismatch between defined Stat and StatsBinaryFormat!");

                var castedPropertyValue = Convert.ChangeType(statsBody.PropertyValue, type);
                propertyInfo.SetValue(stat, castedPropertyValue);
            });

            return stat;
        }).ToList();

        entity.Stats = stats;
    }
}
