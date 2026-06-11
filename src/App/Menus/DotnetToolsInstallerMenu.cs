using CliWrap;

using Debianet.Abstractions;
using Debianet.Properties;
using Debianet.Ui;

namespace Debianet.Menus;

internal class DotnetToolsInstallerMenu : MultiSelectMenu
{
    private readonly ITerminal _terminal;

    public DotnetToolsInstallerMenu(MenuRegistry menuRegistry, ITerminal terminal)
        : base(menuRegistry)
    {
        _terminal = terminal;
    }

    public override string Title
        => Resources.DotnetToolsInstall_Title;

    private MenuItem CreateToolInstaller(string toolName)
    {
        return new MenuItem
        {
            Text = toolName,
            Data = toolName
        };
    }

    public override IEnumerable<MenuItemBase> Items
    {
        get
        {
            yield return CreateToolInstaller("dotnet-counters");
            yield return CreateToolInstaller("dotnet-coverage");
            yield return CreateToolInstaller("dotnet-ef");
            yield return CreateToolInstaller("dotnet-counters");
            yield return CreateToolInstaller("dotnet-gcdump");
            yield return CreateToolInstaller("dotnet-monitor");
            yield return CreateToolInstaller("dotnet-stack");
            yield return CreateToolInstaller("dotnet-symbol");
            yield return CreateToolInstaller("dotnet-trace");
            yield return CreateToolInstaller("Microsoft.VisualStudio.SlnGen.Tool");
            yield return CreateToolInstaller("PowerShell");
            yield return CreateToolInstaller("upgrade-assistant");
            yield return CreateToolInstaller("docfx");
            yield return CreateToolInstaller("csharprepl");
            yield return CreateToolInstaller("ilspycmd");
            yield return CreateToolInstaller("roslynator.dotnet.cli");
            yield return CreateToolInstaller("CsProj");
        }
    }

    public override async Task ProcessSelectedItems(IReadOnlyList<MenuItemBase> selectedItems, CancellationToken cancellationToken)
    {
        int counter = 0;
        var items = selectedItems.OfType<MenuItem>().ToArray();
        TimeSpan totalTime = TimeSpan.Zero;

        foreach (var item in items)
        {
            _terminal.Info($"{counter}/{items.Length}: Installing {item.Data}...");
            _terminal.Line();

            var result = await Cli.Wrap("dotnet")
                .WithArguments(["tool", "install", "-g", item.Data])
                .WithStandardOutputPipe(PipeTarget.ToDelegate(_terminal.StandardOutput))
                .WithStandardErrorPipe(PipeTarget.ToDelegate(_terminal.StandardError))
                .ExecuteAsync(cancellationToken);

            _terminal.Line();
            ++counter;
            totalTime += result.RunTime;
        }
        _terminal.Info($"Run time: {totalTime.FormatTime()}");
        _terminal.WaitKey();

    }
}
