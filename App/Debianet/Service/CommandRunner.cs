using CliWrap;

using Debianet.Domain;
using Debianet.ServiceContract;

namespace Debianet.Service;

internal sealed class CommandRunner : ICommandRunner
{
    public CommandBundle? CreateCommands(string getParam)
    {
        switch (getParam)
        {
            case "aptclean":
                return CommandBundle.Builder()
                    .Command("sudo", ["apt", "atuoremove"])
                    .Command("sudo", ["apt", "clean"])
                    .Build();
            case "aptupdate":
                return CommandBundle.Builder()
                    .Command("sudo", ["apt", "update"])
                    .Command("sudo", ["apt", "upgrade", "-y"])
                    .Build();
            case "installdevtools":
                return CommandBundle.Builder()
                    .Command("sudo", ["apt", "update"])
                    .Command("sudo", ["apt", "install", "-y", "build-essential", "cmake", "gcc", "g++"])
                    .Build();
            case "list-net-runtimes":
                return CommandBundle.Create("dotnet", ["--list-runtimes"]);
            case "list-net-sdks":
                return CommandBundle.Create("dotnet", ["--list-sdks"]);
            case "list-net-workloads":
                return CommandBundle.Create("dotnet", ["workload", "list"]);
            case "list-net-global-tools":
                return CommandBundle.Create("dotnet", ["tool", "list", "-g"]);
            case "list-net-templates":
                return CommandBundle.Create("dotnet", ["new", "list"]);
            case "update-workloads":
                return CommandBundle.Create("dotnet", ["workload", "update"]);
            case "update-installed-tools":
                return CommandBundle.Create("dotnet", ["tool", "update", "-g", "--all"]);
            case "clear-nuget-local-cache":
                return CommandBundle.Create("dotnet", ["nuget", "locals", "all", "--clear"]);
            default:
                return null;
        }
    }

    public async Task RunCommandsAsync(CommandBundle commandBundle,
                                       Func<string, Task> info,
                                       Func<string, Task> stdErr,
                                       Func<string, Task> stdOut,
                                       CancellationToken cancellationToken)
    {
        TimeSpan totalTime = TimeSpan.Zero;
        bool success = true;

        try
        {
            foreach (var command in commandBundle.Commands)
            {
                await info($"Executing command: {command}");

                CommandResult cmdResult = await Cli.Wrap(command.Program)
                    .WithArguments(command.Arguments)
                    .WithStandardErrorPipe(PipeTarget.ToDelegate(stdErr))
                    .WithStandardOutputPipe(PipeTarget.ToDelegate(stdOut))
                    .WithValidation(CommandResultValidation.None)
                    .ExecuteAsync(cancellationToken);

                totalTime += cmdResult.RunTime;

                if (cmdResult.ExitCode != 0)
                {
                    await stdErr("Command failed with exit code: " + cmdResult.ExitCode);
                    success = false;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            await stdErr($"An error occurred while executing commands: {ex.Message}");
            success = false;
        }

        await info($"{(success ? "Success" : "Failure")}, Total execution time: {totalTime}");

    }
}
