using BoostStudio.Application.Exvs2.Audio.Commands.Nus3Audio;
using BoostStudio.Application.Exvs2.Audio.Commands.Nus3Bank;
using BoostStudio.Web.Constants;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BoostStudio.Web.Endpoints.Exvs2;

public class Audio : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this, DefinitionNames.Exvs2)
            .MapGet(UnpackNus3AudioPath, "nus3audio/unpack-path")
            .MapGet(PackNus3AudioPath, "nus3audio/pack-path")
            .MapGet(UnpackNus3BankPath, "nus3bank/unpack-path")
            .MapGet(PackNus3BankPath, "nus3bank/pack-path");
    }

    private static async Task<NoContent> PackNus3AudioPath(
        ISender sender,
        [AsParameters] PackNus3AudioPathCommand pathCommand,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(pathCommand, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> UnpackNus3AudioPath(
        ISender sender,
        [AsParameters] UnpackNus3AudioPathCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> PackNus3BankPath(
        ISender sender,
        [AsParameters] PackNus3BankPathCommand pathCommand,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(pathCommand, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> UnpackNus3BankPath(
        ISender sender,
        [AsParameters] UnpackNus3BankPathCommand request,
        CancellationToken cancellationToken
    )
    {
        await sender.Send(request, cancellationToken);
        return TypedResults.NoContent();
    }
}
