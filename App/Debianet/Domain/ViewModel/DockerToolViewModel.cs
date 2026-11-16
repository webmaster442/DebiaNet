namespace Debianet.Domain.ViewModel;

internal sealed class DockerToolViewModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Url { get; set; }
    public required string Link { get; set; }
    public required string? ContainerId { get; set; }
}