using Autofac;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Workspaces.Data;

namespace DatenMeister.Integration
{
    public static class Helper
    {
        public static IContainer UseDatenMeister(this ContainerBuilder kernel, IntegrationSettings settings)
        {
            var integration = new Integrator(settings);
            return integration.UseDatenMeister(kernel);
        }

        /// <summary>  
        /// Stores all data that needs to be stored persistant on the hard drive  
        /// This method is typically called at the end of the lifecycle of the applciation  
        /// </summary>  
        /// <param name="scope">Kernel to be used to find the appropriate methods</param>  
        public static void UnuseDatenMeister(this ILifetimeScope scope)
        {
            scope.Resolve<WorkspaceLoader>().Store();
            scope.Resolve<ExtentConfigurationLoader>().StoreAllExtents();
        }
    }
}