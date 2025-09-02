using DatenMeister.WebServer.Library.ServerConfiguration;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Pages;

public class IndexModel(ILogger<IndexModel> logger) : PageModel
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