#nullable enable

using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;

namespace DatenMeister.Integration
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
        /// Return the DatenMeisterScope asynchronously as a task.
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
            settings ??= new IntegrationSettings
            {
                EstablishDataEnvironment = true,
                DatabasePath = DefaultDatabasePath,
                IsLockingActivated = true
            };

            var kernel = new ContainerBuilder();
            var container = kernel.UseDatenMeister(settings);

            var scope = new DatenMeisterScope(container.BeginLifetimeScope());
            scope.ScopeStorage = scope.Resolve<ScopeStorage>();

            Scope = scope;

            Scope.BeforeDisposing += (x, y) =>
            {
                Scope.UnuseDatenMeister();
                _scope = null; // Set to null to avoid wrong use of scope
            };

            return Scope;
        }

        /// <summary>
        /// Gets the default database path
        /// </summary>
        public static string DefaultDatabasePath =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "datenmeister/data");

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
