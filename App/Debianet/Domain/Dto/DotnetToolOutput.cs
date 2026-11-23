using System.Text.Json.Serialization;

namespace Debianet.Domain.Dto;

public class DotnetToolOutput
{
    [JsonPropertyName("packageId")]
    public required string PackageId { get; set; }
    [JsonPropertyName("version")]
    public required string Version { get; set; }
    [JsonPropertyName("commands")]
    public required string[] Commands { get; set; }

    [JsonIgnore]
    public string CommandsString
        => string.Join(',', Commands);
}
