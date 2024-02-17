using System.CommandLine;
using Console.Commands;
using Console.Commands.Psarc;

namespace Console;

public static class RootCommandExtensions
{
    public static RootCommand AddRootCommands(this RootCommand root)
    {
        root.AddCommand(
            new Command("fhm", "Fhm operations")
            {
                new PackFhmCommand(),
            });

        root.AddCommand(
            new Command("psarc", "Psarc operations")
            {
                new PackPsarcCommand(),
                new UnpackPsarcCommand(),
            });
        
        return root;
    }
}
