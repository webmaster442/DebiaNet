using System.Text;

using Debianet.Abstractions;

using Spectre.Console;

namespace Debianet.Ui;

internal sealed class TextViewer
{
    private const int TabSize = 8;

    private readonly Palete _palete;
    private readonly IReadOnlyList<string> _lines;
    private readonly string _title;

    public TextViewer(string text, string? title = null, Palete? palete = null)
    {
        _lines = SplitIntoLines(text ?? string.Empty);
        _title = title ?? string.Empty;
        _palete = palete ?? Palete.Dracula;
    }

    public void Show()
    {
        // Move to the alternate screen buffer so the original terminal content is preserved.
        AnsiConsole.WriteLine("\e[?1049h");

        try
        {
            Console.CursorVisible = false;
            Console.Out.Write("\e[?7l\e[2J\e[H"); // disable line wrapping, clear, home

            Loop();
        }
        finally
        {
            // Restore the main screen buffer first so the terminal is usable even on failure.
            AnsiConsole.WriteLine("\e[?1049l");
            Console.Out.Write("\e[?7h"); // re-enable line wrapping
            Console.CursorVisible = true;
        }
    }

    private void Loop()
    {
        int top = 0;
        int renderedWidth = -1;
        int renderedHeight = -1;
        List<string> visibleLines = [];
        bool redraw = true;

        while (true)
        {
            int width = Math.Max(1, Console.WindowWidth);
            int height = Math.Max(2, Console.WindowHeight);
            int viewport = height - 1; // the last row is reserved for the status bar

            if (width != renderedWidth)
            {
                visibleLines = WrapLines(_lines, width);
                renderedWidth = width;
                redraw = true;
            }

            if (height != renderedHeight)
            {
                renderedHeight = height;
                redraw = true;
            }

            int maxTop = Math.Max(0, visibleLines.Count - viewport);
            top = Math.Clamp(top, 0, maxTop);

            if (redraw)
            {
                Render(visibleLines, top, viewport, width, height);
                redraw = false;
            }

            ConsoleKeyInfo key = Console.ReadKey(intercept: true);
            switch (key.Key)
            {
                case ConsoleKey.Escape:
                case ConsoleKey.Q:
                    return;
                case ConsoleKey.UpArrow:
                case ConsoleKey.K:
                    redraw = TryScroll(ref top, -1, maxTop);
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.J:
                    redraw = TryScroll(ref top, 1, maxTop);
                    break;
                case ConsoleKey.PageUp:
                    redraw = TryScroll(ref top, -viewport, maxTop);
                    break;
                case ConsoleKey.PageDown:
                case ConsoleKey.Spacebar:
                    redraw = TryScroll(ref top, viewport, maxTop);
                    break;
                case ConsoleKey.Home:
                    redraw = TryScroll(ref top, int.MinValue, maxTop);
                    break;
                case ConsoleKey.End:
                    redraw = TryScroll(ref top, int.MaxValue, maxTop);
                    break;
            }
        }
    }

    private static bool TryScroll(ref int top, int delta, int maxTop)
    {
        int target = delta switch
        {
            int.MinValue => 0,
            int.MaxValue => maxTop,
            _ => top + delta,
        };

        target = Math.Clamp(target, 0, maxTop);
        if (target == top)
        {
            return false;
        }

        top = target;
        return true;
    }

    private void Render(IReadOnlyList<string> visibleLines, int top, int viewport, int width, int height)
    {
        StringBuilder frame = new();

        for (int row = 0; row < viewport; row++)
        {
            frame.Append("\e[").Append(row + 1).Append(";1H"); // move the caret to the start of the row

            int index = top + row;
            if (index < visibleLines.Count)
            {
                string line = visibleLines[index];
                frame.Append(line.Length > width ? line[..width] : line);
            }

            frame.Append("\e[K"); // clear leftovers from the previous frame
        }

        frame.Append("\e[").Append(height).Append(";1H"); // park the caret on the status row
        Console.Out.Write(frame.ToString());

        string status = BuildStatusBar(visibleLines.Count, top, viewport, width);
        AnsiConsole.Markup($"[#{_palete.Accent.ToHex()} on #{_palete.Info.ToHex()}]{Markup.Escape(status)}[/]");
    }

    private string BuildStatusBar(int totalLines, int top, int viewport, int width)
    {
        string position;
        if (totalLines == 0)
        {
            position = "(empty)";
        }
        else
        {
            int first = top + 1;
            int last = Math.Min(totalLines, top + viewport);
            int maxTop = Math.Max(0, totalLines - viewport);
            int percent = maxTop == 0 ? 100 : (int)Math.Round((100.0 * top) / maxTop);
            position = $"Lines {first}-{last}/{totalLines} ({percent}%)";
        }

        string left = string.IsNullOrWhiteSpace(_title) ? position : $"{_title}  |  {position}";
        const string help = "Up/Down scroll  PgUp/PgDn page  Home/End jump  Esc quit";

        int gap = width - left.Length - help.Length;
        string status = gap >= 2 ? $"{left}{new string(' ', gap)}{help}" : left;

        return status.Length > width ? status[..width] : status.PadRight(width);
    }

    private static List<string> WrapLines(IReadOnlyList<string> lines, int width)
    {
        List<string> wrapped = [];

        foreach (string raw in lines)
        {
            string line = ExpandAndSanitize(raw);
            if (line.Length == 0)
            {
                wrapped.Add(string.Empty);
                continue;
            }

            for (int start = 0; start < line.Length; start += width)
            {
                int length = Math.Min(width, line.Length - start);
                wrapped.Add(line.Substring(start, length));
            }
        }

        return wrapped;
    }

    private static string ExpandAndSanitize(string line)
    {
        StringBuilder builder = new(line.Length);
        int column = 0;

        foreach (char c in line)
        {
            if (c == '\t')
            {
                int spaces = TabSize - (column % TabSize);
                builder.Append(' ', spaces);
                column += spaces;
            }
            else if (c < ' ' || c == '\u007F')
            {
                // Show control characters in caret notation so an embedded escape
                // sequence cannot corrupt the rendered layout.
                builder.Append('^').Append(c == '\u007F' ? '?' : (char)(c + '@'));
                column += 2;
            }
            else
            {
                builder.Append(c);
                column++;
            }
        }

        return builder.ToString();
    }

    private static List<string> SplitIntoLines(string text)
        => text.Length == 0
            ? []
            : [.. text.ReplaceLineEndings("\n").Split('\n')];
}
