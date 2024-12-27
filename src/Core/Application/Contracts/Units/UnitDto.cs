namespace BoostStudio.Application.Contracts.Units;

public record UnitDto
{
    public uint UnitId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string NameJapanese { get; init; } = string.Empty;
    public string NameChinese { get; init; } = string.Empty;
}

public record UnitDto2 : UnitDetailsDto
{
    public uint Id { get; init; }
}

public record UnitDetailsDto
{
    public string SlugName { get; init; } = string.Empty;

    public string NameEnglish { get; init; } = string.Empty;

    public string NameJapanese { get; init; } = string.Empty;

    public string NameChinese { get; init; } = string.Empty;

    public uint? HitboxGroupHash { get; init; }

    public PlayableCharacterDetailsDto? PlayableCharacter { get; init; }
}

public record PlayableCharacterDto : PlayableCharacterDetailsDto
{
    public uint UnitId { get; init; }
}

public record PlayableCharacterDetailsDto
{

}
