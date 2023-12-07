using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using NJsonSchema;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace BoostStudio.Web.Infrastructure;

public class SwaggerOperationProcessor : IOperationProcessor
{
    private readonly ILogger _logger;

    public SwaggerOperationProcessor()
    {
        _logger = LoggerFactory.Create(options => options.AddConsole()).CreateLogger(nameof(SwaggerOperationProcessor));
    }
    
    public bool Process(OperationProcessorContext context)
    {
        // The process method is ran once per controller / minimal Api.
        if (!(context is AspNetCoreOperationProcessorContext aspContext))
            return false;
        
        var operationDescription = aspContext.OperationDescription;
        if (!operationDescription.Operation.ActualConsumes.Any(x => x.Equals(MediaTypeNames.Multipart.FormData, StringComparison.OrdinalIgnoreCase)))
            return true;

        var openApiMediaTypes = operationDescription.Operation.RequestBody.Content.Values;
        if (openApiMediaTypes.Count > 1)
            throw new NotSupportedException("Handling for multiple OpenApiMediaTypes is not supported yet.");
        
        var openApiMediaType = openApiMediaTypes.FirstOrDefault();
        if (openApiMediaType is null)
            return true;
        
        var schema = openApiMediaType.Schema;
        var allProperties = openApiMediaType
            .Schema
            .Properties
            .ToDictionary(x => x.Key, x => x.Value);
        
        schema.Properties.Clear();
        foreach (var properties in allProperties)
            schema.Properties.Add(properties.Key, properties.Value);
        
        return true;
    }
}
