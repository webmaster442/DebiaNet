using System.Web;

namespace Debianet.DomainService;

public sealed class HtmlStreamer
{
    private readonly HttpResponse _response;

    public HtmlStreamer(HttpResponse response)
    {
        _response = response;
    }

    public async Task BegininAsync(string title)
    {
        _response.ContentType = "text/html; charset=utf-8";

        await _response.WriteAsync($$"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="utf-8" />
                <meta name="viewport" content="width=device-width, initial-scale=1.0" />
                <title>{{HttpUtility.HtmlEncode(title)}}</title>
                <link rel="stylesheet" href="picnic.min.css">
                <link rel="stylesheet" href="style.css">
            </head>
            <body style="margin-left: 1rem; background: #f5f5f5;">
            """);

        await _response.Body.FlushAsync();
    }

    private async Task Scroll()
    {
        //await _response.WriteAsync("<script>window.scrollTo({ top: document.body.scrollHeight, behavior: \"smooth\" });</script>");
        //await _response.WriteAsync(Environment.NewLine);
    }

    public async Task UnknownActionAsync(string action)
    {
        await WriteErrorLineAsync($"Unknown action: {action}");
        await EndAsync(backButton: true);
    }

    public async Task BeginPreAsync()
    {
        await _response.WriteAsync("<div class=\"preformatted\">");
        await Scroll();
        await _response.Body.FlushAsync();
    }

    public async Task EndPreAsync()
    {
        await _response.WriteAsync("</div>");
        await Scroll();
        await _response.Body.FlushAsync();
    }

    public async Task HeaderAsync(string header)
    {
        await _response.WriteAsync($"<h2>{HttpUtility.HtmlEncode(header)}</h2>{Environment.NewLine}");
        await Scroll();
        await _response.Body.FlushAsync();
    }

    public async Task WriteLineAsync(string line)
    {
        await _response.WriteAsync($"{HttpUtility.HtmlEncode(line)}{Environment.NewLine}");
        await Scroll();
        await _response.Body.FlushAsync();
    }

    public async Task WriteErrorLineAsync(string line)
    {
        await _response.WriteAsync($"<span class=\"error\">{HttpUtility.HtmlEncode(line)}</span><br/>{Environment.NewLine}");
        await Scroll();
        await _response.Body.FlushAsync();
    }

    public async Task EndAsync(bool backButton = true)
    {
        if (backButton)
        {
            await _response.WriteAsync("""
                <hr/>
                <button onclick='history.back()'>Go back...</button>
            """);
        }

        await Scroll();

        await _response.WriteAsync("""
            </body>
            </html>
            """);

        await _response.Body.FlushAsync();
    }
}
