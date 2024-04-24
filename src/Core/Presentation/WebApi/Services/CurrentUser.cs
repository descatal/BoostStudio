using System.Security.Claims;
using BoostStudio.Application.Common.Interfaces;

namespace BoostStudio.Web.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : IUser
{
    public string? Id => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}
