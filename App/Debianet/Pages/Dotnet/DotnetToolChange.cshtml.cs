using Debianet.Domain;
using Debianet.Domain.ViewModel;
using Debianet.DomainService;
using Debianet.ServiceContract;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Debianet.Pages.Dotnet;

internal sealed class DotnetToolChangeModel : PageModel
{
    private readonly IDotnetToolService _dotnetToolService;
    private readonly IDebianetDataProvider _debianetDataProvider;
    private readonly ICommandRunner _commandRunner;
    private readonly HashSet<string> _knownTools;

    public DotnetToolChangeModel(IDotnetToolService dotnetToolService,
                                 IDebianetDataProvider debianetDataProvider,
                                 ICommandRunner commandRunner)
    {
        _dotnetToolService = dotnetToolService;
        _debianetDataProvider = debianetDataProvider;
        _commandRunner = commandRunner;
        _knownTools = _debianetDataProvider.DotnetTools.Select(tool => tool.Name).ToHashSet();
    }

    [BindProperty]
    public DonetToolSelectionViewModel[] Tools { get; set; } = [];

    public async Task<IActionResult> OnPostAsync()
    {
        foreach (var tool in Tools)
        {
            if (!_knownTools.Contains(tool.Name))
            {
                ModelState.AddModelError($"{tool.Name}", "Unknown tool name.");
            }
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var currentlyInstalled = await _dotnetToolService.GetInstalledToolsAsync(Request.HttpContext.RequestAborted);

        if (currentlyInstalled.Value is Error error)
        {
            return BadRequest(error.Message);
        }
        
        if (currentlyInstalled.Value is not IReadOnlySet<string> installedTools)
        {
            return BadRequest("Unexpected result from GetInstalledToolsAsync.");
        }

        List<string> toolsToInstall = Tools
            .Where(t => t.IsChecked && !installedTools.Contains(t.Name))
            .Select(t => t.Name)
            .ToList();

        List<string> toolsToUninstall = Tools
            .Where(t => !t.IsChecked && installedTools.Contains(t.Name))
            .Select(t => t.Name)
            .ToList();

        HtmlStreamer stream = new(Response);
        await stream.BegininAsync("Modifying .NET Tools");

        CommandBundle toolCommands = _dotnetToolService.CreateInstallBundle(toolsToInstall, toolsToUninstall);

        await stream.BeginPreAsync();

        await _commandRunner.RunCommandsAsync(toolCommands,
            info: line => stream.HeaderAsync(line),
            stdErr: line => stream.WriteErrorLineAsync(line),
            stdOut: line => stream.WriteLineAsync(line),
            cancellationToken: HttpContext.RequestAborted);


        await stream.EndPreAsync();
        await stream.EndAsync(backButton: true);

        return new EmptyResult();
    }
}
