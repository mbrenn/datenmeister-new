using Autofac;
using DatenMeister.Core.Plugins;
using DatenMeister.Integration;
using NUnit.Framework;

namespace DatenMeister.Tests
{
    [TestFixture]
    public class DatenMeisterTests
    {
        [Test]
        public void CheckFailureFreeLoadingOfDatenMeister()
        {
            var datenMeister = GiveMe.DatenMeister();
            var pluginManager = datenMeister.Resolve<PluginManager>();
            Assert.That(pluginManager.NoExceptionDuringLoading, Is.True);
        }
    }
}