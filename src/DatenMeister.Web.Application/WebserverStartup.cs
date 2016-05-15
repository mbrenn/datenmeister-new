using System;
using System.Diagnostics;
using System.IO;
using System.Web.Http;
using BurnSystems.Owin.StaticFiles;
using DatenMeister.Apps.ZipCode;
using DatenMeister.CSV.Runtime.Storage;
using DatenMeister.Integration;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Runtime.Workspaces.Data;
using DatenMeister.XMI.ExtentStorage;
using Microsoft.Owin;
using Microsoft.Owin.BuilderProperties;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;

[assembly: OwinStartup(typeof(DatenMeister.Web.Application.WebserverStartup))]
namespace DatenMeister.Web.Application
{
    public class WebserverStartup
    {
        private StandardKernel _serverInjection;

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

            if (Directory.Exists("..\\..\\..\\htdocs"))
            {
                directory = "..\\..\\..\\htdocs";
            }
#endif
            var configuration = new StaticFilesConfiguration(directory);
            app.UseStaticFiles(configuration);
            
            // Do the full load of all assemblies
            Integration.Helper.LoadAllAssembliesFromCurrentDirectory();
            Integration.Helper.LoadAllReferencedAssemblies();
            Integration.Helper.LoadAssembliesFromFolder("plugins");
            
            // Initializing of the WebAPI, needs to be called after the DatenMeister is initialized
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.MapHttpAttributeRoutes();

            _serverInjection = CreateKernel(app);
            app.UseNinjectMiddleware(() => _serverInjection).UseNinjectWebApi(httpConfiguration);
        }

        private static StandardKernel CreateKernel(IAppBuilder app)
        {
            var settings = new IntegrationSettings
            {
                PathToXmiFiles = "App_Data/Xmi",
                EstablishDataEnvironment = true
            };

            var kernel = new StandardKernel();
            kernel.UseDatenMeister(settings);

            // Defines the shutdown
            var properties = new AppProperties(app.Properties);
            var token = properties.OnAppDisposing;
            token.Register(() =>
            {
                kernel.UnuseDatenMeister();
            });

            return kernel;
        }

        private static void LoadZipCodes(StandardKernel kernel)
        {
            //////////////////////
            // Loads the workspace
            var file = Path.Combine(
                Path.Combine(
                    AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                    "App_Data/Database"),
                "plz.csv");

            var defaultConfiguration = new CSVStorageConfiguration
            {
                ExtentUri = "datenmeister:///zipcodes",
                Path = file,
                Workspace = "Data",
                Settings =
                {
                    HasHeader = false,
                    Separator = '\t',
                    Encoding = "UTF-8"
                }
            };

            var extentStorageLogic = kernel.Get<IExtentStorageLoader>();
            extentStorageLogic.LoadExtent(defaultConfiguration, false);
            
            Debug.WriteLine("Zip codes loaded");
        }
    }
}
