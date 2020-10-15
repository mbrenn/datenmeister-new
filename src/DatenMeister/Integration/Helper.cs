#nullable enable

using Autofac;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Workspaces.Data;

namespace DatenMeister.Integration
{
    /// <summary>
    /// Implements home helper classes
    /// </summary>
    public static class Helper
    {
        public static IDatenMeisterScope UseDatenMeister(this ContainerBuilder kernel, IntegrationSettings settings)
        {
            var integration = new Integrator(settings);
            return integration.UseDatenMeister(kernel);
        }

        /// <summary>
        /// Stores all data that needs to be stored persistent on the hard drive
        /// This method is typically called at the end of the lifecycle of the application
        /// </summary>
        /// <param name="scope">Kernel to be used to find the appropriate methods</param>
        public static void UnuseDatenMeister(this IDatenMeisterScope scope)
        {
            var integrationSettings = scope.ScopeStorage.Get<IntegrationSettings>();
            if (!integrationSettings.IsReadOnly)
            {
                scope.Resolve<WorkspaceLoader>().Store();
            }
            
            scope.Resolve<ExtentManager>().UnloadManager(true);
        }
    }
}