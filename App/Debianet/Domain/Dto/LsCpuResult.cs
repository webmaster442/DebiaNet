using System.Text.Json.Serialization;

namespace Debianet.Domain.Dto;

internal sealed class LsCpuResult
{
    [JsonPropertyName("lscpu")]
    public required FieldData[] Lscpu { get; init; }

    public class FieldData
    {
        [JsonPropertyName("field")]
        public required string Field { get; init; }

        [JsonPropertyName("data")]
        public required string Data { get; init; }

        public override string ToString()
        {
            return $"{Field}: {Data}";
        }
    }

    public const string ModelName = "Model name:";
}
