using BoostStudio.Infrastructure.Data;
using BoostStudio.Web.Constants;
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

    builder.Services.AddCors();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    else
    {
        app.UseCors(corsBuilder => corsBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
    }

    app.MapFallback(context =>
    {
        context.Response.Redirect("/openapi");
        return Task.CompletedTask;
    });

    await app.InitialiseDatabaseAsync();

    app.UseHealthChecks("/health");
    app.UseHttpsRedirection();
    
    app.UseWhen(
        context =>
        {
            var path = context.Request.Path.Value?.ToLower().Trim();
            return
                string.IsNullOrWhiteSpace(path) ||
                path.StartsWith("/assets");
        },
        config => config.UseStaticFiles(new StaticFileOptions
        {
            ServeUnknownFileTypes = true
        }));
    
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.Theme = ScalarTheme.Mars;
    });

    // app.UseSwagger(opts => opts.RouteTemplate = "openapi/{documentName}.{extension:regex(^(json|ya?ml)$)}");
    app.UseSwaggerUI(opts =>
    {
        opts.DisplayOperationId();

        opts.ShowCommonExtensions();

        opts.RoutePrefix = "openapi";
        opts.SwaggerEndpoint($"{DefinitionNames.Exvs}.json", $"{DefinitionNames.Exvs} API");
        opts.SwaggerEndpoint($"{DefinitionNames.Exvs2}.json", $"{DefinitionNames.Exvs2} API");
    });

    app.UseReDoc(options =>
    {
        options.RoutePrefix = "redoc";
        options.SpecUrl = $"/openapi/{DefinitionNames.Exvs}.json";
    });
    app.UseReDoc(options =>
    {
        options.RoutePrefix = "redoc";
        options.SpecUrl = $"/openapi/{DefinitionNames.Exvs2}.json";
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
