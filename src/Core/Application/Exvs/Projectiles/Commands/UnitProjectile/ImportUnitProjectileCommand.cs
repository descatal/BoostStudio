using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Contracts.Hitboxes;
using BoostStudio.Application.Contracts.Projectiles;
using BoostStudio.Formats;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnitProjectileEntity=BoostStudio.Domain.Entities.Unit.Projectiles.UnitProjectile;
using ProjectileEntity=BoostStudio.Domain.Entities.Unit.Projectiles.Projectile;

namespace BoostStudio.Application.Exvs.Projectiles.Commands.UnitProjectile;

public record ImportUnitProjectileCommand(Stream[] Files) : IRequest;

public class ImportUnitProjectileCommandHandler(
    IUnitProjectileBinarySerializer binarySerializer,
    IApplicationDbContext applicationDbContext,
    ILogger<ImportUnitProjectileCommandHandler> logger
) : IRequestHandler<ImportUnitProjectileCommand>
{
    public async Task Handle(ImportUnitProjectileCommand command, CancellationToken cancellationToken)
    {
        foreach (var fileStream in command.Files)
        {
            var statsBinaryFormat = await binarySerializer.DeserializeAsync(fileStream, cancellationToken);

            var unit = await applicationDbContext.Units
                .FirstOrDefaultAsync(unitProjectile => unitProjectile.GameUnitId == statsBinaryFormat.UnitId, cancellationToken);

            if (unit is null)
                continue;

            var entity = await applicationDbContext.UnitProjectiles
                .Include(unitProjectile => unitProjectile.Projectiles)
                .FirstOrDefaultAsync(unitProjectile => unitProjectile.GameUnitId == statsBinaryFormat.UnitId, cancellationToken);

            if (entity is null)
            {
                entity = new UnitProjectileEntity
                {
                    GameUnitId = statsBinaryFormat.UnitId
                };
                await applicationDbContext.UnitProjectiles.AddAsync(entity, cancellationToken);
            }

            MapToEntity(entity, statsBinaryFormat);
            
            
            
            var allHitboxes = applicationDbContext.Hitboxes.Select(x => x.Hash).ToList();
            var allNewHitboxes = entity.Projectiles.Select(x => x.HitboxHash).ToList();

            foreach (var asd in allNewHitboxes)
            {
                if (asd.HasValue && !allHitboxes.Contains(asd.Value))
                {
                    
                }
            }
            
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    private static void MapToEntity(
        UnitProjectileEntity unitProjectile,
        ProjectileBinaryFormat binaryData)
    {
        // update the file magic info
        unitProjectile.FileSignature = binaryData.FileMagic;

        var newEntityHashes = new List<uint>();
        foreach (var projectileBody in binaryData.Projectile)
        {
            var projectileEntity = unitProjectile.Projectiles
                .FirstOrDefault(projectile => projectile.Hash == projectileBody.Hash);

            if (projectileEntity is null)
            {
                projectileEntity ??= new ProjectileEntity();
                unitProjectile.Projectiles.Add(projectileEntity);
            }

            ProjectileMapper.MapToEntity(projectileBody.Hash, projectileBody.ProjectileProperties, projectileEntity);
            newEntityHashes.Add(projectileEntity.Hash);
        }

        // remove items not found in the file
        unitProjectile.Projectiles
            .Where(projectile => !newEntityHashes.Contains(projectile.Hash))
            .ToList()
            .ForEach(projectile => unitProjectile.Projectiles.Remove(projectile));
    }
}
