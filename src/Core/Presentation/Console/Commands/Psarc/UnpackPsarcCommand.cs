using System.CommandLine;
using System.CommandLine.Invocation;
using System.Formats.Tar;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Application.Formats.PsarcFormat;
using MediatR;

namespace Console.Commands.Psarc;

public class UnpackPsarcCommand : Command
{
    // TODO add option to supply directory paths
    public UnpackPsarcCommand() : base(name: "unpack", "Unpack psarc file into directory.")
    {
        AddOption(new Option<string>(["--input", "-i"], "Input file path.")
        {
            IsRequired = true
        });
        AddOption(new Option<string>(["--output", "-o"], "Output directory path."));
    }

    public new class Handler(IMediator mediator) : ICommandHandler
    {
        public string Input { get; set; } = string.Empty;

        public string Output { get; set; } = string.Empty;
        
        
        public async Task<int> InvokeAsync(InvocationContext context)
        {
            if (!File.Exists(Input))
                throw new FileNotFoundException();

            var sourceFile = await File.ReadAllBytesAsync(Input);
            
            if (string.IsNullOrWhiteSpace(Output))
                Output = AppContext.BaseDirectory;
            
            var psarc = await mediator.Send(new UnpackPsarc(sourceFile));

            var outputDirectory = string.IsNullOrWhiteSpace(Output)
                ? Directory.GetCurrentDirectory()
                : Output;
            
            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            await using var psarcStream = new MemoryStream(psarc);
            await TarFile.ExtractToDirectoryAsync(psarcStream, outputDirectory, true);

            return 0;
        }
    }
}
