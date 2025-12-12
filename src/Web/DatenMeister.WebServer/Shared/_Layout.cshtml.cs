using DatenMeister.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Shared;

public class _Layout(IScopeStorage scopeStorage) : PageModel
{
    private readonly IScopeStorage _scopeStorage = scopeStorage;

    public void OnGet()
    {
            
    }
}