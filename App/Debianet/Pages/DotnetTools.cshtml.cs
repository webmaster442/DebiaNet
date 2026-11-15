using Debianet.Domain;
using Debianet.Domain.ViewModel;
using Debianet.ServiceContract;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Debianet.Pages;

internal sealed class DotnetToolsModel : PageModel
{
    public DotnetToolViewModel[] Tools { get; set; } = null!;

    private readonly IDotnetToolService _dotnetToolService;
    private readonly IDebianetDataProvider _dataProvider;

    public DotnetToolsModel(IDotnetToolService dotnetToolService, IDebianetDataProvider debianetDataProvider)
    {
        _dotnetToolService = dotnetToolService;
        _dataProvider = debianetDataProvider;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var installed = await _dotnetToolService.GetInstalledToolsAsync(Request.HttpContext.RequestAborted);
        if (installed.Value is Error error)
        {
            // Handle the error appropriately, e.g., log it or display a message to the user
            Tools = Array.Empty<DotnetToolViewModel>();
            return BadRequest(error.Message);
        }
        if (installed.Value is not IReadOnlySet<string> installedTools)
        {
            // Handle the unexpected result appropriately
            Tools = Array.Empty<DotnetToolViewModel>();
            return BadRequest("Unexpected result from GetInstalledToolsAsync.");
        }
        Tools = _dataProvider.DotnetTools.Select(tool => new DotnetToolViewModel
        { 
            Name = tool.Name,
            Description = tool.Description,
            Link = tool.Link,
            IsChecked = installedTools.Contains(tool.Name)
        }).ToArray();

        return Page();
    }
}
