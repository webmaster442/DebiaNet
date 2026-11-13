using Debianet;
using Debianet.Infrastructure;

Terminal terminal = new();

//StartupChecks startupChecks = new(terminal);

//if (startupChecks.TryCheckExit(out int exitCode))
//{
//    return exitCode;
//}

using ConsoleCancellation cancellation = new();

await Server.RunServerAsync(args, cancellation.Token);

return ExitCodes.Success;