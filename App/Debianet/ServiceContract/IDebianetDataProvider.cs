using Debianet.Domain.DataModel;

namespace Debianet.ServiceContract;

internal interface IDebianetDataProvider
{
    IEnumerable<DotnetTool> DotnetTools { get; }
    IEnumerable<DockerTool> DockerTools { get; }
}