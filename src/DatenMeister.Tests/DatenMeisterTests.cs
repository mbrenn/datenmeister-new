using System;
using System.IO;
using System.Reflection;
using Autofac;
using DatenMeister.Core.Plugins;
using DatenMeister.Integration;
using NUnit.Framework;

namespace DatenMeister.Tests
{
    [TestFixture]
    public class DatenMeisterTests
    {
        /// <summary>
        /// Gets the DatenMeister Scope for the testing
        /// </summary>
        /// <returns></returns>
        public static IDatenMeisterScope GetDatenMeisterScope(bool dropDatabase = true)
        {
            var path = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "testing/datenmeister/data");
            var integrationSettings = new IntegrationSettings()
            {
                DatabasePath = path,
                EstablishDataEnvironment = true,
                PerformSlimIntegration = false
            };

            if (dropDatabase)
            {
                GiveMe.DropDatenMeisterStorage(integrationSettings);

            }

            var datenMeister = GiveMe.DatenMeister(integrationSettings);
            return datenMeister;
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