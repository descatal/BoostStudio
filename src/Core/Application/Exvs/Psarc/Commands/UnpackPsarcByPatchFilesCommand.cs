using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Constants;
using BoostStudio.Application.Common.Interfaces.Repositories;
using BoostStudio.Application.Formats.PsarcFormat;
using BoostStudio.Domain.Enums;

namespace BoostStudio.Application.Exvs.Psarc.Commands;

public record UnpackPsarcByPatchFilesCommand(
    PatchFileVersion[]? PatchFileVersions = null
) : IRequest;

public class UnpackPsarcPatchFilesCommandHandler(
    IMediator mediator,
    IConfigsRepository configsRepository
) : IRequestHandler<UnpackPsarcByPatchFilesCommand>
{
    public async ValueTask<Unit> Handle(UnpackPsarcByPatchFilesCommand request, CancellationToken cancellationToken)
    {
        var productionDirectoryConfig = await configsRepository.GetConfig(ConfigKeys.ProductionDirectory, cancellationToken);
        if (productionDirectoryConfig.IsError)
            throw new NotFoundException(ConfigKeys.ProductionDirectory, productionDirectoryConfig.FirstError.Description);

        var stagingDirectoryConfig = await configsRepository.GetConfig(ConfigKeys.StagingDirectory, cancellationToken);
        if (stagingDirectoryConfig.IsError)
            throw new NotFoundException(ConfigKeys.StagingDirectory, stagingDirectoryConfig.FirstError.Description);

        foreach (var patchFileVersion in request.PatchFileVersions ?? [])
        {
            var patchFileName = patchFileVersion.GetPatchName();

            var sourceFilePath = Path.Combine(productionDirectoryConfig.Value.Value, $"{patchFileName}.psarc");
            if (!File.Exists(sourceFilePath))
                throw new NotFoundException(nameof(sourceFilePath), sourceFilePath);
            
            // if clean staging directory and unpack stuff here
            var destinationDirectory = Path.Combine(stagingDirectoryConfig.Value.Value, "psarc", patchFileName);

            // Unsafe for now, should be removed after migrating to Alist
            if (Directory.Exists(destinationDirectory))
                Directory.Delete(destinationDirectory, true);

            if (!Directory.Exists(destinationDirectory))
                Directory.CreateDirectory(destinationDirectory);

            await mediator.Send(
                new UnpackPsarcByPathCommand(sourceFilePath, destinationDirectory),
                cancellationToken
            );
        }

        return default;
    }
}
