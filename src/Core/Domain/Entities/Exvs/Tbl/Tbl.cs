using BoostStudio.Domain.Enums;

namespace BoostStudio.Domain.Entities.Exvs.Tbl;

public class Tbl : BaseEntity<PatchFileVersion>
{
    // the index of the last asset that's recorded in this TBL
    // since Tbl is a collection of append only file metadata structure,
    // the index count for each tbl should only increase or remain the same for each new patch
    public uint CumulativeAssetIndex { get; set; }
    
    public ICollection<PatchFile> PatchFiles { get; set; } = [];
}
