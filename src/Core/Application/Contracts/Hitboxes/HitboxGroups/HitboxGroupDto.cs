namespace BoostStudio.Application.Contracts.Hitboxes.HitboxGroups;

/// <summary>
/// Different from projectiles, the hitbox binary does not tie a single binary file with one unit
/// It acts more like a global state where all hitbox info loaded in memory can be accessed by anyone
/// However in practical terms, one binary file is usually associated with one unit, except for cases where the info are combined (NPC)
/// During import the link between a hitbox binary and its associated unit is established from an existing mapping
/// Since each binary file does not associate to one unit, it is better to name one single binary file as a single HitboxGroup
/// </summary>
public class HitboxGroupDto
{
    /// <summary>
    /// The first 4 byte hash of the file, used to identify if this file is associated with a unit
    /// </summary>
    public uint Hash { get; set; }

    /// <summary>
    /// A group can have multiple UnitIds associated with it (e.g. NPC)
    /// </summary>
    public uint[] UnitIds { get; set; } = [];

    /// <summary>
    /// All the Hitbox data associated in this group
    /// </summary>
    public List<HitboxDto> Hitboxes { get; set; } = [];
}
