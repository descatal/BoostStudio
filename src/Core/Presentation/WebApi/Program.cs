using BoostStudio.Application;
using BoostStudio.Infrastructure;
using BoostStudio.Infrastructure.Data;
using BoostStudio.Web;
using Scalar.AspNetCore;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        Path.Combine(AppContext.BaseDirectory, "logs/log.txt"),
        rollingInterval: RollingInterval.Day
    )
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddJsonFile("appsettings-Mappings.json");
    builder.Configuration.AddEnvironmentVariables();

    builder.Logging.ClearProviders();

    builder.Host.UseSerilog();

    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddWebServices();

    builder.Services.AddCors();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.MapOpenApi();
    if (!app.Environment.IsDevelopment())
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    else
    {
        app.UseCors(corsBuilder =>
            corsBuilder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("x-file-name", "Content-Disposition")
        );

        app.UseSwaggerUI(opts =>
        {
            opts.DisplayOperationId();
            opts.RoutePrefix = "openapi";
            opts.ConfigObject.Urls =
            [
                new UrlDescriptor { Name = "exvs", Url = "/openapi/exvs.json" },
            ];
        });
    }

    app.MapFallbackToFile("index.html");

    await app.InitialiseDatabaseAsync();

    app.UseHealthChecks("/healthz");
    // app.UseHttpsRedirection();

    app.UseWhen(
        context =>
        {
            var path = context.Request.Path.Value?.ToLower().Trim();
            return string.IsNullOrWhiteSpace(path) || path.StartsWith("/assets");
        },
        config => config.UseStaticFiles(new StaticFileOptions { ServeUnknownFileTypes = true })
    );

    app.MapScalarApiReference(opt =>
    {
        opt.Theme = ScalarTheme.Mars;
    });

    app.UseExceptionHandler();

    app.MapEndpoints();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program;
