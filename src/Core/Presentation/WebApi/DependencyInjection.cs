using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

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

        const string exvs = "EXVS";
        const string exvs2 = "EXVS2";
        services.AddOpenApi(exvs);
        services.AddOpenApi(exvs2);
        
        // If not explicitly bound, swashbuckle binds json path to swagger doc name
        services.AddSwaggerGen(opts =>
        {
            opts.DescribeAllParametersInCamelCase();
            
            opts.SwaggerDoc(exvs, new OpenApiInfo
            {
                Title = "BoostStudio API",
                Description = "API documentation EXVS Api",
                Version = "1.0.0"
            });
            
            opts.SwaggerDoc(exvs2, new OpenApiInfo
            {
                Title = "BoostStudio API 2",
                Description = "API documentation EXVS2 Api",
                Version = "1.0.0"
            });
            
            opts.EnableAnnotations();

            // https://github.com/domaindrivendev/Swashbuckle.AspNetCore#include-descriptions-from-xml-comments
            var filePath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            if (File.Exists(filePath))
                opts.IncludeXmlComments(filePath);
            
            opts.ExampleFilters();
        });

        services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());
        
        return services;
    }
}
