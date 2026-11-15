using Debianet.Domain;

namespace Debianet.ServiceContract;

internal interface IDotnetToolService
{
    CommandBundle CreateInstallBundle(IEnumerable<string> toolsToInstall, IEnumerable<string> toolsToUninstall);
    Task<IReadOnlySet<string>> GetInstalledToolsAsync();
}
