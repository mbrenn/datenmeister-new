using DatenMeister.WebServer.Library.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            Workspace = MvcUrlEncoder.DecodePathOrEmpty(workspace);
            Extent = MvcUrlEncoder.DecodePathOrEmpty(extent);
            Item = MvcUrlEncoder.DecodePath(item);
        }
    }
}