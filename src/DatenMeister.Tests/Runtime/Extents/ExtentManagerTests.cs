#nullable enable

using Autofac;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime.Extents
{
    [TestFixture]
    public class ExtentManagerTests
    {
        [Test]
        public void TestLoadAndUnloading()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();
            var extentManager = dm.Resolve<ExtentManager>();

            var loaderConfig = new InMemoryLoaderConfig("dm:///test")
            {
                workspaceId = WorkspaceNames.WorkspaceData
            };
                    
            extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
            extentManager.DeleteExtent(loaderConfig.workspaceId, loaderConfig.extentUri);
            var extent = extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);

            Assert.That(extent, Is.Not.Null);
            Assert.That(extent.Extent, Is.Not.Null);

            extentManager.RemoveExtent(extent.Extent!);
            extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
        }
    }
}