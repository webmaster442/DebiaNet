using CliWrap;

using Debianet.Abstractions;

namespace Debianet.Ui;

internal sealed class RunCommandsMenuItem(ITerminal terminal) : MenuItemBase
{
    public required IEnumerable<(string program, string[] arguments)> Commands { get; init; }

    public override async Task Execute(IApplication app, CancellationToken cancellationToken)
    {
        TimeSpan totalTime = TimeSpan.Zero;
        foreach (var (program, arguments) in Commands)
        {
            terminal.Info($"Executing {program} {string.Join(' ', arguments)} ...");
            terminal.Line();

            var result = await Cli.Wrap(program)
                .WithArguments(arguments)
                .WithStandardOutputPipe(PipeTarget.ToDelegate(terminal.StandardOutput))
                .WithStandardErrorPipe(PipeTarget.ToDelegate(terminal.StandardError))
                .ExecuteAsync(cancellationToken);

            if (result.ExitCode != 0)
            {
                terminal.Error($"Command {program} {string.Join(' ', arguments)} failed with exit code {result.ExitCode}");
                break;
            }

            totalTime += result.RunTime;
            terminal.Line();
        }
        terminal.Info($"Total run time: {totalTime.FormatTime()}");
        terminal.WaitKey();
    }
}
