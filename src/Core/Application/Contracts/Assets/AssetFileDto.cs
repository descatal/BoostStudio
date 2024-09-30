using BoostStudio.Domain.Enums;

namespace BoostStudio.Application.Contracts.Assets;

public record AssetFileVm : AssetFileDto
{
    public required uint Hash { get; set; }
}

public record AssetFileDto
{
    public uint Order { get; set; }
    
    public AssetFileType FileType { get; set; }
    
    public uint? GameUnitId { get; set; }
}
