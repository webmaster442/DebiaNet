using Debianet.Domain.ViewModel;
using Debianet.ServiceContract;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Debianet.Pages;

internal class DockerToolsModel : PageModel
{
    private readonly IDebianetDataProvider _dataProvider;

    public DockerToolViewModel[] DockerTools { get; set; } = null!;

    public DockerToolsModel(IDebianetDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    public void OnGet()
    {
        DockerTools = _dataProvider.DockerTools.Select(x => new DockerToolViewModel
        {
            Name = x.Name,
            Description = x.Description,
            Url = x.Url,
            Link = x.Link,
            ContainerId = null,
        }).ToArray();
    }
}
