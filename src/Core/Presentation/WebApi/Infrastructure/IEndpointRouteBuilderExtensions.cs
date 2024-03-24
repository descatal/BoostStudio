using Microsoft.AspNetCore.Routing.Patterns;

namespace BoostStudio.Web.Infrastructure;

public static class IEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapGet(this IEndpointRouteBuilder builder, Delegate handler, string pattern = "")
        => MapHttpMethod(builder, HttpMethods.Get, handler, pattern);
    
    public static IEndpointRouteBuilder MapPost(this IEndpointRouteBuilder builder, Delegate handler, string pattern = "")
        => MapHttpMethod(builder, HttpMethods.Post, handler, pattern);

    public static IEndpointRouteBuilder MapPut(this IEndpointRouteBuilder builder, Delegate handler, string pattern = "")
        => MapHttpMethod(builder, HttpMethods.Put, handler, pattern);

    public static IEndpointRouteBuilder MapDelete(this IEndpointRouteBuilder builder, Delegate handler, string pattern = "")
        => MapHttpMethod(builder, HttpMethods.Delete, handler, pattern);

    private static IEndpointRouteBuilder MapHttpMethod(
        this IEndpointRouteBuilder builder,
        string httpVerb,
        Delegate handler,
        string pattern
    ) {
        Guard.Against.AnonymousMethod(handler);

        var routeHandlerBuilder = builder.MapMethods(pattern, [httpVerb], handler);
        
        // Add operation id to endpoint metadata
        routeHandlerBuilder.Add(endpointBuilder =>
        {
            var routeEndpointBuilder = (RouteEndpointBuilder)endpointBuilder;
            var routePattern = routeEndpointBuilder.RoutePattern;
            
            var operationId = string.Join
            (
                "-", // join as slug, nswag will convert into camelCase at client since - is invalid method char
                routePattern.PathSegments
                    .Select(segment => string.Join(" ", segment.Parts.Select(part =>
                        part switch
                        {
                            RoutePatternLiteralPart literalPart => literalPart.Content,
                            RoutePatternParameterPart parameterPart => $"by-{parameterPart.Name}", // converts id parameter to by-id, client to ById
                            _ => "" // separator part, do not map
                        }
                    )))
                    .Prepend(httpVerb.ToLowerInvariant())
            );
            
            endpointBuilder.Metadata.Add(new EndpointNameMetadata(operationId));
        });
        
        return builder;
    }
}
