using BoostStudio.Application.Common.Interfaces.Repositories;

namespace BoostStudio.Application.Configs.Commands;

public record UpsertConfigCommand(string Key, string Value) : IRequest;

public class UpsertConfigCommandHandler(
    IConfigsRepository configsRepository
) : IRequestHandler<UpsertConfigCommand>
{
    public async ValueTask<Unit> Handle(UpsertConfigCommand request, CancellationToken cancellationToken)
    {
        await configsRepository.AddOrUpdateConfig(request.Key, request.Value, cancellationToken: cancellationToken);
        return Unit.Value;
    }
}
