#nullable enable

using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using DatenMeister.Core;
using DatenMeister.DependencyInjection;
using DatenMeister.Integration.DotNet.PluginLoader;
using DatenMeister.Plugins;

#if !NET462
#endif

namespace DatenMeister.Integration.DotNet
{
    /// <summary>
    /// Returns a complete DatenMeister instance
    /// </summary>
    public static class GiveMe
    {
        private static IDatenMeisterScope? _scope;

        /// <summary>
        /// Gets or sets the scope for the DatenMeister
        /// </summary>
        public static IDatenMeisterScope Scope
        {
            get => _scope ?? throw new InvalidOperationException("Scope is null");
            set => _scope = value;
        }

        /// <summary>
        /// Gets the scope, even if it is null
        /// </summary>
        /// <returns>The created scope</returns>
        public static IDatenMeisterScope? TryGetScope() => _scope;

        /// <summary>
        /// Clears the scope
        /// </summary>
        public static void ClearScope()
        {
            _scope = null;
        }

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
        public static IDatenMeisterScope DatenMeister(IntegrationSettings? settings = null,
            PluginLoaderSettings? pluginLoaderSettings = null)
        {
            settings ??= GetDefaultIntegrationSettings();
            pluginLoaderSettings ??= GetDefaultPluginLoaderSettings();

            if (pluginLoaderSettings.PluginLoader is DefaultPluginLoader)
            {
#if NET462
                pluginLoaderSettings.PluginLoader = new DefaultPluginLoader();
#else
                pluginLoaderSettings.PluginLoader = new DotNetCorePluginLoader();
#endif
            }

            var kernel = new ContainerBuilder();
            var container = kernel.UseDatenMeister(settings);

            var scope = new DatenMeisterScope(container.BeginLifetimeScope());
            scope.ScopeStorage = scope.Resolve<IScopeStorage>();

            Scope = scope;
            scope.BeforeDisposing += (x, y) =>
            {
                scope.UnuseDatenMeister();
                ClearScope();
            };

            return Scope;
        }

        public static IntegrationSettings GetDefaultIntegrationSettings()
        {
            return new IntegrationSettings
            {
                EstablishDataEnvironment = true,
                DatabasePath = IntegrationSettings.DefaultDatabasePath
            };
        }

        public static PluginLoaderSettings GetDefaultPluginLoaderSettings()
        {
            return new PluginLoaderSettings
            {
#if NET462
                PluginLoader = new DefaultPluginLoader()
#else
                PluginLoader = new DotNetCorePluginLoader()
#endif
            };
        }

        /// <summary>
        /// Drops the datenmeister settings
        /// </summary>
        /// <param name="settings">Settings to be deleted</param>
        public static void DropDatenMeisterStorage(IntegrationSettings settings)
        {
            if (!Directory.Exists(settings.DatabasePath))
            {
                // Directory does not exist. So remove it
                return;
            }

            var path = settings.DatabasePath;
            foreach (var files in Directory.EnumerateFiles(path))
            {
                File.Delete(files);
            }
        }
    }
}
