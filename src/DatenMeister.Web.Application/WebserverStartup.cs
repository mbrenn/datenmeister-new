﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using BurnSystems.Owin.StaticFiles;
using DatenMeister.Apps.ZipCode;
using DatenMeister.Full.Integration;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.FactoryMapper;
using DatenMeister.Web.Api;
using Microsoft.Owin;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
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
            app.UseStaticFiles(configuration);

            // Need to load the extentcontroller
            var extentControllerType = typeof (ExtentController);
            
            // Initializing of the WebAPI, needs to be called after the DatenMeister is initialized
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.MapHttpAttributeRoutes();
            app.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(httpConfiguration);
        }

        private static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.FillForDatenMeister();

            // Initialize start
            var workSpaceCollection = kernel.Get<WorkspaceCollection>();

            // Initializes the DatenMeister
            workSpaceCollection.Init();
            var workspaceData = workSpaceCollection.Workspaces.First(x => x.id == "Data");

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

            return kernel;
        }
    }
}
