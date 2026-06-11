using Debianet.Abstractions;
using Debianet.Menus;

namespace Debianet;

internal class MenuRegistry
{
    public MenuRegistry(ITerminal terminal)
    {
        Main = new MainMenu(this, terminal);
        Dotnet = new DotnetMenu(this, terminal);
        DotnetToolInstaller = new DotnetToolsInstallerMenu(this, terminal);
    }

    public MainMenu Main { get; }
    public DotnetMenu Dotnet { get; }
    public DotnetToolsInstallerMenu DotnetToolInstaller { get; }
}
