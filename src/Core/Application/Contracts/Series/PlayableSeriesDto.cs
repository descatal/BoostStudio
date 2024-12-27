namespace BoostStudio.Application.Contracts.Series;

public record SeriesDto : SeriesDetailsDto
{
    public byte Id { get; set; }
}

public record SeriesDetailsDto
{
    public string SlugName { get; set; } = string.Empty;

    public string NameEnglish { get; set; } = string.Empty;

    public string NameJapanese { get; set; } = string.Empty;

    public string NameChinese { get; set; } = string.Empty;

    public PlayableSeriesDetailsDto? PlayableSeries { get; set; }
}

public record PlayableSeriesDto : PlayableSeriesDetailsDto
{
    public byte SeriesId { get; set; }
}

public record PlayableSeriesDetailsDto
{
    public byte Unk2 { get; set; }

    public byte Unk3 { get; set; }

    public byte Unk4 { get; set; }

    public byte SelectOrder { get; set; }

    public byte LogoSpriteIndex { get; set; }

    public byte LogoSprite2Index { get; set; }

    public byte Unk11 { get; set; }

    public uint? MovieAssetHash { get; set; }
}
