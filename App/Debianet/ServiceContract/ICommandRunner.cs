using Debianet.Domain;

namespace Debianet.ServiceContract;

internal interface ICommandRunner
{
    CommandBundle? CreateCommands(string getParam);
    Task RunCommandsAsync(CommandBundle commandBundle,
                          Func<string, Task> info,
                          Func<string, Task> stdErr,
                          Func<string, Task> stdOut,
                          CancellationToken cancellationToken);
}
