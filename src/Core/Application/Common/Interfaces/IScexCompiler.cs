namespace BoostStudio.Application.Common.Interfaces;

public interface IScexCompiler
{
    public Task CompileAsync(string sourcePath, string destinationPath, CancellationToken cancellationToken = default);
    
    public Task DecompileAsync(string sourcePath, string destinationPath, CancellationToken cancellationToken = default);
}
