using System;
using System.Diagnostics;
using System.Globalization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DatenMeister.Integration;
using DatenMeister.Modules;
using DatenMeister.Web.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DatenMeister.Web
{
    public class DatenMeisterWebStartup
    {
        private IContainer _container;
        private ILifetimeScope _localScope;

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            _container = CreateKernel(containerBuilder);
            return new AutofacServiceProvider(_container);
        }

        public void Configure(IApplicationLifetime lifetime)
        {
            lifetime.ApplicationStopped.Register(OnApplicationStop);
        }

        private IContainer CreateKernel(ContainerBuilder kernel)
        {
            var settings = new IntegrationSettings
            {
                EstablishDataEnvironment = true
            };
            
            kernel.RegisterInstance(new WebserverStartupPhases()).As<IWebserverStartupPhases>();
            kernel.RegisterWebModules();
            var container = kernel.UseDatenMeister(settings);
            _localScope = container.BeginLifetimeScope("DatenMeister Webapplication");

            return container;
        }

        /// <summary>
        /// Called if the application is stopped
        /// </summary>
        private void OnApplicationStop()
        {
            _localScope.UnuseDatenMeister();
        }
    }
}