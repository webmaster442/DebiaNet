using System.Net;
using System.Web;

using Debianet.Domain;
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
        Response.ContentType = "text/html; charset=utf-8";

        await Response.WriteAsync($$"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="utf-8" />
                <meta name="viewport" content="width=device-width, initial-scale=1.0" />
                <title>{{HttpUtility.HtmlEncode(title)}}</title>
                <style>
                body {
                    background: #282A36;
                    color: #F8F8F2;
                }
                .error {
                    color: #FF5555;
                }
                </style>
            </head>
            <body>
                <pre>
            """);

        await Response.Body.FlushAsync();

        async Task WriteAndFlushAsync(string html)
        {
            await Response.WriteAsync(html);
            await Response.Body.FlushAsync();
        }

        CommandBundle? commands = _commandRunner.CreateCommands(action);
        if (commands == null)
        {
            await Response.WriteAsync($"<span class=\"error\">Unknown action: {WebUtility.HtmlEncode(action)}</span>");
            await Response.WriteAsync("""
                </pre><button onclick='history.back()'>Go back...</button>
            </body>
            </html>
            """);

            await Response.Body.FlushAsync();

            return new EmptyResult();
        }

        await _commandRunner.RunCommandsAsync(commands,
            info: line => WriteAndFlushAsync($"<h3>{WebUtility.HtmlEncode(line)}</h3><hr/>{Environment.NewLine}"),
            stdErr: line => WriteAndFlushAsync($"<span class=\"error\">{WebUtility.HtmlEncode(line)}</span>{Environment.NewLine}"),
            stdOut: line => WriteAndFlushAsync($"{WebUtility.HtmlEncode(line)}{Environment.NewLine}"),
            cancellationToken: HttpContext.RequestAborted);

        await Response.WriteAsync("""
                </pre><button onclick='history.back()'>Go back...</button>
            </body>
            </html>
            """);

        await Response.Body.FlushAsync();

        return new EmptyResult();
    }
}
