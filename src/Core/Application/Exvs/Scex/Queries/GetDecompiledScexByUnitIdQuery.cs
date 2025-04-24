using System.Text.RegularExpressions;
using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Constants;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Exvs.Scex.Queries;

public record GetDecompiledScexByUnitIdQuery(uint UnitId) : IRequest<string>;

public class GetDecompiledScexByUnitIdQueryHandler(
    IConfigsRepository configsRepository,
    IApplicationDbContext applicationDbContext
) : IRequestHandler<GetDecompiledScexByUnitIdQuery, string>
{
    public async ValueTask<string> Handle(
        GetDecompiledScexByUnitIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var scriptDirectoryConfig = await configsRepository.GetConfig(
            ConfigKeys.ScriptDirectory,
            cancellationToken
        );

        if (scriptDirectoryConfig.IsError || !Directory.Exists(scriptDirectoryConfig.Value.Value))
        {
            throw new NotFoundException(
                ConfigKeys.ScriptDirectory,
                scriptDirectoryConfig.FirstError.Description
            );
        }

        var unit = await applicationDbContext.Units.FirstOrDefaultAsync(
            unit => request.UnitId == unit.GameUnitId,
            cancellationToken
        );

        Guard.Against.NotFound(request.UnitId, unit);

        var pathCandidates = Directory.GetFiles(
            scriptDirectoryConfig.Value.Value,
            $"*{unit.GameUnitId}*",
            SearchOption.AllDirectories
        );
        var sourceFilePath = pathCandidates.FirstOrDefault(path =>
            Regex.IsMatch(path, $@"\b{unit.GameUnitId}\b")
        );

        if (sourceFilePath is null || !File.Exists(sourceFilePath))
        {
            throw new NotFoundException(
                nameof(sourceFilePath),
                $"No script file for {unit.NameEnglish} found!"
            );
        }

        return await File.ReadAllTextAsync(sourceFilePath, cancellationToken);
    }
}
