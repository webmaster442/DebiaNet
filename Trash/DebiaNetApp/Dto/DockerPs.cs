namespace DebiaNetApp.Dto;

internal record class DockerPs : DockerContainer
{
    public required string Command { get; init; }
    public required string CreatedAt { get; init; }
    public required string Image { get; init; }
    public required string Labels { get; init; }
    public required string LocalVolumes { get; init; }
    public required string Mounts { get; init; }
    public required string Names { get; init; }
    public required string Networks { get; init; }
    public required DockerPlatform Platform { get; init; }
    public required string Ports { get; init; }
    public required string RunningFor { get; init; }
    public required string Size { get; init; }
    public required string State { get; init; }
    public required string Status { get; init; }
}