using System.Reflection;
using System.Text.RegularExpressions;

namespace BoostStudio.Web.Infrastructure;

public static class WebApplicationExtensions
{
    public static RouteGroupBuilder MapGroup(
        this WebApplication app,
        EndpointGroupBase group,
        string? areaName = null
    ) => MapSubgroup(app, group, "", areaName: areaName);

    public static RouteGroupBuilder MapSubgroup(
        this WebApplication app,
        EndpointGroupBase endpointGroup,
        string customTagName,
        string? areaName = null,
        params string[] additionalDirs
    )
    {
        var endpointGroupName = endpointGroup.GetType().Name;

        var tagName = string.IsNullOrWhiteSpace(customTagName) ? endpointGroupName : customTagName;

        var filteredDirs = additionalDirs
            .Prepend(Constants.Endpoints.ApiRoot)
            .Append(endpointGroupName)
            .Where(dir => !string.IsNullOrWhiteSpace(dir))
            .Select(dir => dir.Trim())
            .Select(Slugify)
            .ToList();

        var routeGroup = app.MapGroup(string.Join('/', filteredDirs));

        if (!string.IsNullOrWhiteSpace(areaName))
            routeGroup.WithGroupName(areaName);

        return routeGroup.WithTags(tagName);
    }

    private static string Slugify(string input) =>
        Regex
            .Replace(
                input,
                "([a-z])([A-Z])",
                "$1-$2",
                RegexOptions.CultureInvariant,
                TimeSpan.FromMilliseconds(100)
            )
            .ToLowerInvariant();

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var endpointGroupType = typeof(EndpointGroupBase);

        var assembly = Assembly.GetExecutingAssembly();

        var endpointGroupTypes = assembly
            .GetExportedTypes()
            .Where(t => t.IsSubclassOf(endpointGroupType));

        foreach (var type in endpointGroupTypes)
        {
            if (Activator.CreateInstance(type) is EndpointGroupBase instance)
            {
                instance.Map(app);
            }
        }

        return app;
    }
}
