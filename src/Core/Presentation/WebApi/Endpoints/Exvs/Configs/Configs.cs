using BoostStudio.Application.Configs.Commands;
using BoostStudio.Application.Configs.Queries;
using BoostStudio.Application.Contracts.Configs;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BoostStudio.Web.Endpoints.Exvs.Configs;

public class Configs : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs)
            .MapGet(GetConfigs)
            .MapPost(CreateOrUpdateConfig)
            .MapGet(GetConfigByKey, "{key}")
            .MapDelete(DeleteConfigByKey, "{key}");
    }

    private static async Task<Ok<ConfigDto[]>> GetConfigs(
        ISender sender,
        [AsParameters] GetConfigsQuery query,
        CancellationToken cancellationToken
    )
    {
        var vm = await sender.Send(query, cancellationToken);
        return TypedResults.Ok(vm);
    }

    private static async Task<NoContent> CreateOrUpdateConfig(
        ISender sender,
        UpsertConfigCommand command,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<Ok<string>> GetConfigByKey(
        ISender sender,
        string key,
        CancellationToken cancellationToken
    )
    {
        var vm = await sender.Send(new GetConfigByKeyQuery(key), cancellationToken);
        return TypedResults.Ok(vm);
    }

    private static async Task<NoContent> DeleteConfigByKey(
        ISender sender,
        string key,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(new DeleteConfigCommand(key), cancellationToken);
        return TypedResults.NoContent();
    }
}
