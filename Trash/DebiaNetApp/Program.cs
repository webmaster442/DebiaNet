using DebiaNetApp;
using DebiaNetApp.Commands;
using DebiaNetApp.Infrastructure;
using DebiaNetApp.Properties;

using Spectre.Console;
using Spectre.Console.Cli;

Console.OutputEncoding = System.Text.Encoding.UTF8;

if (!OperatingSystem.IsLinux())
{
    Ui.Error(Resources.Error_NotLinux);
    return ExitCodes.NotLinux;
}

if (Linux.IsRunningWithElevatedPriviliges())
{
    Ui.Error(Resources.Error_SudoUser);
    return ExitCodes.SudoUser;
}

if (!DependencyChecker.IsAptAvailable())
{
    Ui.Error(Resources.Error_NotAptDistro);
    return ExitCodes.MissingDependency;
}

var app = new CommandApp<MainMenuCommand>();
app.Configure(cfg =>
{
    cfg.SetApplicationName("debianet");
    cfg.PropagateExceptions();
    cfg.SetInterceptor(new DebugInterceptor());
    cfg.SetExceptionHandler((exception, resolver) =>
    {
        AnsiConsole.WriteException(exception);
    });
    cfg
    .AddCommand<DockerMenuCommand>("docker")
        .WithDescription("Docker related tasks");
});

app.Run(args);

return ExitCodes.Success;