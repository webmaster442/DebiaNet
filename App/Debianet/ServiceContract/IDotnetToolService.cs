using Debianet.Domain;

namespace Debianet.ServiceContract;

internal interface IDotnetToolService
{
    CommandBundle CreateInstallBundle(IEnumerable<string> toolsToInstall, IEnumerable<string> toolsToUninstall);
    Task<Result<IReadOnlySet<string>>> GetInstalledToolsAsync(CancellationToken cancellationToken);
}
