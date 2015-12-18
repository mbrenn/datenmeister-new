using System;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;
using BurnSystems.Owin.StaticFiles;
using DatenMeister.App.ZipCode;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DatenMeister.Web.Application.WebserverStartup))]
namespace DatenMeister.Web.Application
{
    public class WebserverStartup
    {
        public void Configuration(IAppBuilder app)
        {
#if DEBUG
            app.UseErrorPage();
#endif

            var directory = "htdocs";
#if DEBUG
            if (Directory.Exists("..\\..\\htdocs"))
            {
                directory = "..\\..\\htdocs";
            }
#endif
            var configuration = new StaticFilesConfiguration(directory);
            configuration.AddIgnoredExtension(".ts");
            app.UseStaticFiles(configuration);

            StartDatenmeister();

            // Initializing of the WebAPI, needs to be called after the DatenMeister is initialized
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.MapHttpAttributeRoutes();
            app.UseWebApi(httpConfiguration);
        }

        private static void StartDatenmeister()
        {
            // Initializes the DatenMeister
            Core.TheOne.Init();

            var workspaceData = Core.TheOne.Workspaces.First(x => x.id == "Data");

            var file = Path.Combine(
                Path.Combine(
                    AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                    "App_Data"),
                "plz.csv");
            using (var stream = new FileStream(file, FileMode.Open))
            {
                DataProvider.TheOne.LoadZipCodes(stream);
                workspaceData.AddExtent(DataProvider.TheOne.ZipCodes);
            }
        }
    }
}
