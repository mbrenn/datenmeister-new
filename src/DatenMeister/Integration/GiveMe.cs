using System;
using System.IO;
using System.Reflection;
using Autofac;

namespace DatenMeister.Integration
{
    /// <summary>
    /// Returns a complete DatenMeister instance
    /// </summary>
    public static class GiveMe
    {
        /// <summary>
        /// Returns a fully initialized DatenMeister for use. 
        /// </summary>
        /// <param name="settings">Integration settings for the initialization of DatenMeister</param>
        /// <returns>The initialized DatenMeister that can be used</returns>
        public static IDatenMeisterScope DatenMeister(IntegrationSettings settings = null)
        {
            if (settings == null)
            {
                settings = new IntegrationSettings
                {
                    EstablishDataEnvironment = true
                };
            }

            var kernel = new ContainerBuilder();
            var container = kernel.UseDatenMeister(settings);

            return new DatenMeisterScope(container.BeginLifetimeScope());
        }

        /// <summary>
        /// Gets the default database path
        /// </summary>
        public static string DatabasePath => 
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "datenmeister/data");
    }
}
