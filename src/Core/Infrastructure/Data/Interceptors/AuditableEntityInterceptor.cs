using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BoostStudio.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor(
    IUser user,
    TimeProvider dateTime
) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<IBaseAuditableEntity>())
        {
            if (entry.State is not (EntityState.Added or EntityState.Modified) && !entry.HasChangedOwnedEntities())
                continue;
            
            var utcNow = dateTime.GetUtcNow().ToUnixTimeMilliseconds();
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = user.Id;
                entry.Entity.Created = utcNow;
            } 
            entry.Entity.LastModifiedBy = user.Id;
            entry.Entity.LastModified = utcNow;
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => 
            r.TargetEntry != null && 
            r.TargetEntry.Metadata.IsOwned() && 
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}
