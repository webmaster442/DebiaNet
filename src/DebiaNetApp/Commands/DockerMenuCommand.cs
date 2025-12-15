
using DebiaNetApp.Dto;
using DebiaNetApp.Infrastructure;
using DebiaNetApp.Properties;

using Spectre.Console;
using Spectre.Console.Cli;

namespace DebiaNetApp.Commands;

internal sealed class DockerMenuCommand : AsyncCommand
{
    public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        var selections = new SelectorMenuItem[]
        {
            new JsonCliMenuItem<DockerPs>(Resources.Menu_Docker_ListRunningContainers, $"{Emoji.Known.Scroll} ")
            {
                Command = new ("docker", "ps", "--format", "\"{{json . }}\""),
                ResultProcessor = DisplayAsTables
            },
            new JsonCliMenuItem<DockerPs>(Resources.Menu_Docker_ListAllContainers, $"{Emoji.Known.Scroll} ")
            {
                Command = new("docker", "ps", "-a", "--format", "\"{{json . }}\""),
                ResultProcessor = DisplayAsTables
            },
            new JsonCliMenuItem<DockerImage>(Resources.Menu_Docker_ListImages, $"{Emoji.Known.Scroll} ")
            {
                Command = new ("docker", "images", "--format", "\"{{json . }}\""),
                ResultProcessor = DisplayAsTables
            },
            new CliMenuItem(Resources.Menu_Docker_Stop, Emoji.Known.StopSign)
            {
                Commands =
                [
                    new("bash", "-c", "docker ps -q | xargs -r docker stop")
                ],
            },
            new DelegateMenuItem(Resources.Menu_Exit, Emoji.Known.Door, Tasks.Exit)
        };

        SelectorMenuItem selection = null!;

        AnsiConsole.AlternateScreen(() =>
        {
            AnsiConsole.Clear();

            var text = new FigletText("DebiaNet Docker")
                .Color(Color.Purple);

            AnsiConsole.Write(text);
            AnsiConsole.WriteLine();

            selection = Ui.SelectorMenu(selections);
        });

        return await selection.Execute();
    }

    private void DisplayAsTables(IEnumerable<DockerImage> enumerable)
    {
        foreach (var item in enumerable.OrderBy(x => x.ID))
        {
            AnsiConsole.MarkupLine($"[green]{Resources.Table_Header_Container} [italic]{item.ID}[/]");
            Ui.DisplayAsTable(item);
        }
    }

    private void DisplayAsTables(IEnumerable<DockerPs> enumerable)
    {
        foreach (var item in enumerable.OrderBy(x => x.ID))
        {
            AnsiConsole.MarkupLine($"[green]{Resources.Table_Header_Container} [italic]{item.ID}[/]");
            Ui.DisplayAsTable(item);
        }
    }
}