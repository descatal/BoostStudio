// using System.CommandLine;
// using System.CommandLine.Invocation;
// using BoostStudio.Application.Formats.FhmFormat.Commands;
// using Mediator;
//
// namespace Console.Commands;
//
// public class PackFhmCommand : Command
// {
//     // TODO add option to supply directory paths
//     public PackFhmCommand() : base(name: "pack", "Pack archived file into Fhm format.")
//     {
//         AddOption(new Option<string>(["--input", "-i"], "Archived file path that contains the files to be packed into Fhm format. Supports tar / zip / 7zip archived format."));
//         AddOption(new Option<string?>(["--output", "-o"], "Output directory."));
//         AddOption(new Option<string?>(["--file", "-f"], "Output file name."));
//     }
//
//     public new class Handler(IMediator mediator, string inputPath, string? outputPath = null, string? fileName = null) : ICommandHandler
//     {
//         private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
//
//         public int Invoke(InvocationContext context)
//         {
//             throw new NotImplementedException();
//         }
//         
//         public async Task<int> InvokeAsync(InvocationContext context)
//         {
//             if (!File.Exists(inputPath))
//                 return 0;
//
//             var fileBytes = await File.ReadAllBytesAsync(inputPath);
//             var fhm = await _mediator.Send(new PackFhm(fileBytes));
//
//             var outputFileName = string.IsNullOrWhiteSpace(fileName) 
//                 ? Path.GetFileNameWithoutExtension(inputPath) 
//                 : fileName;
//             
//             var outputDirectory = string.IsNullOrWhiteSpace(outputPath)
//                 ? Path.GetDirectoryName(inputPath) ?? Directory.GetCurrentDirectory()
//                 : outputPath;
//             
//             var outputFilePath = Path.Combine(outputDirectory, outputFileName);
//             
//             if (!Directory.Exists(outputDirectory))
//                 Directory.CreateDirectory(outputDirectory);
//             
//             await File.WriteAllBytesAsync(outputFilePath, fhm);
//             return 0;
//         }
//     }
// }
