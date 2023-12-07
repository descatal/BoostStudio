using BoostStudio.Application.Common.Models;

namespace BoostStudio.Web.Contracts.Fhm;

public record UnpackFhmRequest(IFormFile File)
{
    public IFormFile File { get; } = File;
}
