using Debianet.Abstractions;

namespace Debianet.Ui;

internal class ReplaceMenuMenuItem : MenuItemBase
{
    public required Menu Submenu { get; init; }

    public override async Task Execute(IApplication app, CancellationToken cancellationToken)
    {
        app.SwitchMenu(Submenu);
    }
}
