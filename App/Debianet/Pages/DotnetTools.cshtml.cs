using Debianet.Domain.ViewModel;
using Debianet.ServiceContract;

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

    public async Task OnGetAsync()
    {
        var installed = await _dotnetToolService.GetInstalledToolsAsync();
        Tools = _dataProvider.DotnetTools.Select(tool => new DotnetToolViewModel
        { 
            Name = tool.Name,
            Description = tool.Description,
            Link = tool.Link,
            IsChecked = installed.Contains(tool.Name)
        }).ToArray();
    }
}
