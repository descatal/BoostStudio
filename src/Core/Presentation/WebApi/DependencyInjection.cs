using System.Text.Json;
using System.Text.Json.Serialization;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Web.Constants;
using BoostStudio.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddScoped<IUser, CurrentUser>();
        
        services.AddHttpContextAccessor();

        services.AddHealthChecks();

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddRazorPages();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

        // Configure endpoint json serializer options
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        services.AddEndpointsApiExplorer();
        
        services.AddOpenApi(DefinitionNames.Exvs);
        services.AddOpenApi(DefinitionNames.Exvs2);
        
        return services;
    }
}
