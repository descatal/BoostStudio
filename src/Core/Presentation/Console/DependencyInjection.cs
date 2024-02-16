using System.CommandLine.Hosting;
using Console.Commands;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddConsoleServices(this IServiceCollection services)
    {
        return services;
    }
    
    public static IHostBuilder AddConsoleCommands(this IHostBuilder builder)
    {
        return builder;
    }
}
