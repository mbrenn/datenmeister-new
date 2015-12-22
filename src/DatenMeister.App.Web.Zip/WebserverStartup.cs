using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using BurnSystems.Owin.StaticFiles;
using DatenMeister.App.Web.Zip;
using DatenMeister.Apps.ZipCode;
using DatenMeister.Web;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Owin;
using ZipCodeFinderWeb.Controllers;

[assembly: OwinStartup(typeof(WebserverStartup))]
namespace DatenMeister.App.Web.Zip
{
    public class WebserverStartup
    {
        public void Configuration(IAppBuilder app)
        {
            const string directory = "htdocs";

            var configuration = new StaticFilesConfiguration(directory);
            //configuration.AddIgnoredExtension(".ts");
            app.UseStaticFiles(configuration);

            // Initializes the DatenMeister
            Core.TheOne.Init();
            var dataPath = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data/plz.csv");
            using (var stream = new FileStream(dataPath, FileMode.Open))
            {
                DataProvider.TheOne.LoadZipCodes(stream);
            }

            // ReSharper disable once ObjectCreationAsStatement
            new ZipController();

            // We have a stage marker here, so the static files are handle
            app.UseStageMarker(PipelineStage.MapHandler);

            // Initializing of the WebAPI, needs to be called after the DatenMeister is initialized
            
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            app.UseWebApi(httpConfiguration);

            app.Use((context, next) =>
            {
                context.Response.StatusCode = 404;
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("UNKNOWN");
            });
        }
    }
}
