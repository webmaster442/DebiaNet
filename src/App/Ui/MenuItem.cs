using Debianet.Abstractions;

namespace Debianet.Ui;

internal class MenuItem : MenuItemBase
{
    public string Data { get; set; } = "";

    public override Task Execute(IApplication app, CancellationToken cancellationToken)
        => Task.CompletedTask;
}