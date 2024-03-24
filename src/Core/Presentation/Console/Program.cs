using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

var root = new RootCommand();

root.AddRootCommands();
root.AddGlobalOption(new Option<bool>("--silent", "Disables diagnostics output"));
root.Handler = CommandHandler.Create(() => root.Invoke("-h"));

var commandLineBuilder = new CommandLineBuilder(root);
var runner = commandLineBuilder.UseHost(_ => CreateHostBuilder(args), (builder) =>
    {
        builder
            .UseEnvironment("CLI")
            .ConfigureServices((hostContext, services) =>
            {
                var configuration = hostContext.Configuration;
                services.AddConsoleServices(); // Dependencies defined by CLI project (this).
                services.AddApplicationServices(); // Register "Application" project.
                services.AddInfrastructureServices(configuration); // Register "Infrastructure" project.
            });

        builder.AddConsoleCommands();
        builder.ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders());
        
        using var log = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    })
    .UseDefaults()
    .Build();

return await runner.InvokeAsync(args);

static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args);
