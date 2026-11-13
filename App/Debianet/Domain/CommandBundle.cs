namespace Debianet.Domain;

internal sealed class CommandBundle
{
    public required List<Command> Commands { get; init; }

    public static CommandBundle Create(string command, string[] arguments)
    {
        return new CommandBundle
        {
            Commands =
            [
                new Command
                {
                    Program = command,
                    Arguments = arguments
                }
            ]
        };
    }

    public static BundleBuilder Builder()
    {
        return new BundleBuilder();
    }

    internal sealed class BundleBuilder
    {
        private readonly List<Command> _commands = new();

        public BundleBuilder Command(string program, string[] arguments)
        {
            _commands.Add(new Command
            {
                Program = program,
                Arguments = arguments
            });
            return this;
        }

        public CommandBundle Build()
        {
            return new CommandBundle
            {
                Commands = _commands
            };
        }
    }
}
