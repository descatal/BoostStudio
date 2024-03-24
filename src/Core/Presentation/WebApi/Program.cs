using Serilog;
using Serilog.Formatting.Compact;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "logs/log.txt"), rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    
    builder.Host.UseSerilog();

    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddWebServices();

    var app = builder.Build();

// Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.MapFallback(context =>
    {
        context.Response.Redirect("/swagger");
        return Task.CompletedTask;
    });

    app.UseHealthChecks("/health");
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseSwagger();
    app.UseSwaggerUI(opts =>
    {
        opts.DisplayOperationId();
    });

    app.UseRedoc(options =>
    {
        // defaults to /api-docs
        // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/738
        options.RoutePrefix = "redoc";
        options.DocumentTitle = "BoostStudio API";
        options.SpecUrl = "/swagger/v1/swagger.json";
    });

    app.UseExceptionHandler(_ => {});

    app.MapEndpoints();

    app.Run();
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
