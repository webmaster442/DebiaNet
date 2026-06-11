using Debianet;
using Debianet.Abstractions;

var terminal = new Terminal();
var registry = new MenuRegistry(terminal);
var application = new Application(terminal, registry);

await application.Run();