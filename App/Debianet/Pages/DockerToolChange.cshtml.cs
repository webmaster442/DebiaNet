using Debianet.ServiceContract;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Debianet.Pages;

internal class DockerToolChangeModel : PageModel
{
    private readonly IDebianetDataProvider _debianetDataProvider;
    private readonly ICommandRunner _commandRunner;
    private readonly HashSet<string> _knownContainers;

    public DockerToolChangeModel(IDebianetDataProvider debianetDataProvider, 
                                 ICommandRunner commandRunner)
    {
        _debianetDataProvider = debianetDataProvider;
        _commandRunner = commandRunner;
        _knownContainers = debianetDataProvider.DockerTools.Select(x => x.Name).ToHashSet();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        return new EmptyResult();
    } 

}
