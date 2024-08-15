using BoostStudio.Application.Common.Interfaces.Repositories;
using BoostStudio.Application.Contracts.Configs;

namespace BoostStudio.Application.Configs.Queries;

public record GetConfigsQuery(string[] Keys) : IRequest<ConfigDto[]>;

public class GetConfigsQueryHandler(IConfigsRepository configsRepository) : IRequestHandler<GetConfigsQuery, ConfigDto[]>
{
    public async ValueTask<ConfigDto[]> Handle(GetConfigsQuery request, CancellationToken cancellationToken)
    {
        var query = await configsRepository.GetConfigs(request.Keys, cancellationToken);
        if (query.IsError)
            throw new Exception(query.FirstError.Description);

        return query.Value.Select(config => new ConfigDto(config.Key, config.Value)).ToArray();
    }
}
