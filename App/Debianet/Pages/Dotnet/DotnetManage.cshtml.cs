using Debianet.Domain.ViewModel;
using Debianet.DomainService;
using Debianet.ServiceContract;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Debianet.Pages.Dotnet;

internal class DotnetManageModel : PageModel
{
    private readonly ICommandRunner _commandRunner;

    public DotnetManageModel(ICommandRunner commandRunner)
    {
        _commandRunner = commandRunner;
    }

    private async Task<string> RunAndGetStdout(string command, string[] arguments, int? lines, CancellationToken cancellationToken)
    {
        var result = await _commandRunner.CaptureCommandOutputAsync(command, arguments, lines, cancellationToken);
        return result.Value is string str ? str : "n/a";
    }

    public Table DotnetRuntimes { get; private set; } = default!;
    public Table DotnetSdks { get; private set; } = default!;
    public string DotnetWorkloads { get; private set; } = default!;
    public string DotnetTemplates { get; private set; } = default!;

    public async Task OnGetAsync()
    {
        var runtimes = await RunAndGetStdout("dotnet", ["--list-runtimes"], null, Request.HttpContext.RequestAborted);
        DotnetRuntimes = TableFactory.FromDotnetListRuntimes(runtimes);

        var sdks = await RunAndGetStdout("dotnet", ["--list-sdks"], null, Request.HttpContext.RequestAborted);
        DotnetSdks = TableFactory.FromDotnetListSdks(sdks);

        DotnetWorkloads = await RunAndGetStdout("dotnet", ["workload", "list"], null, Request.HttpContext.RequestAborted);

        DotnetTemplates = await RunAndGetStdout("dotnet", ["new", "--list"], null, Request.HttpContext.RequestAborted);
    }
}
