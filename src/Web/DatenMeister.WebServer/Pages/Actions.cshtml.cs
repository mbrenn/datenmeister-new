using System;
using Autofac;
using DatenMeister.Core.Helper;
using DatenMeister.Integration.DotNet;
using DatenMeister.Modules.ZipCodeExample.Model;
using DatenMeister.Types;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Pages
{
    public class ActionsModel : PageModel
    {
        public void OnGet()
        {
        }

        public string GetZipCodeMetaClassUri()
        {
            var dm = GiveMe.Scope;
            var localTypeSupport = dm.Resolve<LocalTypeSupport>();
            return localTypeSupport.GetMetaClassFor(typeof(ZipCode))?.GetUri()
                   ?? throw new InvalidOperationException("Zipcode extension was not found");
        }
    }
}