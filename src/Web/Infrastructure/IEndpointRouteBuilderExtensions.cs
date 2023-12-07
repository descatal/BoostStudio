namespace BoostStudio.Web.Infrastructure;

public static class IEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapGet(
        this IEndpointRouteBuilder builder,
        Delegate handler,
        string pattern = "")
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapGet(pattern, handler).WithName(handler.Method.Name);

        return builder;
    }

    public static IEndpointRouteBuilder MapPost(
        this IEndpointRouteBuilder builder,
        Delegate handler,
        string pattern = "",
        bool disableAntiForgery = false)
    {
        Guard.Against.AnonymousMethod(handler);

        var routeBuilder = builder.MapPost(pattern, handler);

        if (disableAntiForgery)
            routeBuilder.DisableAntiforgery();
        
        routeBuilder.WithName(handler.Method.Name);
        
        return builder;
    }

    public static IEndpointRouteBuilder MapPut(this IEndpointRouteBuilder builder, Delegate handler, string pattern)
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapPut(pattern, handler).WithName(handler.Method.Name);

        return builder;
    }

    public static IEndpointRouteBuilder MapDelete(this IEndpointRouteBuilder builder, Delegate handler, string pattern)
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapDelete(pattern, handler).WithName(handler.Method.Name);

        return builder;
    }
}
