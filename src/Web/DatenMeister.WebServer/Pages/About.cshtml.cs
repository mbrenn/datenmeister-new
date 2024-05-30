using System.Collections.Generic;
using System.Linq;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Forms;
using DatenMeister.Integration.DotNet;
using DatenMeister.Plugins;
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

        public static List<string> LoadedPlugins
        {
            get
            {
                var pluginLoader = GiveMe.Scope.ScopeStorage.Get<PluginManager>();
                return  pluginLoader.PluginTypes!.Select(x => x.FullName!).OrderBy(x => x).ToList();
            }
        }

        /// <summary>
        /// Returns a list of strings containing the names of the types of the Form Modification Plugins
        /// </summary>
        public static List<string> FormModificationTypes
        {
            get
            {
                var formPlugin = GiveMe.Scope.ScopeStorage.Get<FormsPluginState>();
                return formPlugin.FormModificationPlugins
                    .Select(x => $"{x} ({x.GetType().FullName})")
                    .ToList();
            }
        }
    }
}