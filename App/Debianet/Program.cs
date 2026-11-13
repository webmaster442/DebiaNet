using Debianet;
using Debianet.Infrastructure;

using Spectre.Console;

Terminal terminal = new();

StartupChecks startupChecks = new(terminal);

if (startupChecks.TryCheckExit(out int exitCode))
{
    return exitCode;
}

AnsiConsole.MarkupLine("[green bold]This app requires sudo privileges. Please enter your password.[/]");
if (!await startupChecks.TryCheckForSudo())
{
    AnsiConsole.MarkupLine("[red bold]Sudo privileges missing[/]");
    return ExitCodes.SudoRequired;
}

using ConsoleCancellation cancellation = new();

try
{
    terminal.SwitchToAlternateBuffer();
    AnsiConsole.WriteLine("Server running. Press Ctrl+C to stop.");
    await Server.RunServerAsync(args, cancellation.Token);
}
finally
{
    terminal.SwitchToMainBuffer();
}



return ExitCodes.Success;