using DatenMeister.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Shared;

public class _Layout(IScopeStorage scopeStorage) : PageModel
{
    private readonly IScopeStorage _scopeStorage = scopeStorage;
    
    /// <summary>
    /// Gets or sets the value if the top bar shall not be shown for minimal design
    /// </summary>
    public bool HideTopBar { get; set; } = false;
    
    /// <summary>
    /// Gets or sets the value if the side bar shall not be shown for minimal design
    /// </summary>
    public bool HideSideBar { get; set; } = false;
}