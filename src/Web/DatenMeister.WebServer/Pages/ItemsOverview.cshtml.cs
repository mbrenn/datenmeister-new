using System.Net;
using System.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DatenMeister.WebServer.Pages
{
    public class ExtentOverviewModel : PageModel
    {
        private readonly ILogger<ExtentOverviewModel> _logger;

        public ExtentOverviewModel(ILogger<ExtentOverviewModel> logger)
        {
            _logger = logger;
        }


        [Parameter] public string Workspace { get; set; } = string.Empty;

        [Parameter] public string Extent { get; set; } = string.Empty;

        [Parameter] public string? Item { get; set; } = string.Empty;

        public void OnGet(string workspace, string extent, string? item)
        {
            Workspace = HttpUtility.UrlDecode(workspace);
            Extent = HttpUtility.UrlDecode(extent);
            Item = HttpUtility.UrlDecode(item);
        }
    }
}