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
        terminal.Info($"Exit code: {result.ExitCode}, Run time: {FormatTime(result.RunTime)}");
        terminal.WaitKey();
    }

    private static string FormatTime(TimeSpan timeSpan)
    {
        if (timeSpan.TotalMilliseconds < 1000)
        {
            return $"{timeSpan.TotalMilliseconds:F0} ms";
        }
        else if (timeSpan.TotalSeconds < 60)
        {
            return $"{timeSpan.TotalSeconds:F2} s";
        }
        else if (timeSpan.TotalMinutes < 60)
        {
            return $"{(int)timeSpan.TotalMinutes} minutes {timeSpan.Seconds:D2} seconds";
        }
        return $"{(int)timeSpan.TotalHours} hours {timeSpan.Minutes:D2} minutes {timeSpan.Seconds:D2} seconds";
    }
}
