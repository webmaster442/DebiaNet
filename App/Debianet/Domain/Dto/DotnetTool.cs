using System.Text.Json.Serialization;

namespace Debianet.Domain.Dto;

public class DotnetTool
{
    [JsonPropertyName("packageId")]
    public required string PackageId { get; set; }
    [JsonPropertyName("version")]
    public required string Version { get; set; }
    [JsonPropertyName("commands")]
    public required string[] Commands { get; set; }
}
