using Debianet.Domain;
using Debianet.Domain.Dto;
using Debianet.ServiceContract;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Debianet.Pages;

internal class DockerNetworksModel : PageModel
{
    private readonly IDockerService _dockerService;

    public IReadOnlyList<DockerNetworkResult> Networks { get; set; } = null!;

    public DockerNetworksModel(IDockerService dockerService)
    {
        _dockerService = dockerService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var networksResult = await _dockerService.GetNetworksAsync(Request.HttpContext.RequestAborted);

        if (networksResult.Value is Error error)
        {
            Networks = Array.Empty<DockerNetworkResult>();
            return BadRequest(error.Message);
        }

        if (networksResult.Value is not IReadOnlyList<DockerNetworkResult> networks)
        {
            Networks = Array.Empty<DockerNetworkResult>();
            return BadRequest("Unexpected result type.");
        }

        Networks = networks;
        
        return Page();
    }
}
