using Spectre.Console;

namespace Debianet.Abstractions;

internal sealed class Palete
{
    public required Color Accent { get; init; }
    public required Color Warning { get; init; }
    public required Color Error { get; init; }
    public required Color Info { get; init; }
    public required Color Success { get; init; }

    public static readonly Palete Dracula = new()
    {
        Accent = Color.FromHex("#F8F8F2"),
        Error = Color.FromHex("#FF5555"),
        Warning = Color.FromHex("#F1FA8C"),
        Info = Color.FromHex("#BD93F9"),
        Success = Color.FromHex("#50FA7B")
    };
}
