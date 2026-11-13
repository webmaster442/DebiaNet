using System.Text.Json.Serialization;

namespace Debianet.Domain.DataModel;

public class DotnetTool
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    [JsonPropertyName("description")]
    public required string Description { get; set; }
    [JsonPropertyName("link")]
    public required string Link { get; set; }
}
