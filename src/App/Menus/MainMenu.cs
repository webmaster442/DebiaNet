using Debianet.Abstractions;
using Debianet.Properties;
using Debianet.Ui;

namespace Debianet.Menus;

internal class MainMenu : Menu
{
    private readonly ITerminal _terminal;

    public MainMenu(MenuRegistry menuRegistry, ITerminal terminal)
        : base(menuRegistry)
    {
        _terminal = terminal;
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
