using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Spectre.Console;

namespace DebiaNetApp;

internal static class Tasks
{
    public static Task<int> Exit()
    {
        Environment.Exit(ExitCodes.Success);
        return Task.FromResult(ExitCodes.Success);
    }

    public static Task<int> EnvVars()
    {
        var sb = new StringBuilder();
        var table = new Table();
        table.AddColumns("Variable", "Value");

        foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
        {
            table.AddRow(de.Key.ToString() ?? "", de.Value?.ToString() ?? "<null>");
        }

        AnsiConsole.Write(table);

        return Task.FromResult(ExitCodes.Success);
    }
}