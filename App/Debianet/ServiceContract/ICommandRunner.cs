using Debianet.Domain;

namespace Debianet.ServiceContract;

internal interface ICommandRunner
{
    CommandBundle? CreateCommands(string getParam);

    Task<bool> RunCommandsAsync(CommandBundle commandBundle,
                                Func<string, Task> info,
                                Func<string, Task> stdErr,
                                Func<string, Task> stdOut,
                                CancellationToken cancellationToken);


    Task<Result<string>> CaptureCommandOutputAsync(string command,
                                                   string[] arguments,
                                                   int? lineCount,
                                                   CancellationToken cancellationToken);
    Task<Result<string>> CaptureCommandOutputAsync(CommandBundle commandBundle,
                                                   CancellationToken cancellationToken);
}
