using System.Buffers.Binary;
using System.Diagnostics;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Reloaded.Memory;

namespace BoostStudio.Application.Scex.Commands;

public record HotReloadScex(string SourcePath) : IRequest;

#pragma warning disable CA1416

public class HotReloadScexCommandHandler(
    ILogger<HotReloadScexCommandHandler> logger    
) : IRequestHandler<HotReloadScex>
{
    public async ValueTask<Unit> Handle(HotReloadScex request, CancellationToken cancellationToken)
    {
        const long mapRegionPointer = 0x300000000;
        
        if (!File.Exists(request.SourcePath))
            return default;

        var compiledByteCode = await File.ReadAllBytesAsync(request.SourcePath, cancellationToken);
        
        using var rpcs3Process = Process.GetProcessesByName("rpcs3").FirstOrDefault();

        if (rpcs3Process is null)
            throw new NotFoundException("No process with name 'rpcs3' was found", "process");
        
        var rpcs3Memory = new ExternalMemory(rpcs3Process);
        var unitPointer = rpcs3Memory.Read<uint>((UIntPtr)(mapRegionPointer + 0x40091000));
        unitPointer = BinaryPrimitives.ReverseEndianness(unitPointer);
        var scexPointer = rpcs3Memory.Read<uint>((UIntPtr)(mapRegionPointer + unitPointer + 0x17358));
        scexPointer = BinaryPrimitives.ReverseEndianness(scexPointer);
        
        rpcs3Memory.WriteRaw((UIntPtr)(mapRegionPointer + scexPointer), compiledByteCode);

        return default;
    }
}
#pragma warning restore CA1416

