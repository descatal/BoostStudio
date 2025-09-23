using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace BoostStudio.Application.Common.Behaviours;

public class PerformanceBehaviour<TMessage, TResponse>(ILogger<TMessage> logger)
    : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    private readonly Stopwatch _timer = new();

    public async ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken
    )
    {
        _timer.Start();

        var response = await next(message, cancellationToken);

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds <= 500)
            return response;

        var requestName = typeof(TMessage).Name;

        logger.LogWarning(
            "BoostStudio Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
            requestName,
            elapsedMilliseconds,
            message
        );

        return response;
    }
}
