using BoostStudio.Domain.Entities.Tbl;
using BoostStudio.Domain.Enums;

namespace BoostStudio.Domain.Entities.Unit.Assets;

public class AssetFile : BaseEntity<uint>
{
    public uint Index { get; set; }

    public ICollection<PatchFile> PatchFiles { get; set; } = [];
    
    public AssetFileType FileType { get; set; }
    
    // can be associated with a specific unit
    public uint? GameUnitId { get; set; }
    
    public Unit? Unit { get; set; }
}
