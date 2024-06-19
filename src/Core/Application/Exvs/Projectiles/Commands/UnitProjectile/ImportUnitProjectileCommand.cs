using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Contracts.Projectiles;
using BoostStudio.Formats;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
                .FirstOrDefaultAsync(unitProjectile =>unitProjectile.GameUnitId == statsBinaryFormat.UnitId, cancellationToken);
            
            if (unit is null)
                continue;
            
            var entity = await applicationDbContext.UnitProjectiles
                .FirstOrDefaultAsync(unitProjectile => unitProjectile.GameUnitId == statsBinaryFormat.UnitId, cancellationToken);
        
            var isExist = entity is not null;
            
            entity ??= new Domain.Entities.Unit.Projectiles.UnitProjectile
            {
                GameUnitId = (uint)statsBinaryFormat.UnitId
            };

            await MapToEntity(entity, statsBinaryFormat, cancellationToken);
            
            if (!isExist)
                await applicationDbContext.UnitProjectiles.AddAsync(entity, cancellationToken);
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
    
    private async Task MapToEntity(
        Domain.Entities.Unit.Projectiles.UnitProjectile entity, 
        ProjectileBinaryFormat binaryFormat, 
        CancellationToken cancellationToken = default)
    {
        var initialProjectileIds = entity.Projectiles.Select(projectile => projectile.Id);
        
        // update the file magic info
        entity.FileSignature = binaryFormat.FileMagic;
        
        foreach (var projectileBody in binaryFormat.Projectile)
        {
            var existingProjectile = entity.Projectiles
                .FirstOrDefault(projectile => projectile.Hash == projectileBody.Hash);

            if (existingProjectile is null)
            {
                existingProjectile ??= new Domain.Entities.Unit.Projectiles.Projectile();
                entity.Projectiles.Add(existingProjectile);
            }
            
            ProjectileMapper.MapToEntity(projectileBody.Hash, projectileBody.ProjectileProperties, existingProjectile);
        }
        
        // remove items not found in the file
        entity.Projectiles
            .Where(projectile => !initialProjectileIds.Contains(projectile.Id))
            .ToList()
            .ForEach(projectile => entity.Projectiles.Remove(projectile));
    }
}
