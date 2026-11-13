using System.Text.Json;

using Debianet.Domain.DataModel;
using Debianet.ServiceContract;

namespace Debianet.Service;

internal sealed class DebianetDataProvider : IDebianetDataProvider
{
    private readonly DebianetDataProvider _data;

    public DebianetDataProvider(IEmbededdFileProvider embededdFileProvider)
    {
        Stream dataStream = embededdFileProvider.GetDataJson();
        _data = JsonSerializer.Deserialize<DebianetDataProvider>(dataStream)
            ?? throw new InvalidOperationException("Failed to deserialize DebianetData from the provided stream.");
    }

    public IReadOnlyList<DotnetTool> DotnetTools
        => _data.DotnetTools;

    public IReadOnlyList<DockerTool> DockerTools
        => _data.DockerTools;
}
