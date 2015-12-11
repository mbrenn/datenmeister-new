using System.IO;
using System.Web.Routing;
using BurnSystems.Owin.StaticFiles;
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
            var routes = RouteTable.Routes;
        }
    }
}
