namespace Debianet.Ui;

internal abstract class MultiSelectMenu : Menu
{
    protected MultiSelectMenu(MenuRegistry menuRegistry)
        : base(menuRegistry)
    {
    }

    public abstract Task ProcessSelectedItems(IReadOnlyList<MenuItemBase> selectedItems, CancellationToken cancellationToken);
}
