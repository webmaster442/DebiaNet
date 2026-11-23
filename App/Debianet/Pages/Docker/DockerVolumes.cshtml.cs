using Debianet.Domain;
using Debianet.Domain.Dto;
using Debianet.ServiceContract;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Debianet.Pages.Docker;

internal class DockerVolumesModel : PageModel
{
    private readonly IDockerService _dockerService;

    public DockerVolumesModel(IDockerService dockerService)
    {
        _dockerService = dockerService;
    }

    public IReadOnlyList<DockerVolumeResult> Volumes { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        var volumes = await _dockerService.GetVolumesAsync(Request.HttpContext.RequestAborted);

        if (volumes.Value is Error error)
        {
            Volumes = Array.Empty<DockerVolumeResult>();
            return BadRequest(error.Message);
        }

        if (volumes.Value is not IReadOnlyList<DockerVolumeResult> volumeList)
        {
            Volumes = Array.Empty<DockerVolumeResult>();
            return BadRequest("Unexpected result type.");
        }

        Volumes = volumeList;

        return Page();
    }
}
