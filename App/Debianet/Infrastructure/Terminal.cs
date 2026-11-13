using Debianet.Domain;

using Spectre.Console;

namespace Debianet.Infrastructure;

internal sealed class Terminal
{
    private readonly Palete _palete = Palete.Dracula;

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

    public void SwitchToAlternateBuffer()
        => AnsiConsole.WriteLine("\e[?1049h");

    public void SwitchToMainBuffer()
        => AnsiConsole.WriteLine("\e[?1049l");

    public void Clear()
        => AnsiConsole.Clear();

    public void WaitKey()
    {
        AnsiConsole.MarkupLine($"[#{_palete.Accent.ToHex()}]Press any key to continue...[/]");
        Console.ReadKey();
    }

    public void ShowMessageBox(MessageBox messageBox)
    {
        SwitchToAlternateBuffer();
        Clear();

        int topPad = (Console.WindowHeight / 2) - 4;

        Console.SetCursorPosition(0, topPad);

        var panel = new Panel(messageBox.Message)
            .Header($"| {messageBox.Title} |", Justify.Center)
            .HeavyBorder()
            .BorderColor(_palete.Accent)
            .Expand();

        AnsiConsole.Write(panel);

        WaitKey();
        SwitchToMainBuffer();
    }

}
