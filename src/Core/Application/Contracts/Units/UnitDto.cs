﻿using BoostStudio.Application.Contracts.Series;

namespace BoostStudio.Application.Contracts.Units;

public record UnitSummaryVm
{
    public uint UnitId { get; set; }
    public string? SlugName { get; set; }
    public string? NameEnglish { get; set; }
    public string? NameJapanese { get; set; }
    public string? NameChinese { get; set; }
    public uint? SeriesId { get; set; }

    public SeriesVm? Series { get; set; }
}

public record UnitDto : UnitDetailsDto
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
    public byte UnitIndex { get; set; }

    public byte SeriesId { get; set; }

    /// <summary>
    /// Always double 0xFF from observed patterns
    /// </summary>
    public ushort Unk2 { get; set; }

    /// <summary>
    /// Format: F_{{unit_id}}
    /// </summary>
    public string? FString { get; set; }

    /// <summary>
    /// Format: F_OUT_{{unit_id}}
    /// </summary>
    public string? FOutString { get; set; }

    /// <summary>
    /// Format: P_{{unit_id}}
    /// </summary>
    public string? PString { get; set; }

    /// <summary>
    /// Placement of unit's selection order in its series, starts from 0
    /// </summary>
    public byte UnitSelectOrderInSeries { get; set; }

    /// <summary>
    /// Placement of unit's arcade small select sprite texture in the 'ArcadeSelectSmallSprites' asset file
    /// </summary>
    public byte ArcadeSmallSpriteIndex { get; set; }

    /// <summary>
    /// Placement of unit's arcade name select texture in the 'ArcadeSelectUnitNameSprites' asset file
    /// </summary>
    public byte ArcadeUnitNameSpriteIndex { get; set; }

    public byte Unk27 { get; set; }

    /// <summary>
    /// Always single 0 from observed patterns
    /// </summary>
    public byte Unk112 { get; set; }

    /// <summary>
    /// Placement of unit's figure sprite texture in the 'FigurineSprites' asset file
    /// </summary>
    public byte FigurineSpriteIndex { get; set; }

    /// <summary>
    /// Not sure what this is, but seems sequential on some parts
    /// </summary>
    public ushort Unk114 { get; set; }

    public uint Unk124 { get; set; }

    public uint Unk128 { get; set; }

    /// <summary>
    /// Format: IS_COSTUME_{{costume_id}}_T
    /// </summary>
    public string? CatalogStorePilotCostume2TString { get; set; }

    /// <summary>
    /// Format: IS_COSTUME_{{costume_id}}
    /// </summary>
    public string? CatalogStorePilotCostume2String { get; set; }

    /// <summary>
    /// Format: IS_COSTUME_{{costume_id}}_T
    /// </summary>
    public string? CatalogStorePilotCostume3TString { get; set; }

    /// <summary>
    /// Format: IS_COSTUME_{{costume_id}}
    /// </summary>
    public string? CatalogStorePilotCostume3String { get; set; }

    public uint Unk156 { get; set; }

    /// <summary>
    /// Asset hash for arcade selection sprites
    /// Used when selecting unit in arcade mode
    /// Asset contains both unit and pilot sprites (costume 1)
    /// </summary>
    public uint ArcadeSelectionCostume1SpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for arcade selection sprites (optional)
    /// Used when selecting unit in arcade mode
    /// Asset contains both unit and pilot sprites (costume 2)
    /// </summary>
    public uint ArcadeSelectionCostume2SpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for arcade selection sprites (optional)
    /// Used when selecting unit in arcade mode
    /// Asset contains both unit and pilot sprites (costume 3)
    /// </summary>
    public uint ArcadeSelectionCostume3SpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for loading screen (left) sprites
    /// Used during VS loading screen when the unit is on the left side
    /// Asset contains both unit and pilot sprites (costume 1)
    /// </summary>
    public uint LoadingLeftCostume1SpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for loading screen (left) sprites (optional)
    /// Used during VS loading screen when the unit is on the left side
    /// Asset contains both unit and pilot sprites (costume 2)
    /// </summary>
    public uint LoadingLeftCostume2SpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for loading screen (left) sprites (optional)
    /// Used during VS loading screen when the unit is on the left side
    /// Asset contains both unit and pilot sprite (costume 3)
    /// </summary>
    public uint LoadingLeftCostume3SpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for loading screen (right) sprites
    /// Used during VS loading screen when the unit is on the right side
    /// Asset contains both unit and pilot sprites (costume 1)
    /// </summary>
    public uint LoadingRightCostume1SpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for loading screen (right) sprites (optional)
    /// Used during VS loading screen when the unit is on the right side
    /// Asset contains both unit and pilot sprites (costume 2)
    /// </summary>
    public uint LoadingRightCostume2SpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for loading screen (right) sprites (optional)
    /// Used during VS loading screen when the unit is on the right side
    /// Asset contains both unit and pilot sprites (costume 3)
    /// </summary>
    public uint LoadingRightCostume3SpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for generic selection sprites
    /// Mostly used in vertical unit selection menu e.g. Free Battle / FB Missions
    /// Asset contains both unit and pilot sprite (costume 1)
    /// </summary>
    public uint GenericSelectionCostume1SpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for generic selection sprites (optional)
    /// Mostly used in vertical unit selection menu e.g. Free Battle / FB Missions
    /// Asset contains both unit and pilot sprites (costume 2)
    /// </summary>
    public uint GenericSelectionCostume2SpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for generic selection sprites (optional)
    /// Mostly used in vertical unit selection menu e.g. Free Battle / FB Missions
    /// Asset contains both unit and pilot sprites (costume 3)
    /// </summary>
    public uint GenericSelectionCostume3SpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for loading screen target unit sprite
    /// Mainly used in Arcade / CPU battles where this unit is the designated target, usually is the same sprite as loading right but bigger
    /// Asset only contains unit sprite
    /// </summary>
    public uint LoadingTargetUnitSpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for loading screen pilot target pilot sprite
    /// Mainly used in Arcade / CPU battles where this unit is the designated target, usually is the same sprite as loading right but bigger
    /// Asset only contains pilot costume 1 sprite
    /// </summary>
    public uint LoadingTargetPilotCostume1SpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for loading screen pilot target pilot sprite (optional)
    /// Mainly used in Arcade / CPU battles where this unit is the designated target, usually is the same sprite as loading right but bigger
    /// Asset only contains pilot costume 2 sprite
    /// </summary>
    public uint LoadingTargetPilotCostume2SpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for loading screen pilot target pilot sprite (optional)
    /// Mainly used in Arcade / CPU battles where this unit is the designated target, usually is the same sprite as loading right but bigger
    /// Asset only contains pilot costume 3 sprite
    /// </summary>
    public uint LoadingTargetPilotCostume3SpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for in game sortie and awakening sprites
    /// Used during initial sortie bottom left pilot costume 1 speaking and awakening cut-in
    /// Asset contains two folders:
    ///   1. Bottom left pilot costume 1 speaking sprite with mouth piece frame sprites, alongside sprite placement / script file LMB
    ///   2. Awakening cut-in pilot costume 1 sprite with background / effects etc, alongside sprite placement / script file LMB
    /// </summary>
    public uint InGameSortieAndAwakeningPilotCostume1SpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for in game sortie and awakening sprites (optional)
    /// Used during initial sortie bottom left pilot costume 2 speaking and awakening cut-in
    /// Asset contains two folders:
    ///   1. Bottom left pilot costume 2 speaking sprite with mouth piece frame sprites, alongside sprite placement / script file LMB
    ///   2. Awakening cut-in pilot costume 2 sprite with background / effects etc, alongside sprite placement / script file LMB
    /// </summary>
    public uint InGameSortieAndAwakeningPilotCostume2SpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for in game sortie and awakening sprites (optional)
    /// Used during initial sortie bottom left pilot costume 3 speaking and awakening cut-in
    /// Asset contains two folders:
    ///   1. Bottom left pilot costume 3 speaking sprite with mouth piece frame sprites, alongside sprite placement / script file LMB
    ///   2. Awakening cut-in pilot costume 3 sprite with background / effects etc, alongside sprite placement / script file LMB
    /// </summary>
    public uint InGameSortieAndAwakeningPilotCostume3SpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for sprite frame data, also known as KPKP format
    /// In game sortie sprite's mouth piece sprite &quot;movement&quot; is controlled by this file
    /// </summary>
    public uint SpriteFramesAssetHash { get; set; }

    /// <summary>
    /// Asset hash for result screen sidebar scoreboard's unit sprite
    /// Asset only contains unit sprite
    /// </summary>
    public uint ResultSmallUnitSpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for unit's standalone figurine sprite
    /// Unused / deprecated in game, the game respects the figurine index instead of this
    /// </summary>
    public uint FigurineSpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for loading screen small target unit sprite
    /// More compact version of `loading_target_unit_sprite_asset_hash`, probably used in similiar scenarios
    /// Asset only contains unit sprite
    /// </summary>
    public uint LoadingTargetUnitSmallSpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for catalog store pilot costume 2 sprite (optional)
    /// Used as a preview on the online catalog store for users to purchase these
    /// </summary>
    public uint CatalogStorePilotCostume2SpriteAssetHash { get; set; }

    /// <summary>
    /// Asset hash for catalog store pilot costume 3 sprite (optional)
    /// Used as a preview on the online catalog store for users to purchase these
    /// </summary>
    public uint CatalogStorePilotCostume3SpriteAssetHash { get; set; }
}
