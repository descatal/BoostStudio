using BoostStudio.Domain.Entities.Exvs.Assets;

namespace BoostStudio.Domain.Entities.Exvs.Series;

public class Series : BaseEntity<uint>
{
    public byte Unk2 { get; set; }

    public byte Unk3 { get; set; }

    public byte Unk4 { get; set; }

    public uint SelectOrder { get; set; }

    public uint LogoSpriteIndex { get; set; }

    public uint LogoSpriteIndex2 { get; set; }

    public uint Unk11 { get; set; }

    public uint MovieAssetHash { get; set; }

    public AssetFile? MovieAsset { get; set; }
}
