using CliWrap;
using CliWrap.Buffered;

using Debianet.Domain;
using Debianet.ServiceContract;

namespace Debianet.Service;

internal sealed class DotnetToolService : IDotnetToolService
{
    public async Task<IReadOnlySet<string>> GetInstalledToolsAsync()
    {
        BufferedCommandResult result = await Cli.Wrap("dotnet")
            .WithArguments(["tool", "list", "--global"])
            .ExecuteBufferedAsync();

        if (result.ExitCode != 0)
        {
            throw new InvalidOperationException($"Failed to list installed dotnet tools: {result.StandardError}");
        }

        HashSet<string> tools = [];
        foreach (string line in result.StandardOutput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (line.StartsWith("Package Id", StringComparison.OrdinalIgnoreCase) ||
                line.StartsWith("---", StringComparison.Ordinal))
            {
                continue;
            }

            string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length > 0)
            {
                tools.Add(parts[0]);
            }
        }

        return tools;
    }

    public CommandBundle CreateInstallBundle(IEnumerable<string> toolsToInstall, IEnumerable<string> toolsToUninstall)
    {
        var builder = CommandBundle.Builder();
        foreach (string tool in toolsToUninstall)
        {
            builder.Command("dotnet", ["tool", "uninstall", tool, "--global"]);
        }
        
        foreach (string tool in toolsToInstall)
        {
            builder.Command("dotnet", ["tool", "install", tool, "--global"]);
        }
        return builder.Build();
    }
}
