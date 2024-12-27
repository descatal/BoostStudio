namespace BoostStudio.Domain.Entities.Exvs.Series;

public class Series : BaseEntity<byte>
{
    public new byte Id { get; set; }

    public PlayableSeries? PlayableSeries { get; set; }

    // some of the fields data such as names needs to be manually identified, which comes from the program's appsettings.json mapping
    // if possible would be better to pre seed these in table, but currently ef core does not support optional principal foreign key
    // https://github.com/dotnet/efcore/issues/13146
    public string SlugName { get; set; } = string.Empty;

    public string NameEnglish { get; set; } = string.Empty;

    public string NameJapanese { get; set; } = string.Empty;

    public string NameChinese { get; set; } = string.Empty;
}
