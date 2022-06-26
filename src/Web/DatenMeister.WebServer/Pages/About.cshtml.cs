using Autofac;
using DatenMeister.Core;
using DatenMeister.Integration.DotNet;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Pages
{
    public class AboutModel : PageModel
    {
        public void OnGet()
        {
        }

        public string WorkspacePath
        {
            get
            {
                var integrationSettings = GiveMe.Scope.Resolve<IntegrationSettings>();
                return integrationSettings.DatabasePath;
            }
        }
    }
}