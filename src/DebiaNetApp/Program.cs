using DebiaNetApp;
using DebiaNetApp.Infrastructure;
using DebiaNetApp.Properties;

Console.OutputEncoding = System.Text.Encoding.UTF8;

if (!OperatingSystem.IsLinux())
{
    Ui.Error(Resources.Error_NotLinux);
    return ExitCodes.NotLinux;
}

if (Linux.IsRunningWithElevatedPriviliges())
{
    Ui.Error(Resources.Error_SudoUser);
    return ExitCodes.NotLinux;
}

return ExitCodes.Success;