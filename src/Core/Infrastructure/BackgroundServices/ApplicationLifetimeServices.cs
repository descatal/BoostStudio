using BoostStudio.Domain.Events;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BoostStudio.Infrastructure.BackgroundServices;

public class ApplicationLifetimeService(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<ApplicationLifetimeService> logger
) : IHostedLifecycleService
{
    public async Task StartingAsync(CancellationToken cancellationToken) =>
        await PublishEvent("Application starting", new ApplicationStartingEvent(), cancellationToken);
    
    public async Task StartAsync(CancellationToken cancellationToken) =>
        await PublishEvent("Application start", new ApplicationStartEvent(), cancellationToken);
    
    public async Task StartedAsync(CancellationToken cancellationToken) =>
        await PublishEvent("Application started", new ApplicationStartedEvent(), cancellationToken);
        
    public async Task StoppingAsync(CancellationToken cancellationToken) =>
        await PublishEvent("Application stopping", new ApplicationStoppingEvent(), cancellationToken);
    
    public async Task StopAsync(CancellationToken cancellationToken) =>
        await PublishEvent("Application stop", new ApplicationStopEvent(), cancellationToken);

    public async Task StoppedAsync(CancellationToken cancellationToken) =>
        await PublishEvent("Application stopped", new ApplicationStoppedEvent(), cancellationToken);

    private async Task PublishEvent(string eventName, INotification @event, CancellationToken cancellationToken = default)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

        logger.LogDebug(eventName);
        await publisher.Publish(@event, cancellationToken);
    }
}
