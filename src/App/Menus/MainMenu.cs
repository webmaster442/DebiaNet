using Debianet.Abstractions;
using Debianet.Properties;
using Debianet.Ui;

using Spectre.Console;

namespace Debianet.Menus;

internal class MainMenu : Menu
{
    private readonly ITerminal _terminal;
    private readonly IDockerClient _dockerClient;

    public MainMenu(MenuRegistry menuRegistry, ITerminal terminal, IDockerClient dockerClient)
        : base(menuRegistry)
    {
        _terminal = terminal;
        _dockerClient = dockerClient;
    }

    public override string Title
        => Resources.MainMenu_Title;

    public override void BeforeSelection()
    {
        string text = $$"""
                     @                                                            
                 @@@@@@@@@@@@@                                                    
               @@@@         @@@@                                                  
            @ @@              @@@ @@@@@        @@@     @@@@@@@@@@  @@@@@@@@@@@@@@ 
            %@        @@@@     @@ @@@@@@       @@@     @@@               @@@      
            @@                 @@ @@@ @@@      @@@     @@@               @@@      
            @       @          @  @@@  @@@@    @@@     @@@               @@@      
            @       @         @@  @@@   @@@@   @@@     @@@@@@@@@@        @@@      
            @@      @@       @    @@@    @@@@  @@@     @@@               @@@      
            @@          :@*       @@@      @@@ @@@     @@@               @@@      
             @@                   @@@       @@@@@@     @@@               @@@      
              @@                  @@@        @@@@@     @@@@@@@@@@        @@@      
                @                                                                 
                  @@                                                              
                      @@  App Version: {{AppVersionProvider.GetAppVersion()}}

            """;

        _terminal.Info($"{text}");
    }

    public override IEnumerable<MenuItemBase> Items
    {
        get
        {
            yield return new ReplaceMenuMenuItem
            {
                Icon = Icons.SubMenu,
                Text = Resources.MainMenu_Item_Dotnet,
                Submenu = MenuRegistry.Dotnet
            };
            yield return new ReplaceMenuMenuItem
            {
                Icon = Icons.SubMenu,
                Text = Resources.MainMenu_DotnetToolsInstall,
                Submenu = MenuRegistry.DotnetToolInstaller
            };
            yield return new ReplaceMenuMenuItem
            {
                Icon = Icons.SubMenu,
                Text = Resources.MainMenu_System,
                Submenu = MenuRegistry.System,
            };

            if (DockerClient.IsDockerInstalled())
            {
                yield return new ReplaceMenuMenuItem
                {
                    Icon = Icons.SubMenu,
                    Text = Resources.MainMenu_Docker,
                    Submenu = new DockerMenu(MenuRegistry, _terminal, _dockerClient),
                };
            }

            yield return new DelegateTaskMenuItem
            {
                Icon = Icons.Computer,
                Task = SytemInfo,
                Text = Resources.MainMenu_SystemInfo,
            };
            yield return new DelegateMenuItem
            {
                Text = Resources.MainMenu_Changelog,
                Icon = Icons.Text,
                Action = DisplayChangeLog
            };
            yield return new DelegateMenuItem()
            {
                Text = Resources.MainMenu_Item_Exit,
                Icon = Icons.Door,
                Action = Exit
            };
        }
    }

    private async Task SytemInfo(CancellationToken token)
    {
        var grid = new Grid();
        grid.AddColumn();
        grid.AddColumn();
        await foreach (var (property, value) in SystemInfoCollector.CollectSystemInfoAsync())
        {
            grid.AddRow(property, value);
        }
        _terminal.ShowDialog(Resources.Dialog_Sysinfo, grid);
    }

    private void DisplayChangeLog()
    {
        var changelog = ResourceHandler.GetChangeLog();
        var viewer = new TextViewer(changelog, Resources.TextView_Changelog);
        viewer.Show();
    }

    private void Exit()
    {
        _terminal.Clear();
        Environment.Exit(0);
    }
}
