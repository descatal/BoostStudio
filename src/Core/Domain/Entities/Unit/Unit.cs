namespace BoostStudio.Domain.Entities.Unit;

public class Unit : BaseEntity<uint>
{
    public string Name { get; set; } = string.Empty;
    
    public string NameJapanese { get; set; } = string.Empty;
    
    public string NameChinese { get; set; } = string.Empty;
}
