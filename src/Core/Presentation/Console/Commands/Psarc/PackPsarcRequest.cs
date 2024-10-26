using System.ComponentModel.DataAnnotations;
using BoostStudio.Application.Formats.PsarcFormat;
using BoostStudio.Domain.Entities.PsarcFormat;
using Mediator;

namespace Console.Commands.Psarc;

public class PackPsarcRequest(IMediator mediator)
{
    /// <summary>
    /// Pack directory into psarc binary
    /// </summary>
    /// <param name="input">Input directory path</param>
    /// <param name="compression">Compression type, supported types are 'None', 'Zlib' and 'Lzma'.</param>
    /// <param name="compressionLevel">Compression level, ranging from 1 - 9, only applicable if compression type is either Zlib or Lzma.</param>
    /// <param name="output">Output directory. Defaults to input file's directory</param>
    /// <param name="fileName">Output filename. Defaults to input file's directory name</param>
    public async Task<int> Pack(
        string input, 
        CompressionType compression = CompressionType.Zlib, 
        [Range(0, 9)] int compressionLevel = 9, 
        string? output = null, 
        string? fileName = null)
    {
        var packPsarcCommand = new PackPsarcByPathCommand
         {
             SourcePath = input,
             DestinationPath = output ?? string.Empty,
             CompressionType = compression,
             CompressionLevel = compressionLevel,
             Filename = fileName
         };
        
        await mediator.Send(packPsarcCommand);

        return 0;
    }
}

// public class PackPsarcRequest : Command
// {
//     // TODO add option to supply directory paths
//     public PackPsarcRequest() : base(name: "pack", "Pack directory into psarc format.")
//     {
//         AddOption(new Option<string>(["--input", "-i"], "Input directory path. Required.")
//         {
//             IsRequired = true
//         });
//         AddOption(new Option<CompressionType>(["--compression", "-c"], "Compression type, supported types are 'None', 'Zlib' and 'Lzma'. Default: Zlib"));
//         AddOption(new Option<int>(["--compressionLevel", "-cl"], "Compression level, ranging from 1 - 9, only applicable if compression type is either Zlib or Lzma. Default: 9"));
//         AddOption(new Option<string>(["--output", "-o"], "Output directory. Default: Input directory."));
//         AddOption(new Option<string>(["--filename", "-f"], "Output filename. Default: Input directory name."));
//     }
//
//     public new class Handler(IMediator mediator) : ICommandHandler
//     {
//         private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
//
//         public string Input { get; set; } = string.Empty;
//
//         public CompressionType Compression { get; set; } = CompressionType.None;
//
//         public int CompressionLevel { get; set; }
//
//         public string Output { get; set; } = string.Empty;
//
//         public string Filename { get; set; } = string.Empty;
//
//         public async Task<int> InvokeAsync(InvocationContext context)
//         {
//             var packPsarcCommand = new PackPsarcCommand
//             {
//                 SourcePath = Input,
//                 DestinationPath = Output,
//                 CompressionType = Compression,
//                 CompressionLevel = CompressionLevel,
//                 Filename = Filename
//             };
//
//             await _mediator.Send(packPsarcCommand);
//             
//             return 0;
//         }
//     }
// }
