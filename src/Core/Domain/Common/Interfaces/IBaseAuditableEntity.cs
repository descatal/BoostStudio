namespace BoostStudio.Domain.Common.Interfaces;

public interface IBaseAuditableEntity
{
    public long Created { get; set; }

    public string? CreatedBy { get; set; }

    public long LastModified { get; set; }

    public string? LastModifiedBy { get; set; }
}
