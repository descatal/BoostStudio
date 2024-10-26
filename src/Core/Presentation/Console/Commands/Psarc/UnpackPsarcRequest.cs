using BoostStudio.Application.Formats.PsarcFormat;
using Mediator;

namespace Console.Commands.Psarc;

public class UnpackPsarcRequest(IMediator mediator)
{
    /// <summary>
    /// Unpack psarc file into directory.
    /// </summary>
    /// <param name="input">Input psarc binary file path.</param>
    /// <param name="output">Output directory path.</param>
    public async Task<int> Unpack(
        string input, 
        string? output = null)
    {
        await mediator.Send(new UnpackPsarcByPathCommand(input, output ?? string.Empty));
        return 0;
    }
}

// public class UnpackPsarcRequest : Command
// {
//     // TODO add option to supply directory paths
//     public UnpackPsarcRequest() : base(name: "unpack", "Unpack psarc file into directory.")
//     {
//         AddOption(new Option<string>(["--input", "-i"], "Input file path. Required.")
//         {
//             IsRequired = true
//         });
//         AddOption(new Option<string>(["--output", "-o"], "Output directory path. Default: Input file directory."));
//     }
//
//     public new class Handler(ISender mediator) : ICommandHandler
//     {
//         public string Input { get; init; } = string.Empty;
//
//         public string Output { get; init; } = string.Empty;
//         
//         public async Task<int> InvokeAsync(InvocationContext context)
//         {
//             await mediator.Send(new UnpackPsarcCommand(Input, Output));
//             
//             return 0;
//         }
//     }
// }
