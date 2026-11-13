using System.Text.Json.Serialization;

namespace Debianet.Domain.DataModel;

public class DebianetData
{
    [JsonPropertyName("dotnetTools")]
    public required DotnetTool[] DotnetTools { get; set; }

    [JsonPropertyName("dockerTools")]
    public required DockerTool[] DockerTools { get; set; }
}
