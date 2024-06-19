namespace BoostStudio.Application.Contracts.Projectiles.UnitProjectiles;

public class UnitProjectileDto
{
    public string? Id { get; set; }

    public uint UnitId { get; set; }

    public List<ProjectileDto> Projectiles { get; set; } = [];
}
