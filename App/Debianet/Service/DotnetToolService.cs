using System.Diagnostics;

using Debianet.Domain;
using Debianet.ServiceContract;

namespace Debianet.Service;

internal sealed class DotnetToolService : IDotnetToolService
{
    private readonly ICommandRunner _commandRunner;

    public DotnetToolService(ICommandRunner commandRunner)
    {
        _commandRunner = commandRunner;
    }

    public async Task<Result<IReadOnlySet<string>>> GetInstalledToolsAsync(CancellationToken cancellationToken)
    {

        Result<string> result = await _commandRunner.CaptureCommandOutputAsync(CommandBundle.Create("dotnet", ["tool", "list", "-g"]).Commands.First(), cancellationToken);

        if (result.Value is Error error)
            return error;

        if (result.Value is string str)
        {
            HashSet<string> tools = [];
            foreach (string line in str.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
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
        
        throw new UnreachableException("This code should be unreachable.");
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
