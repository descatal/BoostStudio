using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Interfaces.Repositories;

namespace BoostStudio.Application.Configs.Queries;

public record GetConfigByKeyQuery(string Key) : IRequest<string>;

public class GetConfigByKeyQueryHandler(IConfigsRepository configsRepository) : IRequestHandler<GetConfigByKeyQuery, string>
{
    public async ValueTask<string> Handle(GetConfigByKeyQuery request, CancellationToken cancellationToken)
    {
        var config = await configsRepository.GetConfig(request.Key, cancellationToken);
        Guard.Against.NotFound(request.Key, config.Value);
        return config.Value.Value;
    }
}
