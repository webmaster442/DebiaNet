using System;
using System.Collections.Generic;
using System.Text;

using CliWrap;

namespace DebiaNetApp.Infrastructure;

internal sealed class CliMenuItem : SelectorMenuItem
{
    public CliMenuItem(string displayText, string icon) : base(displayText, icon)
    {
    }

    public required CliCommand[] Commands { get; init; }

    public override async Task<int> Execute()
    {
        int finalExitCode = 0;

        foreach (var command in Commands)
        {
            Ui.DiplayCmd(command.CommandLine);
            Command cmd = Cli
                .Wrap(command.Program)
                .WithArguments(command.Arguments)
                .WithStandardOutputPipe(PipeTarget.ToDelegate(Ui.StandardOutput, Encoding.UTF8))
                .WithStandardErrorPipe(PipeTarget.ToDelegate(Ui.StandardErrorOutput, Encoding.UTF8));

            var result = await cmd.ExecuteAsync();

            finalExitCode &= result.ExitCode;

            Ui.Finished(result.RunTime, result.ExitCode);
        }

        return finalExitCode;
    }
}
