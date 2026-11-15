using System.Text.Json.Serialization;

namespace Debianet.Domain.Dto;

internal sealed class DockerImageResult
{
    [JsonPropertyName("Containers")]
    public required string Containers { get; set; }
    [JsonPropertyName("CreatedAt")]
    public required string CreatedAt { get; set; }
    [JsonPropertyName("CreatedSince")]
    public required string CreatedSince { get; set; }
    [JsonPropertyName("Digest")]
    public required string Digest { get; set; }
    [JsonPropertyName("ID")]
    public required string ID { get; set; }
    [JsonPropertyName("Repository")]
    public required string Repository { get; set; }
    [JsonPropertyName("SharedSize")]
    public required string SharedSize { get; set; }
    [JsonPropertyName("Size")]
    public required string Size { get; set; }
    [JsonPropertyName("Tag")]
    public required string Tag { get; set; }
    [JsonPropertyName("UniqueSize")]
    public required string UniqueSize { get; set; }
}

