using BoostStudio.Domain.Entities.Exvs.Tbl;
using BoostStudio.Domain.Enums;

namespace BoostStudio.Domain.Entities.Exvs.Assets;

public class AssetFile : BaseEntity<uint> // id is ignored, Hash is the key
{
    public uint Hash { get; set; }
    
    public uint Order { get; set; }

    // should be PatchFileInfo here, but for now ef core can't map to owned entity yet
    public ICollection<PatchFile> PatchFiles { get; set; } = [];
    
    public AssetFileType FileType { get; set; }

    public ICollection<Units.Unit> Units { get; set; } = [];
}
