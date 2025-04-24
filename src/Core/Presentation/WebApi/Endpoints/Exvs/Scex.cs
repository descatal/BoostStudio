using BoostStudio.Application.Exvs.Scex.Commands;
using BoostStudio.Application.Exvs.Scex.Queries;
using BoostStudio.Application.Scex.Commands;
using BoostStudio.Web.Constants;

namespace BoostStudio.Web.Endpoints.Exvs;

public class Scex : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs)
            .MapGet(GetDecompiledScexByUnitId, "decompiled/{unitId}")
            .MapPost(CompileScexByPath, "compile/path")
            .MapPost(DecompileScexByPath, "decompile/path")
            .MapPost(HotReloadScexByPath, "hot-reload/path")
            .MapPost(CompileScexByUnits, "compile/units")
            .MapPost(DecompileScexByUnits, "decompile/units");
    }

    private async Task<string> GetDecompiledScexByUnitId(
        ISender sender,
        uint unitId,
        CancellationToken cancellationToken
    )
    {
        return await sender.Send(new GetDecompiledScexByUnitIdQuery(unitId), cancellationToken);
    }

    private async Task CompileScexByPath(
        ISender sender,
        CompileScexByPathCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
    }

    private async Task DecompileScexByPath(
        ISender sender,
        DecompileScexByPathCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
    }

    private async Task HotReloadScexByPath(
        ISender sender,
        HotReloadScex request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
    }

    private async Task CompileScexByUnits(
        ISender sender,
        CompileScexByUnitsCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
    }

    private async Task DecompileScexByUnits(
        ISender sender,
        DecompileScexByUnitsCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
    }
}
