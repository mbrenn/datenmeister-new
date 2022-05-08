using System;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Core.Helper;
using DatenMeister.Integration.DotNet;
using DatenMeister.Modules.ZipCodeExample.Model;
using DatenMeister.Types;
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