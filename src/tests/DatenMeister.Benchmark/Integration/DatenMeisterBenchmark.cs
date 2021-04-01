using System;
using System.IO;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BurnSystems.Logging;
using BurnSystems.Logging.Provider;
using DatenMeister.Integration;
using DatenMeister.Integration.DotNet;
using DatenMeister.Runtime.Plugins;

namespace DatenMeister.Benchmark.Integration
{
    public class DatenMeisterBenchmark
    {
        /// <summary>
        /// Gets the DatenMeister Scope for the testing
        /// </summary>
        /// <returns></returns>
        public static IDatenMeisterScope GetDatenMeisterScope(bool dropDatabase = true,
            IntegrationSettings? integrationSettings = null)
        {
            TheLog.ClearProviders();
            TheLog.AddProvider(new FileProvider("d:\\dm_log.txt"));

            integrationSettings ??= GetIntegrationSettings(dropDatabase);

            if (dropDatabase)
            {
                GiveMe.DropDatenMeisterStorage(integrationSettings);
            }

            return GiveMe.DatenMeister(integrationSettings);
        }

        /// <summary>
        /// Gets the integration settings
        /// </summary>
        /// <param name="dropDatabase">true, if the database shall be dropped</param>
        /// <returns>The created integration settings</returns>
        public static IntegrationSettings GetIntegrationSettings(bool dropDatabase = true)
        {
            var path = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException("Path is null"),
                "testing/datenmeister/data");
            var integrationSettings = new IntegrationSettings
            {
                DatabasePath = path,
                EstablishDataEnvironment = true,
                PerformSlimIntegration = false,
                AllowNoFailOfLoading = false,
                InitializeDefaultExtents = dropDatabase,
                PluginLoader = new DefaultPluginLoader()
            };

            return integrationSettings;
        }

        [Benchmark]
        public void LoadDatenMeister()
        {
            using var datenMeister = GetDatenMeisterScope();
        }
    }
}