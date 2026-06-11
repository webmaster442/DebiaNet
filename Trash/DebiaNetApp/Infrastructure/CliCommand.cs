namespace DebiaNetApp.Infrastructure;

internal sealed class CliCommand
{
    public CliCommand(string program, params string[] arguments)
    {
        Program = program;
        Arguments = arguments;
    }

    public string CommandLine => $"{Program} {string.Join(' ', Arguments)}";

    public string Program { get; }
    public string[] Arguments { get; }
}