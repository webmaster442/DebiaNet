using Debianet.Abstractions;
using Debianet.Properties;
using Debianet.Ui;

namespace Debianet.Menus;

internal class DockerMenu : Menu
{
    private readonly ITerminal _terminal;
    private readonly IDockerClient _dockerClient;

    public DockerMenu(MenuRegistry menuRegistry, ITerminal terminal, IDockerClient dockerClient)
        : base(menuRegistry)
    {
        _terminal = terminal;
        _dockerClient = dockerClient;
    }

    public override string Title
        => Resources.DockerMenu_Title;

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
        }
    }
}
