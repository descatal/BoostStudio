using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

using var log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

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

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi(
    settings =>
    {
        settings.Path = "/api";
        settings.DocumentPath = "/api/specification.json";
    });

app.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.UseExceptionHandler(
    options =>
    {
    });

app.Map("/", () => Results.Redirect("/api"));

app.MapEndpoints();

app.Run();

// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program;
