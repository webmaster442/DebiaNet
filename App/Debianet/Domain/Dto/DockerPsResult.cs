using System.Text.Json.Serialization;

namespace Debianet.Domain.Dto;

internal sealed class DockerPsResult
{
    [JsonPropertyName("Command")]
    public required string Command { get; set; }
    [JsonPropertyName("CreatedAt")]
    public required string CreatedAt { get; set; }
    [JsonPropertyName("HealthStatus")]
    public required string HealthStatus { get; set; }
    [JsonPropertyName("ID")]
    public required string ID { get; set; }
    [JsonPropertyName("Image")]
    public required string Image { get; set; }
    [JsonPropertyName("Labels")]
    public required string Labels { get; set; }
    [JsonPropertyName("LocalVolumes")]
    public required string LocalVolumes { get; set; }
    [JsonPropertyName("Mounts")]
    public required string Mounts { get; set; }
    [JsonPropertyName("Names")]
    public required string Names { get; set; }
    [JsonPropertyName("Networks")]
    public required string Networks { get; set; }
    [JsonPropertyName("Platform")]
    public required DockerPlatform Platform { get; set; }
    [JsonPropertyName("Ports")]
    public required string Ports { get; set; }
    [JsonPropertyName("RunningFor")]
    public required string RunningFor { get; set; }
    [JsonPropertyName("Size")]
    public required string Size { get; set; }
    [JsonPropertyName("State")]
    public required string State { get; set; }
    [JsonPropertyName("Status")]
    public required string Status { get; set; }
}

