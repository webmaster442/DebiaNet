using Debianet.Abstractions;

namespace Debianet.Ui;

internal sealed class DelegateTaskMenuItem : MenuItemBase
{
    public required Func<CancellationToken, Task> Task { get; init; }

    public override async Task Execute(IApplication app, CancellationToken cancellationToken)
    {
        await Task(cancellationToken);
    }
}