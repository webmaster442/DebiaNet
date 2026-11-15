using System.Text.Json;

using Debianet.Domain.DataModel;
using Debianet.ServiceContract;

namespace Debianet.Service;

internal sealed class DebianetDataProvider : IDebianetDataProvider
{
    private readonly DebianetData _data;

    public DebianetDataProvider(IEmbededdFileProvider embededdFileProvider)
    {
        Stream dataStream = embededdFileProvider.GetDataJson();
        _data = JsonSerializer.Deserialize<DebianetData>(dataStream)
            ?? throw new InvalidOperationException("Failed to deserialize DebianetData from the provided stream.");
    }

    public IEnumerable<DotnetTool> DotnetTools
        => _data.DotnetTools.OrderBy(x => x.Name);

    public IEnumerable<DockerTool> DockerTools
        => _data.DockerTools.OrderBy(x => x.Name);
}
