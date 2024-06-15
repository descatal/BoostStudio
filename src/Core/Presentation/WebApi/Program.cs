using BoostStudio.Infrastructure.Data;
using Scalar.AspNetCore;
using Serilog;

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
        context.Response.Redirect("/openapi");
        return Task.CompletedTask;
    });
    
    await app.InitialiseDatabaseAsync();

    app.UseHealthChecks("/health");
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.MapOpenApi();
    app.MapScalarApiReference();
    
    app.UseSwagger(opts => opts.RouteTemplate = "openapi/{documentName}.{extension:regex(^(json|ya?ml)$)}");
    app.UseSwaggerUI(opts =>
    {
        opts.DisplayOperationId();
        
        opts.ShowCommonExtensions();

        opts.RoutePrefix = "openapi";
        opts.SwaggerEndpoint("EXVS.json", "EXVS Api");
        opts.SwaggerEndpoint("EXVS2.json","EXVS2 Api");
    });

    app.UseReDoc(options =>
    {
        options.RoutePrefix = "redoc";
        options.SpecUrl = "/openapi/EXVS.json";
    });
    app.UseReDoc(options =>
    {
        options.RoutePrefix = "redoc";
        options.SpecUrl = "/openapi/EXVS2.json";
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
