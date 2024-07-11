using Microsoft.Extensions.Logging;

namespace BoostStudio.Application.Common.Behaviours;

public class UnhandledExceptionBehaviour<TMessage, TResponse>(ILogger<TMessage> logger) : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    public async ValueTask<TResponse> Handle(TMessage message, CancellationToken cancellationToken, MessageHandlerDelegate<TMessage, TResponse> next)
    {
        try
        {
            return await next(message, cancellationToken);
        }
        catch (Exception ex)
        {
            var requestName = typeof(TMessage).Name;

            logger.LogError(ex, "BoostStudio Request: Unhandled Exception for Request {Name} {@Request}", requestName, message);

            throw;
        }
    }
}
