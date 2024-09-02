using BoostStudio.Domain.Entities.Unit.Assets;
using BoostStudio.Domain.Enums;
using TblEntity = BoostStudio.Domain.Entities.Tbl.Tbl;

namespace BoostStudio.Domain.Entities.Tbl;

public class PatchFile : BaseEntity<Guid>
{
    // don't ask me why, blame Bandai
    // patch file entry can be an stub file info without path (which is useless), so this is nullable.
    public string? Path { get; set; }

    // patch file entry can be an empty stub with only path, so this is nullable.
    public PatchFileInfo? FileInfo { get; set; }
    
    public PatchFileVersion? TblId { get; set; }
    
    public TblEntity? Tbl { get; set; }
}

public class PatchFileInfo
{
    public PatchFileVersion Version { get; set; }
    
    public ulong Size1 { get; set; }
    
    public ulong Size2 { get; set; }
    
    public ulong Size3 { get; set; }
    
    public uint AssetFileHash { get; set; }
    
    public AssetFile? AssetFile { get; set; }
}
