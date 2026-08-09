using DatenMeister.Core.Interfaces;
using DatenMeister.WebServer.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Pages;

/// <summary>
/// A minimal page which just executes an action on the server and lets the returned
/// ClientActions render the actual content into the pageContent div. The page carries
/// no form of its own — it is a container which the action fills.
/// </summary>
public class ActionPureModel : _Layout
{
    public ActionPureModel(IScopeStorage scopeStorage) : base(scopeStorage)
    {
        HideTopBar = true;
        HideSideBar = true;
    }
    
    [Parameter] public string ActionVerb { get; set; } = string.Empty;

    [Parameter] public string Workspace { get; set; } = string.Empty;

    [Parameter] public string ActionUrl { get; set; } = string.Empty;

    public ActionResult OnGet(string actionName)
    {
        ActionVerb = Request.Query["verb"].ToString() ?? string.Empty;
        Workspace = Request.Query["workspace"].ToString() ?? string.Empty;
        ActionUrl = Request.Query["actionUrl"].ToString() ?? string.Empty;
        return Page();
    }
}
