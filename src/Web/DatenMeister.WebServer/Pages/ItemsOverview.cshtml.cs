using DatenMeister.Core.Interfaces;
using DatenMeister.WebServer.Library.Helper;
using DatenMeister.WebServer.Shared;
using Microsoft.AspNetCore.Components;

namespace DatenMeister.WebServer.Pages;

public class ExtentOverviewModel(ILogger<ExtentOverviewModel> logger, IScopeStorage scopeStorage) : _Layout(scopeStorage)
{
    private readonly ILogger<ExtentOverviewModel> _logger = logger;


    [Parameter] public string Workspace { get; set; } = string.Empty;

    [Parameter] public string Extent { get; set; } = string.Empty;

    [Parameter] public string? Item { get; set; } = string.Empty;

    public void OnGet(string workspace, string extent, string? item)
    {
        Workspace = MvcUrlEncoder.DecodePathOrEmpty(workspace);
        Extent = MvcUrlEncoder.DecodePathOrEmpty(extent);
        Item = MvcUrlEncoder.DecodePath(item);
    }
}