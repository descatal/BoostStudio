using System.Text.Json;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Repositories;
using BoostStudio.Domain.Common;
using BoostStudio.Domain.Entities;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Infrastructure.Data.Repositories;

public class ConfigsRepository(
    IApplicationDbContext applicationDbContext
) : IConfigsRepository
{
    public async Task<ErrorOr<Config>> GetConfig(string key, CancellationToken cancellationToken = default)
    {
        var config = await applicationDbContext.Configs
            .FirstOrDefaultAsync(c => c.Key.ToLower() == key.ToLower(), cancellationToken);      
        
        if (config is null)
            return Error.NotFound($"{key} config not found!");
        
        return config;
    }
    
    public async Task<ErrorOr<List<Config>>> GetConfigs(string[] keys, CancellationToken cancellationToken = default)
    {
        var keysInLowerCase = keys.Select(key => key.ToLower());
        var configs = await applicationDbContext.Configs
            .Where(c => keysInLowerCase.Contains(c.Key.ToLower()))
            .ToListAsync(cancellationToken);      
        
        return configs;
    }
    
    public async Task<Config> AddOrUpdateConfig<T>(string key, T value, BaseEvent[]? domainEvents = null, CancellationToken cancellationToken = default)
    {
        var existingConfig = await applicationDbContext.Configs
            .FirstOrDefaultAsync(c => c.Key.ToLower() == key.ToLower(), cancellationToken);
        
        var serializedValue = value is string valueString ? valueString : JsonSerializer.Serialize(value);
        var newConfig = new Config
        {
            Key = key,
            Value = serializedValue
        };
        
        // Check if the value passed in here has any DomainEvents, and if this is an update operation assign the domain event to the existingConfig object.
        if (domainEvents is not null && existingConfig is not null)
            domainEvents.ToList().ForEach(evt => existingConfig.AddDomainEvent(evt));
        
        // Add or update
        if (existingConfig is null)
            await applicationDbContext.Configs.AddAsync(newConfig, cancellationToken);
        else
            existingConfig.Value = serializedValue;
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return existingConfig ?? newConfig;
    }
    
    public async Task<ErrorOr<T>> GetConfigValue<T>(string key, CancellationToken cancellationToken = default)
    {
        var config = await applicationDbContext.Configs
            .FirstOrDefaultAsync(c => c.Key.ToLower() == key.ToLower(), cancellationToken);        
        if (config is null)
            return Error.NotFound($"{key} config not found!");

        var deserializedValue = JsonSerializer.Deserialize<T>(config.Value);
        if (deserializedValue is null)
            return Error.Unexpected($"{key} config value cannot be deserialized (malformed)!");

        return deserializedValue;
    }
    
    public async Task RemoveConfig(string key, CancellationToken cancellationToken = default)
    {
        var config = await applicationDbContext.Configs
            .FirstOrDefaultAsync(c => c.Key.ToLower() == key.ToLower(), cancellationToken);
        
        // Ignore if the key is not found
        if (config is null)
            return;
        
        applicationDbContext.Configs.Remove(config);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task RemoveConfig(Config config, CancellationToken cancellationToken = default)
    {
        applicationDbContext.Configs.Remove(config);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
