using Console.Commands.Psarc;
using ConsoleAppFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Fatal)
    .WriteTo.Console()
    .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "logs/log.txt"), rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Bind to services( Package Import: Microsoft.Extensions.Options.ConfigurationExtensions )
var services = new ServiceCollection();
var configuration = new ConfigurationBuilder().Build();

services.AddApplicationServices();
services.AddInfrastructureServices(configuration);

services.AddLogging(builder =>
{
    builder.ClearProviders();
    builder.SetMinimumLevel(LogLevel.Information);
    builder.AddSerilog();
});

using var serviceProvider = services.BuildServiceProvider();
ConsoleApp.ServiceProvider = serviceProvider;

var app = ConsoleApp.Create();
app.Add<PackPsarcRequest>();
app.Add<UnpackPsarcRequest>();
app.Run(args);

// try
// {
    // var root = new RootCommand();
    //
    // root.AddRootCommands();
    // root.AddGlobalOption(new Option<bool>("--silent", "Disables diagnostics output"));
    // root.Handler = CommandHandler.Create(() => root.Invoke("-h"));
    //
    // var commandLineBuilder = new CommandLineBuilder(root);
    // var runner = commandLineBuilder.UseHost(_ => CreateHostBuilder(args), (builder) =>
    //     {
    //         builder
    //             .UseEnvironment("CLI")
    //             .ConfigureServices((hostContext, services) =>
    //             {
    //                 var configuration = hostContext.Configuration;
    //                 services.AddConsoleServices(); // Dependencies defined by CLI project (this).
    //                 services.AddApplicationServices(); // Register "Application" project.
    //                 services.AddInfrastructureServices(configuration); // Register "Infrastructure" project.
    //             });
    //
    //         builder.AddConsoleCommands();
    //         builder.ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders());
    //     })
    //     .UseDefaults()
    //     .Build();
    //
    // return await runner.InvokeAsync(args);
// }
// catch (Exception ex)
// {
//     Log.Fatal(ex, "Host terminated unexpectedly");
//     return 1;
// }
// finally
// {
//     Log.CloseAndFlush();
// }

// static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args);
