using Debianet;
using Debianet.Abstractions;

var terminal = new Terminal();
using var dockerClient = new DockerClient();

GlobalArgHandler.HandleGlobalArgs(args);

var startupChecks = new StartupChecks(terminal);
if (startupChecks.TryCheckExit(out int exitCode))
{
    return exitCode;
}

var registry = new MenuRegistry(terminal, dockerClient);
var application = new Application(terminal, registry);

await application.Run();

return ExitCodes.Success;