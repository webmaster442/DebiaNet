using Debianet.Domain;
using Debianet.Domain.Dto;

namespace Debianet.ServiceContract;

internal interface IDotnetToolService
{
    CommandBundle CreateInstallBundle(IEnumerable<string> toolsToInstall, IEnumerable<string> toolsToUninstall);
    Task<Result<IReadOnlySet<string>>> GetInstalledToolsAsync(CancellationToken cancellationToken);
    Task<Result<DotnetToolList>> GetDotnetToolListAsync(CancellationToken cancellationToken);
}
