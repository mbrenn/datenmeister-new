#nullable enable

using Autofac;
using DatenMeister.Core;
using DatenMeister.DependencyInjection;
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
        /// Returns a fully initialized DatenMeister for use.
        /// </summary>
        /// <param name="settings">Integration settings for the initialization of DatenMeister</param>
        /// <param name="pluginLoaderSettings">The default plugin loader settings</param>
        /// <returns>The initialized DatenMeister that can be used</returns>
        public static async Task <IDatenMeisterScope> DatenMeister(IntegrationSettings? settings = null,
            PluginLoaderSettings? pluginLoaderSettings = null)
        {
            settings ??= GetDefaultIntegrationSettings();
            pluginLoaderSettings ??= GetDefaultPluginLoaderSettings();

            var kernel = new ContainerBuilder();
            var container = await kernel.UseDatenMeister(settings, pluginLoaderSettings);

            var scope = new DatenMeisterScope(container.BeginLifetimeScope());
            scope.ScopeStorage = scope.Resolve<IScopeStorage>();

            Scope = scope;
            scope.BeforeDisposing += async (x, y) =>
            {
                await scope.UnuseDatenMeister();
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
                PluginLoader = new DefaultPluginLoader()
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
                // Directory does not exist. So skip it
                return;
            }

            var path = settings.DatabasePath;
            foreach (var file in Directory.EnumerateFiles(path))
            {
                File.Delete(file);
            }

            // Check, that really no file is existing, otherwise throw an exception
            if (Directory.EnumerateFiles(path).Any())
            {
                throw new InvalidOperationException("Could not delete all files");
            }
        }
    }
}