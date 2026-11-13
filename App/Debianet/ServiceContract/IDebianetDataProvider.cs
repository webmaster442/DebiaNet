using Debianet.Domain.DataModel;

namespace Debianet.ServiceContract;

internal interface IDebianetDataProvider
{
    IReadOnlyList<DotnetTool> DotnetTools { get; }
    IReadOnlyList<DockerTool> DockerTools { get; }
}