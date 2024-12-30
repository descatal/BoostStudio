using BoostStudio.Application.Contracts.Units;
using BoostStudio.Domain.Enums;

namespace BoostStudio.Application.Contracts.Tbl.PatchFiles;

public record PatchFileSummaryVm : PatchFileVm
{
    public AssetFileDto? AssetFile { get; init; }
}

public record PatchFileVm : PatchFileDto
{
    public required string? Id { get; init; }
}

public record PatchFileDto
{
    public required PatchFileVersion TblId { get; init; }
    public PathInfoDto? PathInfo { get; init; }
    public FileInfoDto? FileInfo { get; init; }
    public uint? AssetFileHash { get; init; }
}

public record PathInfoDto(
    string Path, 
    uint? Order = null
);

public record FileInfoDto(
    PatchFileVersion Version, 
    ulong Size1, 
    ulong Size2, 
    ulong Size3, 
    ulong Size4
);

public record AssetFileDto(
    uint Hash,
    uint Order,
    List<AssetFileType> FileType,
    UnitDto[] Units
);
