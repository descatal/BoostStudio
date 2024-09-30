using BoostStudio.Domain.Enums;

namespace BoostStudio.Application.Contracts.Tbl;

public record TblVm : TblDto
{
    public required string Id { get; set; }
}

public record TblDto
{
    public uint CumulativeAssetIndex { get; set; }
}
