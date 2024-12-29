using BoostStudio.Domain.Entities.Exvs.Assets;
using BoostStudio.Domain.Entities.Exvs.Units.Characters;

namespace BoostStudio.Domain.Entities.Exvs.Series;

public class PlayableSeries
{
    public byte Unk2 { get; set; }

    public byte Unk3 { get; set; }

    public byte Unk4 { get; set; }

    public byte SelectOrder { get; set; }

    public byte LogoSpriteIndex { get; set; }

    public byte LogoSprite2Index { get; set; }

    public byte Unk11 { get; set; }

    public uint? MovieAssetHash { get; set; }

    public AssetFile? MovieAsset { get; set; }

    public byte SeriesId { get; set; }

    public Series? Series { get; set; }
}
