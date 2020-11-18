using System;
using System.IO;
using System.Reflection;
using BurnSystems.Logging;
using BurnSystems.Logging.Provider;
using DatenMeister.Integration;
#if !NET462
using DatenMeister.NetCore.Modules.PluginLoader;
#endif
using DatenMeister.Runtime.Plugins;
using NUnit.Framework;

namespace DatenMeister.Tests
{
    [TestFixture]
    public class DatenMeisterTests
    {
        public static string GetPathForTemporaryStorage(string fileName)
        {
            var path = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location!)!,
                "testing/datenmeister/data");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return Path.Combine(path, fileName);
        }

        /// <summary>
        /// Gets the DatenMeister Scope for the testing
        /// </summary>
        /// <returns></returns>
        public static IDatenMeisterScope GetDatenMeisterScope(
            bool dropDatabase = true,
            IntegrationSettings integrationSettings = null)
        {
            TheLog.ClearProviders();
            TheLog.AddProvider(new ConsoleProvider());
            TheLog.AddProvider(new DebugProvider());

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
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException("Get Directory Name failed"),
                "testing/datenmeister/data");
            var integrationSettings = new IntegrationSettings
            {
                DatabasePath = path,
                EstablishDataEnvironment = true,
                PerformSlimIntegration = false,
                AllowNoFailOfLoading = false,
                InitializeDefaultExtents = dropDatabase,
#if NET462
                PluginLoader = new DefaultPluginLoader()
#else
                PluginLoader = new DotNetCorePluginLoader()
#endif
            };

            return integrationSettings;
        }

        [Test]
        public void CheckFailureFreeLoadingOfDatenMeister()
        {
            using var datenMeister = GetDatenMeisterScope();
            var pluginManager = datenMeister.ScopeStorage.Get<PluginManager>();
            Assert.That(pluginManager.NoExceptionDuringLoading, Is.True);
        }

        [Test]
        public void TestSlimLoading()
        {
            var integrationSettings = GetIntegrationSettings();
            integrationSettings.PerformSlimIntegration = true;

            using var datenMeister = GetDatenMeisterScope(integrationSettings: integrationSettings);
            var pluginManager = datenMeister.ScopeStorage.Get<PluginManager>();
            Assert.That(pluginManager.NoExceptionDuringLoading, Is.True);
        }
    }
}