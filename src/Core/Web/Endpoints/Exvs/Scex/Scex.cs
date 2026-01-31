using BoostStudio.Application.Exvs.Scex.Commands;
using BoostStudio.Application.Exvs.Scex.Queries;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BoostStudio.Web.Endpoints.Exvs.Scex;

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

    private static async Task<Ok<string>> GetDecompiledScexByUnitId(
        ISender sender,
        uint unitId,
        CancellationToken cancellationToken
    )
    {
        var vm = await sender.Send(new GetDecompiledScexByUnitIdQuery(unitId), cancellationToken);
        return TypedResults.Ok(vm);
    }

    private static async Task<NoContent> CompileScexByPath(
        ISender sender,
        CompileScexByPathCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> DecompileScexByPath(
        ISender sender,
        DecompileScexByPathCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> HotReloadScexByPath(
        ISender sender,
        HotReloadScex request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> CompileScexByUnits(
        ISender sender,
        CompileScexByUnitsCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> DecompileScexByUnits(
        ISender sender,
        DecompileScexByUnitsCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.NoContent();
    }
}
