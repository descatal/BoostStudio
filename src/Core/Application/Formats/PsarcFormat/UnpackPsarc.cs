using System.ComponentModel.DataAnnotations;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Common.Interfaces.Formats.PsarcFormat;
using BoostStudio.Domain.Entities.PsarcFormat;

namespace BoostStudio.Application.Formats.PsarcFormat;

public record UnpackPsarc(byte[] File) : IRequest<byte[]>
{
    [Required]
    public byte[] File { get; } = File;
}

public class UnpackPsarcCommandHandler(IPsarcPacker psarcPacker) : IRequestHandler<UnpackPsarc, byte[]>
{
    public async Task<byte[]> Handle(UnpackPsarc request, CancellationToken cancellationToken)
    {
        return await psarcPacker.UnpackAsync(request.File, cancellationToken);
    }
}
