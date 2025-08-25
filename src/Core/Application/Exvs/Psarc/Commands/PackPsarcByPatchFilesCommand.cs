using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Constants;
using BoostStudio.Application.Common.Interfaces.Repositories;
using BoostStudio.Domain.Enums;
using FileInfo = BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Psarc.Commands;

public record PackPsarcByPatchFilesCommand(PatchFileVersion[]? PatchFileVersions = null) : IRequest;

public class PackPsarcPatchFilesCommandHandler(
    IMediator mediator,
    IConfigsRepository configsRepository
) : IRequestHandler<PackPsarcByPatchFilesCommand>
{
    public async ValueTask<Unit> Handle(
        PackPsarcByPatchFilesCommand request,
        CancellationToken cancellationToken
    )
    {
        var stagingDirectoryConfig = await configsRepository.GetConfig(
            ConfigKeys.StagingDirectory,
            cancellationToken
        );
        if (stagingDirectoryConfig.IsError)
            throw new NotFoundException(
                ConfigKeys.StagingDirectory,
                stagingDirectoryConfig.FirstError.Description
            );

        var productionDirectoryConfig = await configsRepository.GetConfig(
            ConfigKeys.ProductionDirectory,
            cancellationToken
        );
        if (productionDirectoryConfig.IsError)
            throw new NotFoundException(
                ConfigKeys.ProductionDirectory,
                productionDirectoryConfig.FirstError.Description
            );

        foreach (var patchFileVersion in request.PatchFileVersions ?? [])
        {
            var patchFileName = patchFileVersion.GetPatchName();

            var sourceDirectory = Path.Combine(
                stagingDirectoryConfig.Value.Value,
                "psarc",
                patchFileName
            );
            if (!Directory.Exists(sourceDirectory))
                throw new NotFoundException(nameof(sourceDirectory), sourceDirectory);

            string destinationDirectory = productionDirectoryConfig.Value.Value;

            if (!Directory.Exists(destinationDirectory))
                Directory.CreateDirectory(destinationDirectory);

            await mediator.Send(
                new PackPsarcByPathCommand
                {
                    SourcePath = sourceDirectory,
                    DestinationPath = destinationDirectory,
                    Filename = $"{patchFileName}.psarc",
                },
                cancellationToken
            );
        }

        return default;
    }
}
