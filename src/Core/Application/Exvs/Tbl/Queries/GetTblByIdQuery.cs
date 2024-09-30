using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Contracts.Tbl;
using BoostStudio.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Tbl.Queries;

public record GetTblByIdQuery(PatchFileVersion Id) : IRequest<TblVm>;

public class GetTblByIdQueryHandler(
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetTblByIdQuery, TblVm>
{
    public async ValueTask<TblVm> Handle(GetTblByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await applicationDbContext.Tbl
            .FirstOrDefaultAsync(entity => entity.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);
        
        return TblMapper.ToVm(entity);
    }
}
