using System.Reflection;
using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.AudioFormats;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Interfaces.Formats.FhmFormat;
using BoostStudio.Application.Common.Interfaces.Formats.PsarcFormat;
using BoostStudio.Application.Common.Interfaces.Formats.TblFormat;
using BoostStudio.Domain.Entities.Unit.Ammo;
using BoostStudio.Formats;
using BoostStudio.Infrastructure.Compressor;
using BoostStudio.Infrastructure.Data;
using BoostStudio.Infrastructure.Data.Interceptors;
using BoostStudio.Infrastructure.Formats.AmmoFormat;
using BoostStudio.Infrastructure.Formats.AudioFormats;
using BoostStudio.Infrastructure.Formats.AudioFormats.Bnsf;
using BoostStudio.Infrastructure.Formats.AudioFormats.Nus3Audio;
using BoostStudio.Infrastructure.Formats.AudioFormats.Wav;
using BoostStudio.Infrastructure.Formats.FhmFormat;
using BoostStudio.Infrastructure.Formats.HitboxFormat;
using BoostStudio.Infrastructure.Formats.ProjectileFormat;
using BoostStudio.Infrastructure.Formats.PsarcFormat;
using BoostStudio.Infrastructure.Formats.StatsFormat;
using BoostStudio.Infrastructure.Formats.TblFormat;
using BoostStudio.Infrastructure.Scex;
using FFMpegCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        #region Database

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        Guard.Against.Null(connectionString, message: "Connection string not found.");

        // Parse connection string to place db files in nested directory
        var sqliteConnectionBuilder = new SqliteConnectionStringBuilder(connectionString);
        var dataSource = sqliteConnectionBuilder.DataSource;
        var directory = Path.GetDirectoryName(dataSource);
        var fileName = Path.GetFileName(dataSource);

        if (string.IsNullOrWhiteSpace(directory) || !Path.IsPathFullyQualified(directory))
            directory = Path.Combine(AppContext.BaseDirectory, directory ?? string.Empty);

        if (!Directory.Exists(directory))
        {
            Console.WriteLine($"Creating new directory at {directory}");
            Directory.CreateDirectory(directory);
        }

        sqliteConnectionBuilder.DataSource = Path.Combine(directory, fileName);
        Console.WriteLine($"Connecting to database using connection string: {sqliteConnectionBuilder}");

        // Parse connection string to place db files in nested directory
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlite(sqliteConnectionBuilder.ToString(), builder =>
            {
                builder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                
                // This is needed to prevent any db locking issue on frequent db write, not the best solution, but necessary.
                builder.CommandTimeout((int)TimeSpan.FromDays(365).TotalSeconds);
            });
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitializer>();

        #endregion
        
        services.AddSingleton(TimeProvider.System);

        services.AddSingleton<ICompressor, Compressor>();
        
        services.AddTransient<IFormatBinarySerializer<Fhm>, FhmBinarySerializer>();
        services.AddTransient<IFormatBinarySerializer<Tbl>, TblBinarySerializer>();
        services.AddTransient<IFormatBinarySerializer<List<Ammo>>, AmmoBinarySerializer>();
        
        services.AddTransient<IUnitStatBinarySerializer, UnitStatBinarySerializer>();
        services.AddTransient<IUnitProjectileBinarySerializer, UnitProjectileBinarySerializer>();
        services.AddTransient<IHitboxGroupBinarySerializer, HitboxGroupGroupBinarySerializer>();
        
        services.AddTransient<IBnsf, Bnsf>();
        services.AddTransient<IRiff, Riff>();
        services.AddTransient<INus3Audio, Nus3Audio>();
        services.AddTransient<IAudioConverter, AudioConverter>();
        
        services.AddSingleton<IFhmPacker, FhmPacker>();
        services.AddSingleton<IPsarcPacker, PsarcPacker>();
        services.AddSingleton<ITblMetadataSerializer, TblMetadataSerializer>();
        
        services.AddSingleton<IScexCompiler, ScexCompiler>();
        
        var ffmpegBinaryFolder = Path.Combine(Path.GetTempPath(), "BoostStudio", "Resources", "ffmpeg");
        GlobalFFOptions.Configure(options =>
        {
            options.BinaryFolder = ffmpegBinaryFolder;
        });

        InitializeResources();
        
        return services;
    }
    
    /// <summary>
    /// Copies the resources from the executing assembly to a temp location.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    private static void InitializeResources()
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), "BoostStudio");
        Directory.CreateDirectory(workingDirectory);
            
        // Extracting executable from resource to a temp location.
        var resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();

        foreach (var resourceName in resources)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
                continue;
            
            using var resourceStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(resourceName);
            
            if (resourceStream is null)
                throw new FileNotFoundException($"{resourceName} resource not found!");
            
            // Get anything after Resources
            // This assumes files in resources have an extension, or else this won't work
            var resourcePathParts = resourceName.Split('.').ToList();
            var resourceDirectoryIndex = resourcePathParts.FindIndex(p => p.Equals("Resources"));
            var resourceDirectoryRelativePath = Path.Combine(resourcePathParts.GetRange(resourceDirectoryIndex, resourcePathParts.Count - resourceDirectoryIndex - 2).ToArray());
            var resourceFileName = string.Join(".", resourcePathParts.TakeLast(2));
            
            var resourceDirectoryPath = Path.Combine(workingDirectory, resourceDirectoryRelativePath);
            var resourcePath = Path.Combine(resourceDirectoryPath, resourceFileName);

            Directory.CreateDirectory(resourceDirectoryPath);
            
            using var fileStream = File.Create(resourcePath);
            resourceStream.CopyTo(fileStream);
            fileStream.Close();
        }
    }
}
