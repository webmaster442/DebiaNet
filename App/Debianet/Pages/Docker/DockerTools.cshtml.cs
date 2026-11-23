using Debianet.Domain;
using Debianet.Domain.ViewModel;
using Debianet.ServiceContract;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Debianet.Pages.Docker;

internal class DockerToolsModel : PageModel
{
    private readonly IDebianetDataProvider _dataProvider;
    private readonly IDockerService _dockerService;
    private readonly IFileSystemService _fileSystemService;

    public List<DockerToolViewModel> DockerTools { get; set; } = null!;

    public DockerToolsModel(IDebianetDataProvider dataProvider,
                            IDockerService dockerService,
                            IFileSystemService fileSystemService)
    {
        _dataProvider = dataProvider;
        _dockerService = dockerService;
        _fileSystemService = fileSystemService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        DockerTools = new List<DockerToolViewModel>();
        foreach (var tool in _dataProvider.DockerTools)
        {
            string? composeFilePath = _fileSystemService.ResolvePath(tool.Compose);

            Result<string> containerId = await _dockerService.GetComposeContainerId(composeFilePath, HttpContext.RequestAborted);

            string? containerIdValue = containerId.Value is string value ? value : null;

            DockerTools.Add(new DockerToolViewModel
            {
                Name = tool.Name,
                Description = tool.Description,
                Url = tool.Url,
                Link = tool.Link,
                ContainerId = containerIdValue,
            });
        }

        return Page();
    }
}
