﻿using System;
using System.Diagnostics;
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
        /// Gets or sets the scope for the DatenMeister
        /// </summary>
        public static IDatenMeisterScope Scope { get; set; }

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
                    EstablishDataEnvironment = true,
                    DatabasePath = DefaultDatabasePath
                };
            }

            var kernel = new ContainerBuilder();
            var container = kernel.UseDatenMeister(settings);

            return new DatenMeisterScope(container.BeginLifetimeScope());
        }

        /// <summary>
        /// Gets the default database path
        /// </summary>
        public static string DefaultDatabasePath => 
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
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
