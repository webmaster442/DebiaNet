namespace Debianet.ServiceContract;

internal interface IDotnetToolService
{
    Task<IReadOnlyList<string>> GetInstalledToolsAsync();
}
