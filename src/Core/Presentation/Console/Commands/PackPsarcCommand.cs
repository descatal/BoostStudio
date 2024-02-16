using System.CommandLine;
using System.CommandLine.Invocation;
using BoostStudio.Application.Formats.PsarcFormat;
using BoostStudio.Domain.Entities.PsarcFormat;
using MediatR;

namespace Console.Commands;

public class PackPsarcCommand : Command
{
    // TODO add option to supply directory paths
    public PackPsarcCommand() : base(name: "pack", "Pack directory into psarc format.")
    {
        AddOption(new Option<string>(["--input", "-i"], "Input directory path.")
        {
            IsRequired = true
        });
        AddOption(new Option<CompressionType>(["--compression", "-c"], "Compression type, supported types are 'None', 'Zlib' and 'Lzma'. Default: Zlib"));
        AddOption(new Option<int>(["--compressionLevel", "-cl"], "Compression level, ranging from 1 - 9, only applicable if compression type is either Zlib or Lzma. Default: 9"));
        AddOption(new Option<string>(["--output", "-o"], "Output directory."));
        AddOption(new Option<string>(["--filename", "-f"], "Output filename."));
    }

    public new class Handler(IMediator mediator) : ICommandHandler
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        public string Input { get; set; } = string.Empty;

        public CompressionType Compression { get; set; } = CompressionType.None;

        public int CompressionLevel { get; set; }

        public string Output { get; set; } = string.Empty;

        public string Filename { get; set; } = string.Empty;

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            if (string.IsNullOrWhiteSpace(Input))
                Input = AppContext.BaseDirectory;

            if (!Directory.Exists(Input))
                return 0;

            var psarc = await _mediator.Send(new PackPsarc
            {
                SourcePath = Input, CompressionType = Compression, CompressionLevel = CompressionLevel,
            });

            var outputFileName = string.IsNullOrWhiteSpace(Filename)
                ? Path.GetFileNameWithoutExtension(Input)
                : Filename;

            var outputDirectory = string.IsNullOrWhiteSpace(Output)
                ? Directory.GetCurrentDirectory()
                : Output;

            var outputFilePath = Path.Combine(outputDirectory, Path.ChangeExtension(outputFileName, ".psarc"));

            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            await File.WriteAllBytesAsync(outputFilePath, psarc);
            return 0;
        }
    }
}
