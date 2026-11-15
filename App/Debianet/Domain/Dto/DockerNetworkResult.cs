using System.Text.Json.Serialization;

namespace Debianet.Domain.Dto;

internal sealed class DockerNetworkResult
{
    [JsonPropertyName("CreatedAt")]
    public required string CreatedAt { get; set; }
    [JsonPropertyName("Driver")]
    public required string Driver { get; set; }
    [JsonPropertyName("ID")]
    public required string ID { get; set; }
    [JsonPropertyName("IPv4")]
    public required string IPv4 { get; set; }
    [JsonPropertyName("IPv6")]
    public required string IPv6 { get; set; }
    [JsonPropertyName("Internal")]  
    public required string Internal { get; set; }
    [JsonPropertyName("Labels")]
    public required string Labels { get; set; }
    [JsonPropertyName("Name")]
    public required string Name { get; set; }
    [JsonPropertyName("Scope")]
    public required string Scope { get; set; }
}
