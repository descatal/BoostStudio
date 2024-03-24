using System.CommandLine;
using System.CommandLine.Invocation;
using System.Formats.Tar;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Formats.PsarcFormat;
using MediatR;

namespace Console.Commands.Psarc;

public class UnpackPsarcRequest : Command
{
    // TODO add option to supply directory paths
    public UnpackPsarcRequest() : base(name: "unpack", "Unpack psarc file into directory.")
    {
        AddOption(new Option<string>(["--input", "-i"], "Input file path. Required.")
        {
            IsRequired = true
        });
        AddOption(new Option<string>(["--output", "-o"], "Output directory path. Default: Input file directory."));
    }

    public new class Handler(ISender mediator) : ICommandHandler
    {
        public string Input { get; init; } = string.Empty;

        public string Output { get; init; } = string.Empty;
        
        public async Task<int> InvokeAsync(InvocationContext context)
        {
            await mediator.Send(new UnpackPsarcCommand(Input, Output));
            
            return 0;
        }
    }
}
