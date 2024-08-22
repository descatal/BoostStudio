using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace BoostStudio.Infrastructure.Data.ValueGenerators;

public class UUIDv7Generator : ValueGenerator
{
    protected override object NextValue(EntityEntry entry) => Guid.CreateVersion7(DateTimeOffset.UtcNow);

    public override bool GeneratesTemporaryValues { get; }
}
