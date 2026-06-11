using System;
using System.Collections.Generic;
using System.Text;

using CliWrap;

namespace DebiaNetApp.Infrastructure;

internal class JsonCliMenuItem<TJsonDto> : SelectorMenuItem where TJsonDto : class
{
    private readonly List<TJsonDto> _dtos = new();

    public JsonCliMenuItem(string displayText, string icon) : base(displayText, icon)
    {
    }

    public required CliCommand Command { get; init; }

    public required Action<IEnumerable<TJsonDto>> ResultProcessor { get; init; }

    public override async Task<int> Execute()
    {
        Ui.DiplayCmd(Command.CommandLine);

        _dtos.Clear();

        var result = await Cli
               .Wrap(Command.Program)
               .WithArguments(Command.Arguments)
               .WithStandardOutputPipe(PipeTarget.ToDelegate(OnCaptureDto, Encoding.UTF8))
               .WithStandardErrorPipe(PipeTarget.ToDelegate(Ui.StandardErrorOutput, Encoding.UTF8))
               .ExecuteAsync();

        ResultProcessor.Invoke(_dtos);

        Ui.Finished(result.RunTime, result.ExitCode);

        return result.ExitCode;
    }

    private void OnCaptureDto(string output)
    {
        string json = output.Trim('"');
        TJsonDto? dto = System.Text.Json.JsonSerializer.Deserialize<TJsonDto>(json, System.Text.Json.JsonSerializerOptions.Default);
        if (dto != null)
        {
            _dtos.Add(dto);
        }
    }
}

/*
 using System.Reflection;
using System.Text;
using System.Text.Json;

using CliWrap;

using Spectre.Console;

namespace DebiaNet.Infrastructure;

internal sealed class JsonTableMenuItem<TItem> : CliMenuItem
    where TItem : class, IContainerId
{
    private readonly PropertyInfo[] _properties;

    public JsonTableMenuItem(string displayText, string icon) : base(displayText, icon)
    {
        _properties = typeof(TItem).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    public override async Task<int> Execute()
    {
        int finalExitCode = 0;

        foreach (var programItem in ProgramItems)
        {
            Ui.DiplayCmd(programItem.CliText);
            Command cmd = Cli
                .Wrap(programItem.Program)
                .WithArguments(programItem.Arguments)
                .WithStandardOutputPipe(PipeTarget.ToDelegate(OnAddRow, Encoding.UTF8))
                .WithStandardErrorPipe(PipeTarget.ToDelegate(OnStdErr, Encoding.UTF8));

            var result = await cmd.ExecuteAsync();

            finalExitCode &= result.ExitCode;
        }

        return finalExitCode;
    }

    private void OnAddRow(string output)
    {
        string json = output.Trim('"');

        var table = new Table();
        table.AddColumns("Property", "Value");

        TItem? item = JsonSerializer.Deserialize<TItem>(json, JsonSerializerOptions.Default);
        if (item != null)
        {
            AnsiConsole.MarkupLine($"[green]Container: {item.ID}[/]");
            var properties = _properties.ToDictionary(x => x.Name, x => x.GetValue(item));
            foreach (var prop in properties)
            {
                table.AddRow(prop.Key, prop.Value?.ToString()?.EscapeMarkup() ?? "<null>");
            }
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }
}
 */