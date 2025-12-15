using System.Runtime.InteropServices;

namespace DebiaNetApp.Infrastructure;

internal static partial class Linux
{
    [LibraryImport("libc")]
    private static partial uint geteuid();

    public static bool IsRunningWithElevatedPriviliges()
    {
        try
        {
            uint euid = geteuid();
            return euid == 0;
        }
        catch
        {
            return string.Equals(Environment.UserName, "root", StringComparison.Ordinal);
        }
    }
}