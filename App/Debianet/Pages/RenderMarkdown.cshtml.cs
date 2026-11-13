using Debianet.ServiceContract;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Debianet.Pages;

public class RenderMarkdownModel : PageModel
{
    public string RenderedHtml { get; set; } = string.Empty;

    private readonly IEmbededdFileProvider _embededdFileProvider;

    public RenderMarkdownModel(IEmbededdFileProvider embededdFileProvider)
    {
        _embededdFileProvider = embededdFileProvider;
    }

    public void OnGet(string doc)
    {
        switch (doc)
        {
            case "changelog":
                RenderedHtml = Markdig.Markdown.ToHtml(_embededdFileProvider.GetChangeLogMarkdown());
                break;
            case "readme":
                RenderedHtml = Markdig.Markdown.ToHtml(_embededdFileProvider.GetReadmeMd());
                break;
                case "license":
                RenderedHtml = Markdig.Markdown.ToHtml(GetLicense());
                break;
            default:
                RenderedHtml = "<p>Document not found.</p>";
                break;
        }
    }

    private string GetLicense()
    {
        return $"""
            ```
            {_embededdFileProvider.GetLicenseText()}
            ```
            """;
    }
}
