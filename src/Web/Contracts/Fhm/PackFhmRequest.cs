using BoostStudio.Application.Common.Models;

namespace BoostStudio.Web.Contracts.Fhm;

public record PackFhmRequest(IFormFile File)
{
    public IFormFile File { get; } = File;
}
