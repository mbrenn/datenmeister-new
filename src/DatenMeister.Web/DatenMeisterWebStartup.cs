using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DatenMeister.Integration;
using DatenMeister.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DatenMeister.Web
{
    public class DatenMeisterWebStartup
    {
        
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            var container = CreateKernel(containerBuilder);
            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

        }

        private static IContainer CreateKernel(ContainerBuilder kernel)
        {
            var settings = new IntegrationSettings
            {
                EstablishDataEnvironment = true
            };
            
            kernel.RegisterInstance(new WebserverStartupPhases()).As<IWebserverStartupPhases>();
            var container = kernel.UseDatenMeister(settings);
            return container;
        }
    }
}