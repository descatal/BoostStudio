namespace BoostStudio.Application.Contracts.Hitboxes.HitboxGroups;

public class HitboxGroupDto
{
    public uint[] UnitIds { get; set; } = [];
    
    public uint Hash { get; set; }

    public List<HitboxDto> Hitboxes { get; set; } = [];
}
