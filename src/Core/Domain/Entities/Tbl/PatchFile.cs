using BoostStudio.Domain.Entities.Unit.Assets;
using BoostStudio.Domain.Enums;
using TblEntity = BoostStudio.Domain.Entities.Tbl.Tbl;

namespace BoostStudio.Domain.Entities.Tbl;

public class PatchFile : BaseEntity<Guid>
{
    // don't ask me why, blame Bandai
    // patch file entry can be an stub file info without path (which is useless), so this is nullable.
    public PathInfo? PathInfo { get; set; }

    // patch file entry can be an empty stub with only path, so this is nullable.
    public PatchFileInfo? FileInfo { get; set; }
    
    // this relationship should be inside the PatchFileInfo owned entity, but ef core does not support owned entity relationship yet
    // in theory if there's no file info there should be no AssetFile relationship
    // monitor here:
    // https://github.com/dotnet/efcore/issues/27175
    // https://github.com/dotnet/efcore/issues/1985
    public uint? AssetFileHash { get; set; }
    
    public AssetFile? AssetFile { get; set; }
    
    
    // a patch file entry must be associated with a tbl, since this entity is the metadata representation of tbl format
    public PatchFileVersion TblId { get; set; }
    
    public TblEntity? Tbl { get; set; }
}

public class PathInfo
{
    public string Path { get; set; } = string.Empty;
    
    public uint Order { get; set; }
}

public class PatchFileInfo
{
    // it is possible to have different patch file version compared to the parent tbl, so this field is needed
    // blame bandai
    public PatchFileVersion Version { get; set; }
    
    public ulong Size1 { get; set; }
    
    public ulong Size2 { get; set; }
    
    public ulong Size3 { get; set; }
    
    public ulong Size4 { get; set; }
}
