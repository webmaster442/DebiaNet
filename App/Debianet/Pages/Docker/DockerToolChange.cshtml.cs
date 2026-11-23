using Debianet.Domain;
using Debianet.DomainService;
using Debianet.ServiceContract;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Debianet.Pages.Docker;

internal class DockerToolChangeModel : PageModel
{
    private readonly IDebianetDataProvider _debianetDataProvider;
    private readonly ICommandRunner _commandRunner;
    private readonly IFileSystemService _fileSystemService;
    private readonly HashSet<string> _knownContainers;

    public DockerToolChangeModel(IDebianetDataProvider debianetDataProvider, 
                                 ICommandRunner commandRunner,
                                 IFileSystemService fileSystemService)
    {
        _debianetDataProvider = debianetDataProvider;
        _commandRunner = commandRunner;
        _fileSystemService = fileSystemService;
        _knownContainers = debianetDataProvider.DockerTools.Select(x => x.Name).ToHashSet();
    }

    [BindProperty]
    public string ToolName { get; set; } = string.Empty;

    [BindProperty]
    public string? ContainerId { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!_debianetDataProvider.DockerTools.Any(x => x.Name == ToolName))
        {
            ModelState.AddModelError(nameof(ToolName), "Unknown tool name.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var tool = _debianetDataProvider.DockerTools.First(x => x.Name == ToolName);

        HtmlStreamer stream = new(Response);
        await stream.BegininAsync("Modifying Docker tools");
        await stream.BeginPreAsync();

        if (!_fileSystemService.FileExists(tool.Compose))
        {
            await stream.WriteErrorLineAsync($"Compose file '{tool.Compose}' does not exist.");
            await stream.EndPreAsync();
            await stream.EndAsync(backButton: true);
            return new EmptyResult();
        }

        var file = _fileSystemService.ResolvePath(tool.Compose)!;
        CommandBundle bundle = CreateCommands(file, ContainerId);

        await _commandRunner.RunCommandsAsync(bundle,
            info: line => stream.HeaderAsync(line),
            stdErr: line => stream.WriteErrorLineAsync(line),
            stdOut: line => stream.WriteLineAsync(line),
            cancellationToken: HttpContext.RequestAborted);

        await stream.EndPreAsync();
        await stream.EndAsync(backButton: true);

        return new EmptyResult();
    }

    private static CommandBundle CreateCommands(string file, string? containerId)
    {
        var builder = CommandBundle.Builder();
        return string.IsNullOrEmpty(containerId)
            ? builder.Command("docker", ["compose", "-f", file, "up", "-d"]).Build()
            : builder.Command("docker", ["compose", "-f", file, "down"])
                     .Build();
    }
}
