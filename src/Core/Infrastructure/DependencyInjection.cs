using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.FhmFormat;
using BoostStudio.Application.Common.Interfaces.Formats.PsarcFormat;
using BoostStudio.Application.Common.Interfaces.Formats.TblFormat;
using BoostStudio.Domain.Entities.PsarcFormat;
using BoostStudio.Formats;
using BoostStudio.Infrastructure.Compressor;
using BoostStudio.Infrastructure.Formats.FhmFormat;
using BoostStudio.Infrastructure.Formats.PsarcFormat;
using BoostStudio.Infrastructure.Formats.TblFormat;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(TimeProvider.System);

        services.AddSingleton<ICompressor, Compressor>();
        
        services.AddSingleton<IFormatSerializer<Fhm>, FhmSerializer>();
        services.AddSingleton<IFormatSerializer<Tbl>, TblSerializer>();
        
        services.AddSingleton<IFhmPacker, FhmPacker>();
        services.AddSingleton<IPsarcPacker, PsarcPacker>();
        services.AddSingleton<ITblMetadataSerializer, TblMetadataSerializer>();

        return services;
    }
}
