#nullable enable

using Autofac;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
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
            var extentManager = dm.Resolve<IExtentManager>();

            var loaderConfig = new InMemoryLoaderConfig
            {
                extentUri = "dm:///test",
                workspaceId = WorkspaceNames.NameData
            };
                    
            extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
            extentManager.DeleteExtent(loaderConfig.workspaceId, loaderConfig.extentUri);
            var extent = extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
            extentManager.DeleteExtent(extent);
            extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
        }
    }
}