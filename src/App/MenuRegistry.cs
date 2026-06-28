using Debianet.Abstractions;
using Debianet.Menus;

namespace Debianet;

internal class MenuRegistry
{
    public MenuRegistry(ITerminal terminal, DockerClient dockerClient)
    {
        Main = new MainMenu(this, terminal);
        Dotnet = new DotnetMenu(this, terminal);
        DotnetToolInstaller = new DotnetToolsInstallerMenu(this, terminal);
        System = new SystemMenu(this, terminal);
        Docker = new DockerMenu(this, dockerClient);
    }

    public MainMenu Main { get; }
    public DotnetMenu Dotnet { get; }
    public DotnetToolsInstallerMenu DotnetToolInstaller { get; }
    public SystemMenu System { get; }
    public DockerMenu Docker { get; }
}
