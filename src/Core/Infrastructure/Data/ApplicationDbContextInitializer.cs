using BoostStudio.Application.Common.Constants;
using BoostStudio.Application.Common.Enums.Assets;
using BoostStudio.Application.Common.Models.Options;
using BoostStudio.Domain.Entities.Exvs.Assets;
using BoostStudio.Domain.Entities.Exvs.Series;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BoostStudio.Infrastructure.Data;

public static class InitializerExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();

        await initializer.InitialiseAsync();

        await initializer.SeedAsync();
    }
}

public class ApplicationDbContextInitializer(
    ApplicationDbContext context,
    ILogger<ApplicationDbContextInitializer> logger)
{
    public async Task InitialiseAsync()
    {
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        // Default data
        // Seed, if necessary
    }

    private async Task SeedCommonAssets()
    {
        var query = context.AssetFiles.AsQueryable();
        var exvsCommonAssets = Enum.GetValues<ExvsCommonAssets>();
        var exvsCommonAssetsHash = exvsCommonAssets.Select(assets => (uint)assets).ToArray();

        query = query.Where(assetFile => exvsCommonAssetsHash.Contains((uint)assetFile.FileType));

        // do upsert
        var assetFiles = query.ToList();
        foreach (var exvsCommonAsset in exvsCommonAssets)
        {
            var assetFile = assetFiles.FirstOrDefault(assetFile => assetFile.Hash == (uint)exvsCommonAsset);
            if (assetFile is null)
            {
                assetFile = new AssetFile
                {
                    // only update the order on new creation
                    Hash = (uint)exvsCommonAsset,
                    Order = exvsCommonAsset.GetDefaultOrderIndex()
                };
                context.AssetFiles.Add(assetFile);
            }

            // update the file type if it matches
            assetFile.FileType = exvsCommonAsset.GetAssetFileType();
        }

        await context.SaveChangesAsync();
    }
}
