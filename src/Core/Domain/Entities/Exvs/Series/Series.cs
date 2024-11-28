namespace BoostStudio.Domain.Entities.Exvs.Series;

public class Series : BaseEntity<byte>
{
    public string Name { get; set; } = string.Empty;

    public string NameJapanese { get; set; } = string.Empty;

    public string NameChinese { get; set; } = string.Empty;
}
