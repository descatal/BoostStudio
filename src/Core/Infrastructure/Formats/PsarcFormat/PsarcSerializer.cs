using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using BoostStudio.Application.Common.Interfaces;
using BoostStudio.Domain.Entities.PsarcFormat;
using Microsoft.Extensions.Logging;
using CompressionType=BoostStudio.Domain.Entities.PsarcFormat.CompressionType;

namespace BoostStudio.Infrastructure.Formats.PsarcFormat;

// TODO rework to Kaitai struct implementation
public class PsarcSerializer(ILogger<PsarcSerializer> logger) : IFormatSerializer<Psarc>
{
    public Task<Psarc> DeserializeAsync(Stream data, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Psarc());
    }

    public async Task<byte[]> SerializeAsync(Psarc data, CancellationToken cancellationToken)
    {
        var tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempPath);
        
        try
        {
            return await PackFhm(data, tempPath, cancellationToken);
        }
        finally
        {
            // Cleanup regardless
            Directory.Delete(tempPath, true);
        }
    }

    private async Task<byte[]> PackFhm(Psarc data, string workingDirectory, CancellationToken cancellationToken)
    {
        if (!Directory.Exists(data.SourcePath))
            return Array.Empty<byte>();

        var tempPsarcExePath = Path.Combine(workingDirectory, "psarc.exe");
        var tempMetadataPath = Path.Combine(workingDirectory, "metadata.xml");
        var tempPsarcOutputPath = Path.Combine(workingDirectory, "output.psarc");

        // Extracting executable from resource to a temp location.
        await using var psarcResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BoostStudio.Infrastructure.Resources.psarc.exe");

        if (psarcResourceStream is null)
            return Array.Empty<byte>();

        await using var fileStream = File.Create(tempPsarcExePath);
        await psarcResourceStream.CopyToAsync(fileStream, cancellationToken);
        fileStream.Close();
        
        // Construct metadata xml: adding all of the files to be packed.
        var fileSystemsPath = Directory.GetFiles(data.SourcePath, "*", SearchOption.AllDirectories);
        
        var create = new XElement("create",
            new XAttribute("overwrite", "true"),
            new XAttribute("archive", $"{tempPsarcOutputPath}"));

        if (data.CompressionType != CompressionType.None)
        {
            create.Add(new XElement("compression",
                new XAttribute("type", $"{data.CompressionType.ToString().ToLower()}"),
                new XAttribute("level", $"{data.CompressionLevel}"),
                new XAttribute("enabled", "true")));
        }

        create.Add(fileSystemsPath.Select(path => new XElement("file", new XAttribute("path", path))));
        
        var metadataDoc = new XDocument(
            new XDeclaration("1.0", "UTF-8", null),
            new XElement("psarc", create));

        await using var writer = XmlWriter.Create(tempMetadataPath, new XmlWriterSettings
        {
            Encoding = new UTF8Encoding(false), Async = true
        });
        await metadataDoc.SaveAsync(writer, cancellationToken);
        writer.Close();

        // Execute process
        using var psarcProcess = new Process();
        psarcProcess.StartInfo = new ProcessStartInfo
        {
            Arguments = $"create --xml {tempMetadataPath}",
            CreateNoWindow = true,
            FileName = tempPsarcExePath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        psarcProcess.Start();

        // Synchronously read the standard output of the spawned process.
        var reader = psarcProcess.StandardOutput;
        var output = await reader.ReadToEndAsync(cancellationToken);

        logger.LogInformation("{}", output);
        await psarcProcess.WaitForExitAsync(cancellationToken);

        if (!File.Exists(tempPsarcOutputPath))
            throw new Exception($"{output}");

        return await File.ReadAllBytesAsync(tempPsarcOutputPath, cancellationToken);
    }
}
