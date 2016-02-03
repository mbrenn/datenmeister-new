﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.Http;
using BurnSystems.Owin.StaticFiles;
using DatenMeister.Apps.ZipCode;
using DatenMeister.CSV;
using DatenMeister.CSV.Runtime.Storage;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.Full.Integration;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.FactoryMapper;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Runtime.Workspaces.Data;
using DatenMeister.Web.Api;
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
        private StandardKernel serverInjection;

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
            app.UseStaticFiles(configuration);

            // Need to load the extentcontroller
            var extentControllerType = typeof (ExtentController);
            
            // Initializing of the WebAPI, needs to be called after the DatenMeister is initialized
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.MapHttpAttributeRoutes();

            serverInjection = CreateKernel(app);
            app.UseNinjectMiddleware(() => serverInjection).UseNinjectWebApi(httpConfiguration);

            // Loading and storing the workspaces
            var loaded = new WorkspaceStorage(serverInjection.Get<IWorkspaceCollection>(), "data/workspaces.xml");
            loaded.Load();
            serverInjection.Bind<WorkspaceStorage>().ToConstant(loaded);
        }

        private static StandardKernel CreateKernel(IAppBuilder app)
        {
            var kernel = new StandardKernel();
            kernel.UseDatenMeister();

            // Defines the shutdown
            var properties = new AppProperties(app.Properties);
            var token = properties.OnAppDisposing;
            token.Register(() =>
            {
                kernel.Get<IExtentStorageLogic>().StoreAll();
                kernel.Get<WorkspaceStorage>().Store();
            });

            // Loading the zipcodes
            LoadZipCodes(kernel);

            return kernel;
        }

        private static void LoadZipCodes(StandardKernel kernel)
        {
            //////////////////////
            // Loads the workspace
            var file = Path.Combine(
                Path.Combine(
                    AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                    "App_Data"),
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
                    Encoding = Encoding.UTF8
                }
            };

            var extentStorageLogic = kernel.Get<IExtentStorageLogic>();
            extentStorageLogic.LoadExtent(defaultConfiguration);
            
            Debug.WriteLine("Zip codes loaded");
        }
    }
}
