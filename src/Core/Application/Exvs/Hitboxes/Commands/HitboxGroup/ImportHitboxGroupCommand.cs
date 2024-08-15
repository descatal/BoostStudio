using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Formats;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HitboxGroupEntity=BoostStudio.Domain.Entities.Unit.Hitboxes.HitboxGroup;
using HitboxEntity=BoostStudio.Domain.Entities.Unit.Hitboxes.Hitbox;

namespace BoostStudio.Application.Exvs.Hitboxes.Commands.HitboxGroup;

public record ImportHitboxGroupCommand(ImportHitboxGroupDetails[] Data) : IRequest;

public record ImportHitboxGroupDetails(Stream File, uint[]? UnitIds = null);

public class ImportHitboxGroupCommandHandler(
    IHitboxGroupBinarySerializer hitboxGroupBinarySerializer,
    IApplicationDbContext applicationDbContext,
    ILogger<ImportHitboxGroupCommandHandler> logger
) : IRequestHandler<ImportHitboxGroupCommand>
{
    public async ValueTask<Unit> Handle(ImportHitboxGroupCommand groupCommand, CancellationToken cancellationToken)
    {
        foreach ((Stream binaryStream, uint[]? ids) in groupCommand.Data)
        {
            var binaryFormat = await hitboxGroupBinarySerializer.DeserializeAsync(binaryStream, cancellationToken);
            
            var entity = await applicationDbContext.HitboxGroups
                .Include(group => group.Hitboxes)
                .FirstOrDefaultAsync(group => group.Hash == binaryFormat.FileMagic, cancellationToken);
            
            if (entity is null)
            {
                entity = new HitboxGroupEntity
                {
                    Hash = binaryFormat.FileMagic,
                };
                await applicationDbContext.HitboxGroups.AddAsync(entity, cancellationToken);
            }

            MapToEntity(entity, binaryFormat);
            
            var unitIds = ids ?? [];
            var units = await applicationDbContext.Units
                .Where(unit => unitIds.Contains(unit.GameUnitId))
                .ToListAsync(cancellationToken);
            
            entity.Units = units;
        }
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return default;
    }

    private static void MapToEntity(
        HitboxGroupEntity hitboxGroup,
        HitboxBinaryFormat binaryData)
    {
        // update the file magic info
        hitboxGroup.Hash = binaryData.FileMagic;

        var newEntityHashes = new List<uint>();
        foreach (var hitboxBody in binaryData.Hitbox)
        {
            var hitboxEntity = hitboxGroup.Hitboxes
                .FirstOrDefault(hitbox => hitbox.Hash == hitboxBody.Hash);

            if (hitboxEntity is null)
            {
                hitboxEntity ??= new HitboxEntity();
                hitboxEntity.Hash = hitboxBody.Hash;
                hitboxGroup.Hitboxes.Add(hitboxEntity);
            }

            // Use reflection to map the HitboxProperty enum to concrete Hitbox
            foreach (var hitboxProperty in hitboxBody.Properties)
            {
                var propertyName = hitboxProperty.Name;
                var propertyInfo = typeof(HitboxEntity).GetProperty(propertyName.ToString());
                if (propertyInfo is null)
                    throw new Exception("Property mismatch between binary's hitbox body and hitbox entity!");

                var castedPropertyValue = Convert.ChangeType(hitboxProperty.Value, propertyInfo.PropertyType);
                propertyInfo.SetValue(hitboxEntity, castedPropertyValue);
            }

            newEntityHashes.Add(hitboxEntity.Hash);
        }

        // remove items not found in the file
        hitboxGroup.Hitboxes
            .Where(hitbox => !newEntityHashes.Contains(hitbox.Hash))
            .ToList()
            .ForEach(hitbox => hitboxGroup.Hitboxes.Remove(hitbox));
    }
}
