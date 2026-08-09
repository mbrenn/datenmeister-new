using System.Text;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.WebServer.Library.Helper;
using DatenMeister.WebServer.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Pages;

public class ItemModel(IScopeStorage scopeStorage) : _Layout(scopeStorage)
{
    public readonly StringBuilder ScriptLines = new();

    [Parameter] public string Workspace { get; set; } = string.Empty;

    [Parameter] public string ItemUrl { get; set; } = string.Empty;

    public IObject? FoundItem { get; set; }

    public IObject? Form { get; set; }

    public string ExtentUrl
    {
        get
        {
            var posHash = ItemUrl.IndexOf('#');
            if (posHash == -1)
                return ItemUrl;
            return ItemUrl.Substring(0, posHash);
        }
    }

    public void OnGet(string workspace, string itemUrl)
    {
        Workspace = MvcUrlEncoder.DecodePathOrEmpty(workspace);
        ItemUrl = MvcUrlEncoder.DecodePathOrEmpty(itemUrl);
    }
}