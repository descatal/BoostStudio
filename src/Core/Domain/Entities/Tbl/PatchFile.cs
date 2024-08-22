using BoostStudio.Domain.Enums;

namespace BoostStudio.Domain.Entities.Tbl;

public class PatchFile : BaseEntity<Guid>
{
    public uint Hash { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Path { get; set; } = string.Empty;
    
    public PatchFileVersion Version { get; set; }
    
    public ulong Size1 { get; set; }
    
    public ulong Size2 { get; set; }
    
    public ulong Size3 { get; set; }
}
