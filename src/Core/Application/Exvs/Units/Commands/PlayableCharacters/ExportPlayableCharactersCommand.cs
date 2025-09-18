using Ardalis.GuardClauses;
using BoostStudio.Application.Common.Constants;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.BinarySerializers;
using BoostStudio.Application.Common.Interfaces.Repositories;
using BoostStudio.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FileInfo = BoostStudio.Application.Common.Models.FileInfo;

namespace BoostStudio.Application.Exvs.Units.Commands.PlayableCharacters;

public record ExportPlayableCharactersCommand(bool ReplaceWorking = false) : IRequest<FileInfo>;

public class ExportPlayableCharactersCommandHandler(
    IConfigsRepository configsRepository,
    IApplicationDbContext applicationDbContext,
    IListInfoBinarySerializer binarySerializer,
    ILogger<ExportPlayableCharactersCommandHandler> logger
) : IRequestHandler<ExportPlayableCharactersCommand, FileInfo>
{
    public async ValueTask<FileInfo> Handle(
        ExportPlayableCharactersCommand command,
        CancellationToken cancellationToken
    )
    {
        var workingDirectory = await configsRepository.GetConfig(
            ConfigKeys.WorkingDirectory,
            cancellationToken
        );
        if (
            command.ReplaceWorking
            && (workingDirectory.IsError || string.IsNullOrWhiteSpace(workingDirectory.Value.Value))
        )
            throw new NotFoundException(
                ConfigKeys.WorkingDirectory,
                workingDirectory.FirstError.Description
            );

        var query = applicationDbContext
            .Units.Include(entity => entity.PlayableCharacter)
            .Include(entity => entity.AssetFiles)
            .AsQueryable();

        var units = await query.ToListAsync(cancellationToken);

        List<uint> foo =
        [
            1011,
            1021,
            1031,
            1041,
            1051,
            1071,
            1081,
            1091,
            1101,
            1131,
            2011,
            2021,
            2031,
            2041,
            2051,
            2061,
            2091,
            3011,
            3021,
            3031,
            3041,
            3051,
            3061,
            3071,
            3091,
            4011,
            4021,
            5011,
            5021,
            5031,
            5041,
            5111,
            7011,
            7021,
            7031,
            7041,
            8011,
            8021,
            8031,
            10011,
            10021,
            10031,
            10041,
            10061,
            12011,
            12021,
            13011,
            13021,
            13031,
            13041,
            13051,
            14011,
            14021,
            14031,
            14041,
            14051,
            14061,
            14071,
            14081,
            14091,
            14111,
            14121,
            14131,
            15011,
            15021,
            15031,
            15041,
            15051,
            15061,
            15081,
            15091,
            16011,
            16021,
            16031,
            16041,
            17011,
            17021,
            17031,
            18011,
            18021,
            18031,
            18041,
            18051,
            20011,
            20021,
            20031,
            20041,
            20051,
            20061,
            20091,
            21011,
            21021,
            21031,
            21041,
            21051,
            21061,
            21081,
            21091,
            21101,
            21121,
            22011,
            22021,
            22031,
            22081,
            22091,
            23011,
            23021,
            23031,
            23041,
            24011,
            24021,
            25011,
            25021,
            26011,
            27011,
            27021,
            27061,
            28011,
            28021,
            28031,
            28091,
            28101,
            28131,
            29011,
            29021,
            30011,
            31011,
            31021,
            34011,
            34021,
            34031,
            1061,
            33011,
            33021,
            3121,
            3131,
            20081,
            21071,
            22051,
            22041,
            22061,
            27051,
            27031,
            28111,
            28121,
            34041,
            5131,
            21111,
            21131,
            2101,
            14141,
            18061,
            18071,
            20101,
            23051,
            27041,
            33031,
            33041,
            33051,
            33061,
            26021,
            33071,
            12041,
            21151,
            33081,
            34051,
            42011,
            42021,
            42031,
            43011,
            17041,
            42041,
            46011,
            2161,
            43041,
            46021,
            20111,
            52011,
            49011,
            46031,
            49021,
            49031,
            51011,
            42051,
            49041,
            51021,
            80011,
            80031,
            80032,
            80033,
            80021,
            80041,
            80051,
            80052,
            80061,
            80081,
            80091,
            80101,
            80111,
            80131,
            80141,
            80151,
            80161,
            80171,
            80201,
            80221,
            80231,
            80241,
            80251,
            80261,
            80271,
            80281,
            80291,
            80311,
            80321,
            80331,
            80341,
            80351,
            80361,
            80371,
            80381,
            80301,
            60011,
            60021,
            70421,
            70581,
            70361,
            60091,
            70061,
            70391,
            16211,
            16111,
        ];

        units = units.OrderBy(x => foo.IndexOf(x.GameUnitId)).ToList();

        var serializedBytes = await binarySerializer.SerializePlayableCharactersAsync(
            units,
            cancellationToken
        );

        if (command.ReplaceWorking)
        {
            var workingFilePath = Path.Combine(
                workingDirectory.Value.Value,
                WorkingDirectoryConstants.CommonDirectory,
                AssetFileType.ListInfo.GetSnakeCaseName(),
                "001.bin"
            );
            await File.WriteAllBytesAsync(workingFilePath, serializedBytes, cancellationToken);
        }

        var fileName = Path.ChangeExtension("playablecharacters", ".listinfo");
        return new FileInfo(serializedBytes, fileName);
    }
}
