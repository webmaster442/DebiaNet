using System.Reflection;

using DebiaNetApp.Properties;

using Spectre.Console;

namespace DebiaNetApp.Infrastructure;

internal static class Ui
{
    public static void Error(string message)
        => AnsiConsole.MarkupLine($"[red bold]{message.EscapeMarkup()}[/]");

    public static void DiplayCmd(string commandText)
    {
        AnsiConsole.MarkupLine($"[teal italic]>{Resources.Info_Executing}: {commandText.EscapeMarkup()}...[/]");
        AnsiConsole.WriteLine();
    }

    public static void Finished(TimeSpan runTime, int exitCode)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[grey]{Resources.Info_Runtime} {runTime.TotalSeconds}[/]");
        AnsiConsole.MarkupLine($"[grey]{Resources.Info_ExitCode} {exitCode}[/]");
    }

    public static void StandardOutput(string line)
        => AnsiConsole.MarkupLine($"[green]{line.EscapeMarkup()}[/]");

    public static void StandardErrorOutput(string line)
        => AnsiConsole.MarkupLine($"[red]{line.EscapeMarkup()}[/]");

    public static TMenuItem SelectorMenu<TMenuItem>(IEnumerable<TMenuItem> menuItems)
        where TMenuItem : SelectorMenuItem
    {
        var prompt = new SelectionPrompt<TMenuItem>()
            .Title(Resources.Prompt_SelectAnOption)
            .PageSize(10)
            .AddChoices(menuItems)
            .UseConverter(item => $"{item.Icon} {item.DisplayText}");

        return prompt.Show(AnsiConsole.Console);
    }

    private static readonly Dictionary<Type, PropertyInfo[]> PropertyCache = new();


    public static void DisplayAsTable<T>(T item)
    {
        var table = new Table();
        table.AddColumns(Resources.Table_Header_Property, Resources.Table_Header_Value);
        if (!PropertyCache.TryGetValue(typeof(T), out PropertyInfo[]? properties))
        {
            properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyCache[typeof(T)] = properties;
        }

        foreach (var property in properties)
        {
            table.AddRow(property.Name, property.GetValue(item)?.ToString().EscapeMarkup() ?? "<null>");
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }
}

