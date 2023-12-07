using BoostStudio.Infrastructure.Identity;

namespace BoostStudio.Web.Endpoints;

public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this).MapIdentityApi<ApplicationUser>();
    }
}
