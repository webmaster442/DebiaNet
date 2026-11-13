namespace Debianet.Domain;

internal sealed class Command
{
    public required string Program { get; init; }
    public required string[] Arguments { get; init; }

    public override string ToString()
    {
        return $"{Program} {string.Join(" ", Arguments.Select(arg => $"\"{arg}\""))}";
    }
}
