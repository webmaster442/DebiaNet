using System.Runtime.InteropServices;

using Debianet.Domain;

namespace Debianet.Infrastructure;

internal sealed partial class StartupChecks
{
    [LibraryImport("libc", EntryPoint = "geteuid")]
    private static partial uint GetEUid();

    private static bool IsRunningWithElevatedPriviliges()
    {
        try
        {
            uint euid = GetEUid();
            return euid == 0;
        }
        catch
        {
            return string.Equals(Environment.UserName, "root", StringComparison.Ordinal);
        }
    }

    private readonly Terminal _terminal;

    public StartupChecks(Terminal terminal)
    {
        _terminal = terminal;
    }

    public bool TryCheckExit(out int exitCode)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            _terminal.ShowMessageBox(new MessageBox
            {
                Title = "Error",
                Message = "This application is only supported on Linux systems."
            });
            exitCode = ExitCodes.NotLinux;
            return true;
        }

        if (IsRunningWithElevatedPriviliges())
        {
            _terminal.ShowMessageBox(new MessageBox
            {
                Title = "Error",
                Message = "This application should not be run as the root user."
            });
            exitCode = ExitCodes.SudoUser;
            return true;
        }

        exitCode = ExitCodes.Success;
        return false;
    }

}
