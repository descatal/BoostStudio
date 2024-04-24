namespace BoostStudio.Domain.Common;

public abstract class BaseAuditableEntity<TId> : BaseEntity<TId>
{
    public long Created { get; set; }

    public string? CreatedBy { get; set; }

    public long LastModified { get; set; }

    public string? LastModifiedBy { get; set; }
}
