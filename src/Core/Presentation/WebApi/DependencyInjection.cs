using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Web.Services;
using DotSwashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

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
        
        // If not explicitly bound, swashbuckle binds json path to swagger doc name
        const string swaggerDocName = "v1";
        services.AddSwaggerGen(opts =>
        {
            opts.DescribeAllParametersInCamelCase();
            opts.SwaggerDoc(swaggerDocName, new OpenApiInfo
            {
                Title = "BoostStudio API",
                Description = "API documentation for BoostStudio Service",
                Version = "1.0.0"
            });
            opts.EnableAnnotations();

            // https://github.com/domaindrivendev/Swashbuckle.AspNetCore#include-descriptions-from-xml-comments
            var filePath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            if (File.Exists(filePath))
                opts.IncludeXmlComments(filePath);
            
            opts.ExampleFilters();
        });

        // Enable Swashbuckle.AspNetCore.Filters multiple example provider
        // AddSwaggerExamples requires manual registration
        // AddSwaggerExamplesFromAssemblies uses reflection to register all example providers
        services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());
        
        return services;
    }
}
