using Debianet.Abstractions;
using Debianet.Properties;
using Debianet.Ui;

namespace Debianet.Menus;

internal sealed class SystemMenu : Menu
{
    private readonly ITerminal _terminal;

    public SystemMenu(MenuRegistry menuRegistry, ITerminal terminal) : base(menuRegistry)
    {
        _terminal = terminal;
    }

    public override string Title
        => Resources.SystemMenu_Title;

    public override IEnumerable<MenuItemBase> Items
    {
        get
        {
            yield return new ReplaceMenuMenuItem
            {
                Icon = Icons.Back,
                Text = Resources.Menu_Item_Back,
                Submenu = MenuRegistry.Main
            };
            yield return new RunCommandsMenuItem(_terminal)
            {
                Text = Resources.SystemMenu_AptClean,
                Commands =
                [
                    ("sudo", ["apt", "autoremove"]),
                    ("sudo", ["apt", "clean"])
                ]
            };
            yield return new RunCommandsMenuItem(_terminal)
            {
                Text = Resources.SystemMenu_Update,
                Commands =
                [
                    ("sudo", ["apt", "update"]),
                    ("sudo", ["apt", "upgrade", "-y"])
                ]
            };
            yield return new RunCommandsMenuItem(_terminal)
            {
                Text = Resources.SystemMenu_InstallCpp,
                Commands =
                [
                    ("sudo", ["apt", "update"]),
                    ("sudo", ["apt", "install", "-y", "build-essential", "cmake", "gcc", "g++", "libtool"])
                ]
            };
        }
    }
}