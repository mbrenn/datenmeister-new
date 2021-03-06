using System.Threading.Tasks;
using Autofac;
using DatenMeister.Integration;
using DatenMeister.NetCore.Modules.PluginLoader;
using DatenMeister.Runtime.Plugins;

namespace DatenMeister.NetCore
{
    public class GiveMeDotNetCore
    {
        /// <summary>
        /// Return the DatenMeisterScope asynchronisously as a task.
        /// </summary>
        /// <param name="settings">Settings to be used</param>
        /// <returns>The created task, retuning the DatenMeister. </returns>
        public static Task<IDatenMeisterScope> DatenMeisterAsync(IntegrationSettings? settings = null)
            => Task.Run(() => DatenMeister(settings));
        
        /// <summary>
        /// Returns a fully initialized DatenMeister for use.
        /// </summary>
        /// <param name="settings">Integration settings for the initialization of DatenMeister</param>
        /// <returns>The initialized DatenMeister that can be used</returns>
        public static IDatenMeisterScope DatenMeister(IntegrationSettings? settings = null)
        {
            settings ??= GetDefaultIntegrationSettings();

            if (settings.PluginLoader is DefaultPluginLoader || settings.PluginLoader == null)
            {
                settings.PluginLoader = new DotNetCorePluginLoader();
            }

            var kernel = new ContainerBuilder();
            var container = kernel.UseDatenMeister(settings);

            var scope = new DatenMeisterScope(container.BeginLifetimeScope());
            scope.ScopeStorage = scope.Resolve<IScopeStorage>();

            GiveMe.Scope = scope;
            scope.BeforeDisposing += (x, y) =>
            {
                scope.UnuseDatenMeister();
                GiveMe.ClearScope();
            };

            return GiveMe.Scope;
        }

        public static IntegrationSettings GetDefaultIntegrationSettings()
        {
            return new IntegrationSettings
            {
                EstablishDataEnvironment = true,
                DatabasePath = GiveMe.DefaultDatabasePath,
                PluginLoader = new DotNetCorePluginLoader()
            };
        }
    }
}