using BoostStudio.Domain.Entities.Exvs.Series;
using SeriesEntity = BoostStudio.Domain.Entities.Exvs.Series.Series;

namespace BoostStudio.Domain.Entities.Exvs.Units.Characters;

// Character is essentially a Unit + Pilot information
// This table stores the data from ListInfo's 1st file, also known as `SCharacterList`
public class PlayableCharacter
{
    public byte UnitIndex { get; set; }

    public byte SeriesId { get; set; }

    public SeriesEntity? Series { get; set; }

    public uint UnitId { get; set; }

    public Unit? Unit { get; set; }

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
}
