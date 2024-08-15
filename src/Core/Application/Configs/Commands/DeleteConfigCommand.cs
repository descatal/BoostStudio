using BoostStudio.Application.Common.Interfaces.Repositories;

namespace BoostStudio.Application.Configs.Commands;

public record DeleteConfigCommand(string Key) : IRequest;

public class DeleteConfigCommandHandler(
    IConfigsRepository configsRepository
) : IRequestHandler<DeleteConfigCommand>
{
    public async ValueTask<Unit> Handle(DeleteConfigCommand request, CancellationToken cancellationToken)
    {
        await configsRepository.RemoveConfig(request.Key, cancellationToken: cancellationToken);
        return Unit.Value;
    }
}
