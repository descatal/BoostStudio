using BoostStudio.Domain.Entities.Exvs.Tbl;
using Riok.Mapperly.Abstractions;

namespace BoostStudio.Application.Contracts.Tbl.PatchFiles;

[Mapper]
public static partial class PatchFilesMapper
{
    public static partial IQueryable<PatchFileVm> ProjectToVm(IQueryable<PatchFile> entity);

    public static partial IQueryable<PatchFileSummaryVm> ProjectToSummaryVm(
        IQueryable<PatchFile> entity
    );

    public static partial PatchFileVm ToVm(PatchFile entity);

    public static partial PatchFileSummaryVm ToSummaryVm(PatchFile entity);

    public static partial PatchFile ToEntity(PatchFileDto dto);

    public static partial void Update(PatchFileDto source, PatchFile target);
}
