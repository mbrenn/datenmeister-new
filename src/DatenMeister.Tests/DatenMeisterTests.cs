using System;
using System.IO;
using System.Reflection;
using Autofac;
using BurnSystems.Logging;
using BurnSystems.Logging.Provider;
using DatenMeister.Core.Plugins;
using DatenMeister.Integration;
using NUnit.Framework;

namespace DatenMeister.Tests
{
    [TestFixture]
    public class DatenMeisterTests
    {
        public static string GetPathForTemporaryStorage(string fileName)
        {
            var path = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "testing/datenmeister/data"); 
            if ( !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return Path.Combine(path, fileName);
        }

        /// <summary>
        /// Gets the DatenMeister Scope for the testing
        /// </summary>
        /// <returns></returns>
        public static IDatenMeisterScope GetDatenMeisterScope(bool dropDatabase = true)
        {
            TheLog.ClearProviders();
            TheLog.AddProvider(new ConsoleProvider());
            TheLog.AddProvider(new DebugProvider());

            var path = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "testing/datenmeister/data");
            var integrationSettings = new IntegrationSettings
            {
                DatabasePath = path,
                EstablishDataEnvironment = true,
                PerformSlimIntegration = false,
                AllowNoFailOfLoading = false
            };

            if (dropDatabase)
            {
                GiveMe.DropDatenMeisterStorage(integrationSettings);
            }

            return GiveMe.DatenMeister(integrationSettings);
        }

        [Test]
        public void CheckFailureFreeLoadingOfDatenMeister()
        {
            var datenMeister = GetDatenMeisterScope();
            var pluginManager = datenMeister.Resolve<PluginManager>();
            Assert.That(pluginManager.NoExceptionDuringLoading, Is.True);
        }
    }
}