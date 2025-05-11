using System.Buffers.Binary;
using System.Diagnostics;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Reloaded.Memory;
using Reloaded.Memory.Interfaces;

namespace BoostStudio.Web.Endpoints.Exvs.Sse;

public class Sse : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.Map("/sse/game-info", Handler);
    }

    private async Task<ServerSentEventsResult<GameInfo>> Handler(
        CancellationToken cancellationToken
    )
    {
        async IAsyncEnumerable<SseItem<GameInfo>> GetEnemyHealth(
            [EnumeratorCancellation] CancellationToken ct
        )
        {
            const long mapRegionPointer = 0x300000000;
            using var rpcs3Process = Process.GetProcessesByName("rpcs3").FirstOrDefault();

            if (rpcs3Process is null)
                throw new NotFoundException("No process with name 'rpcs3' was found", "process");

#pragma warning disable CA1416

            var rpcs3Memory = new ExternalMemory(rpcs3Process);
            // check if game is opened
            var enemyUnit = rpcs3Memory.Read<uint>((UIntPtr)(mapRegionPointer + 0x40091000));
            enemyUnit = BinaryPrimitives.ReverseEndianness(enemyUnit);

            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(1000, ct);

                var enemyHealth = rpcs3Memory.Read<uint>(
                    (UIntPtr)(mapRegionPointer + enemyUnit + 0x164)
                );
                var enemyExBytes = rpcs3Memory.ReadRaw(
                    (UIntPtr)(mapRegionPointer + enemyUnit + 0x9D8),
                    4
                );
                enemyHealth = BinaryPrimitives.ReverseEndianness(enemyHealth);
                enemyExBytes = enemyExBytes.Reverse().ToArray();
                var enemyEx = BitConverter.ToSingle(enemyExBytes);

                yield return new SseItem<GameInfo>(
                    new GameInfo((int)enemyHealth, enemyEx),
                    "game-info"
                );
            }
        }

        return TypedResults.ServerSentEvents(GetEnemyHealth(cancellationToken));
    }
}

public record GameInfo(int EnemyHealth, float EnemyEx);
