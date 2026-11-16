using System.Text.Json.Serialization;

namespace Debianet.Domain.DataModel;

public class DockerTool
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("compose")]
    public required string Compose { get; set; }

    [JsonPropertyName("folders")]
    public required string[] Folders { get; set; }

    [JsonPropertyName("link")]
    public required string Link { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }
}
