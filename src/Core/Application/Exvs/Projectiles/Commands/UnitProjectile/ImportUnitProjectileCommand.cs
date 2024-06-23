using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
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
                .ThenInclude(projectile => projectile.Hitbox)
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
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    private static void MapToEntity(
        UnitProjectileEntity unitProjectile,
        ProjectileBinaryFormat binaryData)
    {
        var initialProjectileIds = unitProjectile.Projectiles.Select(projectile => projectile.Id);

        // update the file magic info
        unitProjectile.FileSignature = binaryData.FileMagic;

        foreach (var projectileBody in binaryData.Projectile)
        {
            var existingProjectile = unitProjectile.Projectiles
                .FirstOrDefault(projectile => projectile.Hash == projectileBody.Hash);

            if (existingProjectile is null)
            {
                existingProjectile ??= new ProjectileEntity();
                unitProjectile.Projectiles.Add(existingProjectile);
            }

            ProjectileMapper.MapToEntity(projectileBody.Hash, projectileBody.ProjectileProperties, existingProjectile);
        }

        // remove items not found in the file
        unitProjectile.Projectiles
            .Where(projectile => !initialProjectileIds.Contains(projectile.Id))
            .ToList()
            .ForEach(projectile => unitProjectile.Projectiles.Remove(projectile));
    }
}
