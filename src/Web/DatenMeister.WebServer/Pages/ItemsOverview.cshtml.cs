using System.Web;
using DatenMeister.WebServer.Library.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
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
            Workspace = MvcUrlEncoder.DecodePath(workspace);
            Extent = MvcUrlEncoder.DecodePath(extent);
            Item = MvcUrlEncoder.DecodePath(item);
        }
    }
}