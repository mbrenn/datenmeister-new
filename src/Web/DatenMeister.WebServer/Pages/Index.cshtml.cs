using DatenMeister.Core.Interfaces;
using DatenMeister.WebServer.Library.ServerConfiguration;
using DatenMeister.WebServer.Shared;

namespace DatenMeister.WebServer.Pages;

public class IndexModel(ILogger<IndexModel> logger, IScopeStorage scopeStorage) : _Layout(scopeStorage)
{
    private readonly ILogger<IndexModel> _logger = logger;

    public void OnGet()
    {
        var startPage = WebServerSettingHandler.TheOne.WebServerSettings.startPage;
        if (!string.IsNullOrEmpty(startPage))
        {
            Response.Redirect(startPage);
        }
    }
}