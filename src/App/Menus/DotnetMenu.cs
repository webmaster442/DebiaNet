using Debianet.Abstractions;
using Debianet.Properties;
using Debianet.Ui;

using Spectre.Console;

namespace Debianet.Menus;

internal class DotnetMenu : Menu
{
    private readonly ITerminal _terminal;

    public DotnetMenu(MenuRegistry menuRegistry, ITerminal terminal)
        : base(menuRegistry)
    {
        _terminal = terminal;
    }

    public override string Title
        => Resources.DotnetMenu_Title;

    public override void BeforeSelection()
    {
        AnsiConsole.Write(new FigletText(".NET"));
    }

    public override IEnumerable<MenuItemBase> Items
    {
        get
        {
            yield return new ReplaceMenuMenuItem
            {
                Icon = Icons.Back,
                Text = Resources.DotnetMenu_Item_Back,
                Submenu = MenuRegistry.Main
            };
            yield return new RunCommandMenuItem(_terminal)
            {
                Text = Resources.DotnetMenu_Item_ListRuntimes,
                Program = "dotnet",
                Arguments = ["--list-runtimes"]
            };
            yield return new RunCommandMenuItem(_terminal)
            {
                Text = Resources.DotnetMenu_Item_ListSdks,
                Program = "dotnet",
                Arguments = ["--list-sdks"]
            };
            yield return new RunCommandMenuItem(_terminal)
            {
                Text = Resources.DotnetMenu_Item_ListWorkloads,
                Program = "dotnet",
                Arguments = ["workload", "list"]
            };
            yield return new RunCommandMenuItem(_terminal)
            {
                Text = Resources.DotnetMenu_Item_ListTools,
                Program = "dotnet",
                Arguments = ["tool", "list", "-g"]
            };
            yield return new RunCommandMenuItem(_terminal)
            {
                Text = Resources.DotnetMenu_Item_UpdateWorkloads,
                Program = "dotnet",
                Arguments = ["workload", "update"]
            };
            yield return new RunCommandMenuItem(_terminal)
            {
                Text = Resources.DotnetMenu_Item_UpdateTools,
                Program = "dotnet",
                Arguments = ["tool", "update", "-g", "--all"]
            };
            yield return new RunCommandMenuItem(_terminal)
            {
                Text = Resources.DotnetMenu_Item_ClearNugetCache,
                Program = "dotnet",
                Arguments = ["nuget", "locals", "all", "--clear"]
            };
        }
    }
}
