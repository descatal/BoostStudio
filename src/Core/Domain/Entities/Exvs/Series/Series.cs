﻿using BoostStudio.Domain.Entities.Exvs.Assets;

namespace BoostStudio.Domain.Entities.Exvs.Series;

public class Series : BaseEntity<byte>
{
    public new byte Id { get; set; }

    public byte Unk2 { get; set; }

    public byte Unk3 { get; set; }

    public byte Unk4 { get; set; }

    public byte SelectOrder { get; set; }

    public byte LogoSpriteIndex { get; set; }

    public byte LogoSprite2Index { get; set; }

    public byte Unk11 { get; set; }

    public uint? MovieAssetHash { get; set; }

    public AssetFile? MovieAsset { get; set; }

    // some of the fields data such as names needs to be manually identified, which comes from the program's appsettings.json mapping
    // if possible would be better to pre seed these in table, but currently ef core does not support optional principal foreign key
    // https://github.com/dotnet/efcore/issues/13146
    public string SlugName { get; set; } = string.Empty;

    public string NameEnglish { get; set; } = string.Empty;

    public string NameJapanese { get; set; } = string.Empty;

    public string NameChinese { get; set; } = string.Empty;
}
