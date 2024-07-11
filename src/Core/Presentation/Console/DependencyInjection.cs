using System.CommandLine.Hosting;
using Console.Commands.Psarc;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddConsoleServices(this IServiceCollection services)
    {
        services.AddSerilog();
        return services;
    }
    
    public static IHostBuilder AddConsoleCommands(this IHostBuilder builder)
    {
        // builder.UseCommandHandler<PackPsarcRequest, PackPsarcRequest.Handler>();
        // builder.UseCommandHandler<UnpackPsarcRequest, UnpackPsarcRequest.Handler>();
        
        return builder;
    }
}
