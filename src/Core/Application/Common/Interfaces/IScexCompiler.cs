namespace BoostStudio.Application.Common.Interfaces;

public interface IScexCompiler
{
    public Task CompileAsync(string sourcePath, string destinationPath, CancellationToken cancellationToken = default);
}
