using Debianet.Domain;
using Debianet.Domain.Dto;
using Debianet.ServiceContract;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Debianet.Pages.Docker;

internal class DockerImagesModel : PageModel
{
    private readonly IDockerService _dockerService;

    public IReadOnlyList<DockerImageResult> Images { get; set; } = null!;

    public DockerImagesModel(IDockerService dockerService)
    {
        _dockerService = dockerService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var imagesResult = await _dockerService.GetImagesAsync(Request.HttpContext.RequestAborted);

        if (imagesResult.Value is Error error)
        {
            Images = Array.Empty<DockerImageResult>();
            return BadRequest(error.Message);
        }

        if (imagesResult.Value is not IReadOnlyList<DockerImageResult> images)
        {
            Images = Array.Empty<DockerImageResult>();
            return BadRequest("Unexpected result type.");
        }

        Images = images;

        return Page();
    }
}
