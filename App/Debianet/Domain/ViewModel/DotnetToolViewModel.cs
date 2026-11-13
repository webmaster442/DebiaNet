namespace Debianet.Domain.ViewModel;

internal sealed class DotnetToolViewModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Link { get; set; }
    public required bool IsChecked { get; set; }
}