namespace BoostStudio.Application.Contracts.Units;

public record UnitDetailsDto
{
    public string Name { get; init; } = string.Empty;
    public string NameJapanese { get; init; } = string.Empty;
    public string NameChinese { get; init; } = string.Empty;
}

