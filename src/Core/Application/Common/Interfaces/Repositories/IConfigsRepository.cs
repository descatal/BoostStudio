using BoostStudio.Domain.Common;
using BoostStudio.Domain.Entities;
using ErrorOr;

namespace BoostStudio.Application.Common.Interfaces.Repositories;

public interface IConfigsRepository
{
    public Task<ErrorOr<Config>> GetConfig(string key, CancellationToken cancellationToken = default);
    
    public Task<ErrorOr<List<Config>>> GetConfigs(string[] keys, CancellationToken cancellationToken = default);
    
    public Task<Config> AddOrUpdateConfig<T>(string key, T value, BaseEvent[]? domainEvents = null, CancellationToken cancellationToken = default);
    
    public Task<ErrorOr<T>> GetConfigValue<T>(string key, CancellationToken cancellationToken = default);

    public Task RemoveConfig(string key, CancellationToken cancellationToken = default);

    public Task RemoveConfig(Config config, CancellationToken cancellationToken = default);
}

