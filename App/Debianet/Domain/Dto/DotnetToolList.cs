using System.Text.Json.Serialization;

namespace Debianet.Domain.Dto;


public class DotnetToolList
{
    [JsonPropertyName("version")]
    public required int Version { get; set; }
    [JsonPropertyName("data")]
    public required DotnetTool[] Data { get; set; }
}
