using System.Collections.Generic;
using System.Linq;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Extent.Manager.Extents.Configuration;
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

        public static List<string> KnownExtentTypes
        {
            get
            {
                var extentSettings = GiveMe.Scope.ScopeStorage.Get<ExtentSettings>();
                return extentSettings.extentTypeSettings.Select(x => x.name).ToList();
            }
        }
    }
}