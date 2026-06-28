using System.Diagnostics;

namespace DebiaNetApp.Infrastructure;

internal static class DependencyChecker
{
    public static bool IsInstalled(string appBinary, params string[] arguments)
    {
        try
        {
            using var process = new Process();
            process.StartInfo.FileName = appBinary;
            for (int i = 0; i < arguments.Length; i++)
            {
                process.StartInfo.ArgumentList.Add(arguments[i]);
            }
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            process.WaitForExit();
            return process.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsDockerInstalled()
        => IsInstalled("docker", "--version");

    public static bool IsAptAvailable()
        => IsInstalled("apt-get", "--version");
}
