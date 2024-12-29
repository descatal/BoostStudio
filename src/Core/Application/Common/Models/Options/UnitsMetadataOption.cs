namespace BoostStudio.Application.Common.Models.Options;

public record UnitsMetadataOption(
    uint Id,
    string SlugName,
    string NameEnglish,
    string NameJapanese,
    string NameChinese,
    uint? HitboxGroupHash = null,
    string? StagingDirectoryPath = null
);
