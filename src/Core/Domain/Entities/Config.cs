namespace BoostStudio.Domain.Entities;

public class Config : BaseEntity<string>
{
    public required string Key { get; set; }
    
    public required string Value { get; set; }
}

