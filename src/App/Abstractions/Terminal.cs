using Spectre.Console;

namespace Debianet.Abstractions;

internal sealed class Terminal : ITerminal
{
    private readonly Palete _palete;

    public Terminal(Palete? palete = null)
    {
        _palete = palete ?? Palete.Dracula;
    }

    public void Clear()
        => AnsiConsole.Clear();

    public void DisplayException(Exception ex)
        => AnsiConsole.WriteException(ex);

    public void Info(FormattableString formattableString)
        => AnsiConsole.MarkupLine($"[#{_palete.Info.ToHex()}]{formattableString}[/]");

    public void Warning(FormattableString formattableString)
        => AnsiConsole.MarkupLine($"[#{_palete.Warning.ToHex()}]{formattableString}[/]");

    public void Success(FormattableString formattableString)
        => AnsiConsole.MarkupLine($"[#{_palete.Success.ToHex()}]{formattableString}[/]");

    public void Error(FormattableString formattableString)
        => AnsiConsole.MarkupLine($"[#{_palete.Error.ToHex()}]{formattableString}[/]");

    public void StandardError(string stdErr)
        => AnsiConsole.MarkupLine($"[#{_palete.Error.ToHex()}]{stdErr}[/]");

    public void StandardOutput(string stdOut)
        => AnsiConsole.WriteLine(stdOut);

    public void SwitchToAlternateBuffer()
        => AnsiConsole.WriteLine("\e[?1049h");

    public void SwitchToMainBuffer()
        => AnsiConsole.WriteLine("\e[?1049l");

    public void WaitKey()
    {
        AnsiConsole.MarkupLine($"[#{_palete.Accent.ToHex()}]Press any key to continue...[/]");
        Console.ReadKey();
    }

    public void Line() 
        => AnsiConsole.Write(new Rule());
}