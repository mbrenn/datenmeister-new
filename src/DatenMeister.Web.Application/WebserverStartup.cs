using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using BurnSystems.Owin.StaticFiles;
using DatenMeister.CSV.Runtime.Storage;
using DatenMeister.Integration;
using DatenMeister.Integration.DotNet;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Web.Application;
using DatenMeister.Web.Models.Modules;
using DatenMeister.Web.Modules;
using Microsoft.Owin;
using Microsoft.Owin.BuilderProperties;
using Owin;

[assembly: OwinStartup(typeof(WebserverStartup))]
namespace DatenMeister.Web.Application
{
    public class WebserverStartup
    {
        private IContainer _serverInjection;
        private ILifetimeScope _lifetimeScope;

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
            
            // Do the full load of all assemblies
            LoadingHelper.LoadAllAssembliesFromCurrentDirectory();
            LoadingHelper.LoadAllReferencedAssemblies();
            LoadingHelper.LoadAssembliesFromFolder("plugins");
            
            // Initializing of the WebAPI, needs to be called after the DatenMeister is initialized
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.MapHttpAttributeRoutes();

            _serverInjection = CreateKernel(app);

            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebModules();
            builder.Update(_serverInjection);

            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(_serverInjection);

            _lifetimeScope = _serverInjection.BeginLifetimeScope("DatenMeister Webapplication");

            app.UseAutofacMiddleware(_lifetimeScope);

            var configuration = new StaticFilesConfiguration(directory);
            app.UseStaticFiles(configuration);
            app.UseAutofacWebApi(httpConfiguration);
            app.UseWebApi(httpConfiguration);

            _lifetimeScope
                .Resolve<IWebserverStartupPhases>()
                .OnAfterInitialization(_lifetimeScope);
        }

        private IContainer CreateKernel(IAppBuilder app)
        {
            var settings = new IntegrationSettings
            {
                PathToXmiFiles = "App_Data/Xmi",
                EstablishDataEnvironment = true
            };
            
            var kernel = new ContainerBuilder();
            kernel.RegisterInstance(new WebserverStartupPhases()).As<IWebserverStartupPhases>();
            var container = kernel.UseDatenMeisterDotNet(settings);

            // Defines the shutdown
            var properties = new AppProperties(app.Properties);
            var token = properties.OnAppDisposing;
            token.Register(() =>
            {
                _lifetimeScope.UnuseDatenMeister();
            });

            return container;
        }

        private static void LoadZipCodes(ILifetimeScope scope)
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

            var extentStorageLogic = scope.Resolve<IExtentStorageLoader>();
            extentStorageLogic.LoadExtent(defaultConfiguration, false);
            
            Debug.WriteLine("Zip codes loaded");
        }
    }
}
