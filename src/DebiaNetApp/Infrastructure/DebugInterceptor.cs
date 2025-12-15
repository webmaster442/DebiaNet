using System.Diagnostics;

using Spectre.Console;
using Spectre.Console.Cli;

namespace DebiaNetApp.Infrastructure;

internal class DebugInterceptor : ICommandInterceptor
{
    public void Intercept(CommandContext context, CommandSettings settings)
    {
        if (context.Arguments.Contains("--wd"))
        {
            AnsiConsole.MarkupLine("[yellow]Waiting for debugger...[/]");
            AnsiConsole.MarkupLine("Press [red]CTRL+C[/] to abort it.");
            while (!Debugger.IsAttached)
            {
                Thread.Sleep(25);
            }
        }
        Debug.WriteLine("Debugger attached. Happy debugging.");
        Debugger.Break();
    }
}
