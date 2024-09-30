using System.Net.Mime;
using BoostStudio.Application.Configs.Commands;
using BoostStudio.Application.Configs.Queries;
using BoostStudio.Application.Contracts.Configs;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints.Exvs;

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
    
    private static async Task<ConfigDto[]> GetConfigs(
        ISender sender, 
        [AsParameters] GetConfigsQuery query, 
        CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }
    
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> CreateOrUpdateConfig(
        ISender sender, 
        UpsertConfigCommand command, 
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
    
    // [Produces(MediaTypeNames.Application.Json)]
    // [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    private static async Task<string> GetConfigByKey(ISender sender, string key, CancellationToken cancellationToken)
    {
        return await sender.Send(new GetConfigByKeyQuery(key), cancellationToken);
    }
    
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    private static async Task<IResult> DeleteConfigByKey(ISender sender, string key, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteConfigCommand(key), cancellationToken);
        return Results.NoContent();
    }
}
