using System.Reflection;

namespace BoostStudio.Infrastructure.Common;

public static class Utils
{
    public static string InitializeExecutable(
        string workingDirectory,
        string executableName,
        string resourceName)
    {
        if (!Directory.Exists(workingDirectory))
            Directory.CreateDirectory(workingDirectory);
        
        var workingPath = Path.Combine(workingDirectory, executableName);
        
        // Extracting executable from resource to a temp location.
        using var psarcResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

        if (psarcResourceStream is null)
            throw new FileNotFoundException($"{executableName} resource not found.");

        using var fileStream = File.Create(workingPath);
        psarcResourceStream.CopyTo(fileStream);
        fileStream.Close();

        return workingPath;
    }
}
