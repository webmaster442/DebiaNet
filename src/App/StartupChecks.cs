using System.Runtime.InteropServices;

using Debianet.Abstractions;
using Debianet.Properties;
using Debianet.Ui;

namespace Debianet;

internal sealed partial class StartupChecks
{
    private readonly Terminal _terminal;

    public StartupChecks(Terminal terminal)
    {
        _terminal = terminal;
    }

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

    public bool TryCheckExit(out int exitCode)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            _terminal.ShowMessageBox(new MessageBox
            {
                Title = Resources.MessageBox_Error,
                Message = Resources.StartupCheck_NotLinux
            });
            exitCode = ExitCodes.NotLinux;
            return true;
        }

        if (IsRunningWithElevatedPriviliges())
        {
            _terminal.ShowMessageBox(new MessageBox
            {
                Title = Resources.MessageBox_Error,
                Message = Resources.StartupCheck_RootUser
            });
            exitCode = ExitCodes.SudoUser;
            return true;
        }

        exitCode = ExitCodes.Success;
        return false;
    }
}
