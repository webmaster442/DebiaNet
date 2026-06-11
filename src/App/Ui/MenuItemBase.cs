using Debianet.Abstractions;

namespace Debianet.Ui;

internal abstract class MenuItemBase
{
    public string? Icon { get; init; }
    public required string Text { get; init; }
    public abstract Task Execute(IApplication app, CancellationToken cancellationToken);
}
