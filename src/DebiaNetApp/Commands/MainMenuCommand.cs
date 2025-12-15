using DebiaNetApp.Infrastructure;
using DebiaNetApp.Properties;

using Spectre.Console;
using Spectre.Console.Cli;

namespace DebiaNetApp.Commands;

internal sealed class MainMenuCommand : AsyncCommand
{
    public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        var options = new SelectorMenuItem[]
        {
            new CliMenuItem(Resources.Menu_DistUpgrade, Emoji.Known.FastForwardButton)
            {
                Commands =
                [
                    new("sudo", "apt-get", "update"),
                    new("sudo", "apt-get", "dist-upgrade", "-y")
                ],
            },
            new CliMenuItem(Resources.Menu_UpdateDotnetWorkloads, Emoji.Known.FastForwardButton)
            {
                Commands =
                [
                    new("dotnet", "workload", "update"),
                ],
            },
            new CliMenuItem(Resources.Menu_UpdateDotnetTools, Emoji.Known.FastForwardButton)
            {
                Commands =
                [
                    new("dotnet", "tool", "update", "--global", "--all"),
                ],
            },
            new DelegateMenuItem(Resources.Menu_ListEnvironmentVariables, $"{Emoji.Known.Scroll} ", Tasks.EnvVars),
            new CliMenuItem(Resources.Menu_ListDotnetTools, $"{Emoji.Known.Scroll} ")
            {
                Commands =
                [
                    new("dotnet", "tool", "list", "--global"),
                ],
            },
            new CliMenuItem(Resources.Menu_RuntimeAndSdks, $"{Emoji.Known.Scroll} ")
            {
                Commands =
                [
                    new("dotnet", "--list-runtimes"),
                    new("dotnet", "--list-sdks"),
                ],
            },
            new CliMenuItem(Resources.Menu_ListDotnetWorkloads, $"{Emoji.Known.Scroll} ")
            {
                Commands =
                [
                    new("dotnet", "workload", "list"),
                ],
            },
            new DelegateMenuItem(Resources.Menu_SwitchDocker, Emoji.Known.Toolbox, () =>
            {
                var dockerMenu = new DockerMenuCommand();
                return dockerMenu.ExecuteAsync(context, cancellationToken);
            }),
            new CliMenuItem(Resources.Menu_ClearNugetCache, Emoji.Known.Broom)
            {
                Commands =
                [
                    new("dotnet", "nuget", "locals", "all", "--clear"),
                ],
            },
            new DelegateMenuItem(Resources.Menu_Exit, Emoji.Known.Door, Tasks.Exit)

        };

        SelectorMenuItem selection = null!;

        AnsiConsole.AlternateScreen(() =>
        {
            AnsiConsole.Clear();

            var text = new FigletText("DebiaNet")
                .Color(Color.Purple);

            AnsiConsole.Write(text);
            AnsiConsole.MarkupLine("[link]https://github.com/webmaster442/debianet[/]");
            AnsiConsole.WriteLine();

            selection = Ui.SelectorMenu(options);
        });

        return await selection.Execute();
    }
}
