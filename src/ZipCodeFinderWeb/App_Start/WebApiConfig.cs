using DatenMeister.App.ZipCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ZipCodeFinderWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web-API-Konfiguration und -Dienste

            // Web-API-Routen
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
                        
            
            var dataPath =Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data/plz.csv");
            using (var stream = new FileStream(dataPath, FileMode.Open))
            {
                DataProvider.TheOne.LoadZipCodes(stream);
            }
        }
    }
}
