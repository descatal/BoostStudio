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
    public async ValueTask<Unit> Handle(ImportUnitProjectileCommand command, CancellationToken cancellationToken)
    {
        foreach (var fileStream in command.Files)
        {
            var binaryData = await binarySerializer.DeserializeAsync(fileStream, cancellationToken);

            var unit = await applicationDbContext.Units
                .FirstOrDefaultAsync(unitProjectile => unitProjectile.GameUnitId == binaryData.UnitId, cancellationToken);

            if (unit is null)
                continue;

            var unitProjectileEntity = await applicationDbContext.UnitProjectiles
                .Include(unitProjectile => unitProjectile.Projectiles)
                .FirstOrDefaultAsync(unitProjectile => unitProjectile.GameUnitId == binaryData.UnitId, cancellationToken);

            if (unitProjectileEntity is null)
            {
                unitProjectileEntity = new UnitProjectileEntity
                {
                    GameUnitId = binaryData.UnitId
                };
                await applicationDbContext.UnitProjectiles.AddAsync(unitProjectileEntity, cancellationToken);
            }
            
            MapToEntity(unitProjectileEntity, binaryData);
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return default;
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
