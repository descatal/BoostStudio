using System.CommandLine.Hosting;
using BoostStudio.Application.Formats.PsarcFormat;
using Console.Commands;
using Console.Commands.Psarc;
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
        builder.UseCommandHandler<PackPsarcCommand, PackPsarcCommand.Handler>();
        builder.UseCommandHandler<UnpackPsarcCommand, UnpackPsarcCommand.Handler>();
        
        return builder;
    }
}
