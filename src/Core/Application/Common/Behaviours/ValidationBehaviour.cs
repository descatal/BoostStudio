using ValidationException=BoostStudio.Application.Common.Exceptions.ValidationException;

namespace BoostStudio.Application.Common.Behaviours;

public class ValidationBehaviour<TMessage, TResponse>(IEnumerable<IValidator<TMessage>> validators) : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    public async ValueTask<TResponse> Handle(TMessage message, CancellationToken cancellationToken, MessageHandlerDelegate<TMessage, TResponse> next)
    {
        if (!validators.Any())
            return await next(message, cancellationToken);
        
        var context = new ValidationContext<TMessage>(message);

        var validationResults = await Task.WhenAll(
            validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Count != 0)
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Count != 0)
            throw new ValidationException(failures);
        
        return await next(message, cancellationToken);
    }
}
