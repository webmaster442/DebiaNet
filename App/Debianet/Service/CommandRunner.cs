using System.Diagnostics;
using System.Text;

using CliWrap;
using CliWrap.Buffered;

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
                    .Command("sudo", ["-n", "apt-get", "autoremove", "-y"])
                    .Command("sudo", ["-n", "apt-get", "clean", "-y"])
                    .Build();
            case "aptupdate":
                return CommandBundle.Builder()
                    .Command("sudo", ["-n", "apt-get", "update"])
                    .Command("sudo", ["-n", "apt-get", "upgrade", "-y"])
                    .Build();
            case "installdevtools":
                return CommandBundle.Builder()
                    .Command("sudo", ["-n", "apt-get", "update"])
                    .Command("sudo", ["-n", "apt-get", "install", "-y", "build-essential", "cmake", "gcc", "g++"])
                    .Build();
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

    public async Task<bool> RunCommandsAsync(CommandBundle commandBundle,
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
                    .WithArguments(command.Arguments, true)
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

        return success;

    }

    public async Task<Result<string>> CaptureCommandOutputAsync(string command, string[] arguments, int? lineCount, CancellationToken cancellationToken)
    {
        if (command.StartsWith('$'))
        {
            command = Environment.GetEnvironmentVariable(command[1..]) ?? command;
        }

        var result = await CaptureCommandOutputAsync(CommandBundle.Create(command, arguments), cancellationToken);

        if (result.Value is Error error)
            return error;

        if (result.Value is string str)
        {
            return lineCount.HasValue 
                ? GetLines(str, lineCount.Value)
                : str;
        }

        throw new UnreachableException("This code path should never be reached.");
    }

    private static string GetLines(string str, int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 1);

        StringBuilder stringBuilder = new StringBuilder();
        using var reader = new StringReader(str);
        string? line;
        do
        {
            line = reader.ReadLine();
            if (line == null)
                break;
            stringBuilder.AppendLine(line);
        }
        while (--value > 0 && line != null);
        return stringBuilder.ToString();
    }

    public async Task<Result<string>> CaptureCommandOutputAsync(CommandBundle commandBundle, CancellationToken cancellationToken)
    {
        if (commandBundle.Commands.Count > 1)
        {
            return new Error("CaptureCommandOutputAsync only supports a single command.");
        }

        BufferedCommandResult result = await Cli.Wrap(commandBundle.Commands[0].Program)
            .WithArguments(commandBundle.Commands[0].Arguments)
            .WithValidation(CommandResultValidation.None)
            .ExecuteBufferedAsync(cancellationToken);

        return result.ExitCode != 0
            ? (Result<string>)new Error($"Command failed with exit code: {result.ExitCode}, Error: {result.StandardError}")
            : (Result<string>)result.StandardOutput;
    }

}
