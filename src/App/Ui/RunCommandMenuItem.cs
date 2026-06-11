using CliWrap;

using Debianet.Abstractions;

namespace Debianet.Ui;

internal sealed class RunCommandMenuItem(ITerminal terminal) : MenuItemBase
{
    public required string Program { get; init; }

    public required string[] Arguments { get; init; }

    public override async Task Execute(IApplication app, CancellationToken cancellationToken)
    {
        terminal.Info($"Executing {Program} {string.Join(' ', Arguments)} ...");
        terminal.Line();

        var result = await Cli.Wrap(Program)
            .WithArguments(Arguments)
            .WithStandardOutputPipe(PipeTarget.ToDelegate(terminal.StandardOutput))
            .WithStandardErrorPipe(PipeTarget.ToDelegate(terminal.StandardError))
            .ExecuteAsync(cancellationToken);

        terminal.Line();
        terminal.Info($"Exit code: {result.ExitCode}, Run time: {result.RunTime.FormatTime()}");
        terminal.WaitKey();
    }
}
