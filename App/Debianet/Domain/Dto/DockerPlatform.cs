using System.Text.Json.Serialization;

namespace Debianet.Domain.Dto;

internal sealed class DockerPlatform
{
    [JsonPropertyName("architecture")]
    public required string Architecture { get; set; }
    [JsonPropertyName("os")]
    public required string Os { get; set; }
}

