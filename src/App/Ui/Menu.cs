namespace Debianet.Ui;

internal abstract class Menu
{
    public MenuRegistry MenuRegistry { get; }

    public Menu(MenuRegistry menuRegistry)
    {
        MenuRegistry = menuRegistry;
    }

    public abstract string Title { get; }

    public abstract IEnumerable<MenuItemBase> Items { get; }

    public virtual void BeforeSelection()
    {

    }
}
