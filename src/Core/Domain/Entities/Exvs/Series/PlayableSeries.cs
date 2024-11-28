using BoostStudio.Domain.Entities.Exvs.Assets;

namespace BoostStudio.Domain.Entities.Exvs.Series;

public class PlayableSeries : BaseEntity<Guid>
{
    public byte InGameId { get; set; }

    public byte Unk2 { get; set; }

    public byte Unk3 { get; set; }

    public byte Unk4 { get; set; }

    public byte SelectOrder { get; set; }

    public byte LogoSpriteIndex { get; set; }

    public byte LogoSpriteIndex2 { get; set; }

    public byte Unk11 { get; set; }

    public uint? MovieAssetHash { get; set; }

    public AssetFile? MovieAsset { get; set; }
}
