using BoostStudio.Application.Common.Constants;
using BoostStudio.Application.Common.Models.Options;
using ServiceScan.SourceGenerator;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Transient;
        });
        
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        
        // services.AddMediatR(cfg =>
        // {
        //     cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        //     cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        //     cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        //     cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        // });

        services.AddHttpClient("IgnoreSsl").ConfigurePrimaryHttpMessageHandler(builder =>
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (m, c, ch, e) => true
            };
        });

        services.AddOptions<List<SeriesMetadataOption>>()
            .BindConfiguration(AppSettingPaths.Series)
            .ValidateOnStart();

        services.AddOptions<List<UnitsMetadataOption>>()
            .BindConfiguration(AppSettingPaths.Units)
            .ValidateOnStart();

        return services;
    }
}

public static partial class ServicesExtensions
{
    [GenerateServiceRegistrations(AssignableTo = typeof(IValidator<>), Lifetime = ServiceLifetime.Singleton)]
    public static partial IServiceCollection AddValidators(this IServiceCollection services);
}
