namespace DebiaNetApp.Dto;

internal record class DockerImage : DockerContainer
{
    public required string Containers { get; init; }
    public required string CreatedAt { get; init; }
    public required string CreatedSince { get; init; }
    public required string Digest { get; init; }
    public required string Repository { get; init; }
    public required string SharedSize { get; init; }
    public required string Size { get; init; }
    public required string Tag { get; init; }
    public required string UniqueSize { get; init; }
}