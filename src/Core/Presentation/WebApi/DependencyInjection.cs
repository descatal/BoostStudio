﻿using System.Text.Json;
using System.Text.Json.Serialization;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Web.Constants;
using BoostStudio.Web.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace BoostStudio.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddScoped<IUser, CurrentUser>();
        services.AddHttpContextAccessor();

        services.AddHealthChecks();

        services.AddProblemDetails();
        services.AddExceptionHandler<CustomExceptionHandler>();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true
        );

        // Configure endpoint json serializer options
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        services.AddOpenApi(
            DefinitionNames.Exvs,
            options =>
            {
                options.AddSchemaTransformer(
                    (schema, context, _) =>
                    {
                        // Transform any FileContentHttpResult into binary format
                        if (context.JsonTypeInfo.Type == typeof(FileContentHttpResult))
                        {
                            schema.Type = JsonSchemaType.String;
                            schema.Format = "binary";
                            schema.Properties = null;
                        }
                        return Task.CompletedTask;
                    }
                );

                options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_1;
            }
        );
        services.AddOpenApi(DefinitionNames.Exvs2);

        return services;
    }
}
