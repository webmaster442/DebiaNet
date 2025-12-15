using System.Text.Json.Serialization;

namespace DebiaNetApp.Dto;

internal record class DockerPlatform
{
    [JsonPropertyName("architecture")]
    public required string Architecture { get; init; }

    [JsonPropertyName("os")]
    public required string Os { get; init; }
}