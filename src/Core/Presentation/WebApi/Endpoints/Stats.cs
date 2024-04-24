using BoostStudio.Application.Exvs.Ammo.Commands;
using BoostStudio.Application.Exvs.Ammo.Models;
using BoostStudio.Application.Exvs.Ammo.Queries;
using BoostStudio.Application.Formats.AmmoFormat.Commands;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints;

public class Stats : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetStats)
            .MapPost(CreateStats)
            .MapPost(BulkCreateStats, "bulk")
            .MapGet(SerializeStats, "serialize-path")
            .MapGet(DeserializeStats, "deserialize-path");
    }
    
    private static async Task<AmmoView> GetStats(ISender sender, [AsParameters] GetAmmoQuery request, CancellationToken cancellationToken)
    {
        return await sender.Send(request, cancellationToken);
    }

    private static async Task CreateStats(ISender sender, CreateAmmoCommand request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
    
    private static async Task BulkCreateStats(ISender sender, BulkCreateAmmoCommand request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
    
    private static async Task SerializeStats(ISender sender, [AsParameters] SerializeAmmoCommand request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
    
    private static async Task<AmmoView> DeserializeStats(ISender sender, [AsParameters] DeserializeAmmoCommand command, CancellationToken cancellationToken)
    {
        return await sender.Send(command, cancellationToken);
    }
}
