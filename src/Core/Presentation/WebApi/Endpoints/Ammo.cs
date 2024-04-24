using BoostStudio.Application.Exvs.Ammo.Commands;
using BoostStudio.Application.Exvs.Ammo.Models;
using BoostStudio.Application.Exvs.Ammo.Queries;
using BoostStudio.Application.Formats.AmmoFormat.Commands;
using Microsoft.AspNetCore.Mvc;

namespace BoostStudio.Web.Endpoints;

public class Ammo : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetAmmo)
            .MapPost(CreateAmmo)
            .MapPost(BulkCreateAmmo, "bulk")
            .MapGet(SerializeAmmo, "serialize-path")
            .MapGet(DeserializeAmmo, "deserialize-path");
    }
    
    private static async Task<AmmoView> GetAmmo(ISender sender, [AsParameters] GetAmmoQuery request, CancellationToken cancellationToken)
    {
        return await sender.Send(request, cancellationToken);
    }

    private static async Task CreateAmmo(ISender sender, CreateAmmoCommand request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
    
    private static async Task BulkCreateAmmo(ISender sender, BulkCreateAmmoCommand request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
    
    private static async Task SerializeAmmo(ISender sender, [AsParameters] SerializeAmmoCommand request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
    }
    
    private static async Task<AmmoView> DeserializeAmmo(ISender sender, [AsParameters] DeserializeAmmoCommand command, CancellationToken cancellationToken)
    {
        return await sender.Send(command, cancellationToken);
    }
}
