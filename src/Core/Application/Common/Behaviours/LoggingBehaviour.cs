using BoostStudio.Application.Common.Interfaces;
using Mediator;
using Microsoft.Extensions.Logging;

namespace BoostStudio.Application.Common.Behaviours;

public class LoggingBehaviour<TMessage, TResponse>(ILogger<TMessage> logger) : MessagePreProcessor<TMessage, TResponse>
    where TMessage : IMessage
{
    protected override ValueTask Handle(TMessage request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TMessage).Name;
        logger.LogInformation("BoostStudio Request: {Name} {@Request}", requestName, request);
        return default;
    }
}
