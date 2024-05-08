namespace BoostStudio.Application.Contracts.Units;

public record UnitDto
{
    public uint GameUnitId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string NameJapanese { get; init; } = string.Empty;
    public string NameChinese { get; init; } = string.Empty;
}

