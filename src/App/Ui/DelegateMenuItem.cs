using Debianet.Abstractions;

namespace Debianet.Ui;

internal sealed class DelegateMenuItem : MenuItemBase
{
    public required Action Action { get; init; }

    public override Task Execute(IApplication app, CancellationToken cancellationToken)
    {
        Action();
        return Task.CompletedTask;
    }
}
