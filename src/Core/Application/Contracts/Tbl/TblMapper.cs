using Riok.Mapperly.Abstractions;
using TblEntity = BoostStudio.Domain.Entities.Exvs.Tbl.Tbl;

namespace BoostStudio.Application.Contracts.Tbl;

[Mapper]
public static partial class TblMapper
{
    public static partial IQueryable<TblVm> ProjectToVm(IQueryable<TblEntity> entity);

    public static partial TblVm ToVm(TblEntity entity);
}
