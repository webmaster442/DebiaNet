using Debianet.Domain;
using Debianet.Domain.Dto;
using Debianet.ServiceContract;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Debianet.Pages.Docker;

internal class DockerPsModel : PageModel
{
    private readonly IDockerService _dockerService;

    public IReadOnlyList<DockerPsResult> DockerPs { get; set; } = null!;

    public DockerPsModel(IDockerService dockerService)
    {
        _dockerService = dockerService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var dockerPsResult = await _dockerService.GetPsAsync(Request.HttpContext.RequestAborted);

        if (dockerPsResult.Value is Error error)
        {
            // Handle the error appropriately, e.g., log it or display a message to the user
            DockerPs = Array.Empty<DockerPsResult>();
            return BadRequest(error.Message);
        }

        if (dockerPsResult.Value is not IReadOnlyList<DockerPsResult> dockerPs)
        {
            DockerPs = Array.Empty<DockerPsResult>();
            return BadRequest("Unexpected result type.");
        }

        DockerPs = dockerPs;

        return Page();
    }
}
