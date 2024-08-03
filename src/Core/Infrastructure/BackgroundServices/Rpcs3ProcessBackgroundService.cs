using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BoostStudio.Infrastructure.BackgroundServices;

public class Rpcs3ProcessBackgroundService(
    ILogger<Rpcs3ProcessBackgroundService> logger
) : BackgroundService
{
    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(5));
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting RPCS3 process listener...");
        
        while (await _timer.WaitForNextTickAsync(stoppingToken)
               && !stoppingToken.IsCancellationRequested)
        {
            // do it later
        }
    }
}
