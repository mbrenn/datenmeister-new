using DatenMeister.Core;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Shared;

public class _Layout : PageModel
{
    private readonly IScopeStorage _scopeStorage;

    public _Layout(IScopeStorage scopeStorage)
    {
        _scopeStorage = scopeStorage;
    }
        
    public void OnGet()
    {
            
    }
}