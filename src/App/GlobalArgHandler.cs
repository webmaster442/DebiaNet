using Spectre.Console;

namespace Debianet;

internal static class GlobalArgHandler
{
    public static void HandleGlobalArgs(string[] args)
    {
        HashSet<string> argsSet = new HashSet<string>(args, StringComparer.InvariantCultureIgnoreCase);

        if (argsSet.Contains("--wd") || argsSet.Contains("--wait-debugger"))
        {
            AnsiConsole.WriteLine("Waiting for debugger to attach...");
            while (!System.Diagnostics.Debugger.IsAttached)
            {
                Thread.Sleep(100);
            }
            AnsiConsole.WriteLine("Debugger attached.");
        }

        if (argsSet.Contains("--version") || argsSet.Contains("-v"))
        {
            AnsiConsole.WriteLine(AppVersionProvider.GetAppVersion().ToString());
            Environment.Exit(0);
        }
    }
}
