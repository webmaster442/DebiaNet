using Debianet.Domain;
using Debianet.Domain.Dto;
using Debianet.ServiceContract;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Debianet.Pages.Dotnet;

internal class DotnetGlobalToolsModel : PageModel
{
    private readonly IDotnetToolService _dotnetToolService;

    public IReadOnlyList<Domain.Dto.DotnetToolOutput> DotnetTools { get; set; } = null!;

    public DotnetGlobalToolsModel(IDotnetToolService dotnetToolService)
    {
        _dotnetToolService = dotnetToolService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var tools = await _dotnetToolService.GetDotnetToolListAsync(Request.HttpContext.RequestAborted);

        if (tools.Value is Error error)
        {
            DotnetTools = Array.Empty<Domain.Dto.DotnetToolOutput>();
            return BadRequest(error.Message);
        }

        if (tools.Value is not DotnetToolList toolList)
        {
            DotnetTools = Array.Empty<Domain.Dto.DotnetToolOutput>();
            return BadRequest("Unexpected result type.");
        }

        DotnetTools = toolList.Data;

        return Page();
    }
}
