using System.Reflection;
using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.FhmFormat;
using BoostStudio.Application.Common.Interfaces.Formats.PsarcFormat;
using BoostStudio.Application.Common.Interfaces.Formats.TblFormat;
using BoostStudio.Domain.Entities.PsarcFormat;
using BoostStudio.Domain.Entities.Unit;
using BoostStudio.Domain.Entities.Unit.Ammo;
using BoostStudio.Domain.Entities.Unit.Stats;
using BoostStudio.Formats;
using BoostStudio.Infrastructure.Compressor;
using BoostStudio.Infrastructure.Data;
using BoostStudio.Infrastructure.Data.Interceptors;
using BoostStudio.Infrastructure.Formats.AmmoFormat;
using BoostStudio.Infrastructure.Formats.FhmFormat;
using BoostStudio.Infrastructure.Formats.PsarcFormat;
using BoostStudio.Infrastructure.Formats.StatsFormat;
using BoostStudio.Infrastructure.Formats.TblFormat;
using BoostStudio.Infrastructure.Scex;
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

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        // Parse connection string to place db files in nested directory
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlite(connectionString, builder =>
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
        services.AddTransient<IFormatBinarySerializer<StatsBinaryFormat>, UnitStatBinarySerializer>();
        
        services.AddSingleton<IFhmPacker, FhmPacker>();
        services.AddSingleton<IPsarcPacker, PsarcPacker>();
        services.AddSingleton<ITblMetadataSerializer, TblMetadataSerializer>();
        
        services.AddSingleton<IScexCompiler, ScexCompiler>();

        return services;
    }
}
