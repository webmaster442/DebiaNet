using System.Text.Json.Serialization;

namespace Debianet.Domain.Dto;

internal sealed class DockerVolumeResult
{
    [JsonPropertyName("Availability")]
    public required string Availability { get; set; }
    [JsonPropertyName("Driver")]
    public required string Driver { get; set; }
    [JsonPropertyName("Group")]
    public required string Group { get; set; }
    [JsonPropertyName("Labels")]
    public required string Labels { get; set; }
    [JsonPropertyName("Links")]
    public required string Links { get; set; }
    [JsonPropertyName("Mountpoint")]
    public required string Mountpoint { get; set; }
    [JsonPropertyName("Name")]
    public required string Name { get; set; }
    [JsonPropertyName("Scope")]
    public required string Scope { get; set; }
    [JsonPropertyName("Size")]
    public required string Size { get; set; }
    [JsonPropertyName("Status")]
    public required string Status { get; set; }
}

