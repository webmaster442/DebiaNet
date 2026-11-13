using Debianet.Domain;
using Debianet.DomainService;
using Debianet.ServiceContract;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Debianet.Pages;

internal class PerformActionModel : PageModel
{
    private readonly ICommandRunner _commandRunner;

    public PerformActionModel(ICommandRunner commandRunner)
    {
        _commandRunner = commandRunner;
    }

    public async Task<IActionResult> OnGetAsync(string action, string title)
    {
        HtmlStreamer stream = new(Response);
        await stream.BegininAsync(title);

        CommandBundle? commands = _commandRunner.CreateCommands(action);
        if (commands == null)
        {
            await stream.HeaderAsync("Error");
            await stream.UnknownActionAsync(action);
            return new EmptyResult();
        }

        await stream.BeginPreAsync();

        await _commandRunner.RunCommandsAsync(commands,
            info: line => stream.HeaderAsync(line),
            stdErr: line => stream.WriteErrorLineAsync(line),
            stdOut: line => stream.WriteLineAsync(line),
            cancellationToken: HttpContext.RequestAborted);


        await stream.EndPreAsync();

        await stream.EndAsync(backButton: true);

        await Response.Body.FlushAsync();

        return new EmptyResult();
    }
}
