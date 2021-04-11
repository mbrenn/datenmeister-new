#nullable enable

using Autofac;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.ExtentManager.ExtentStorage;
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
            var extentManager = dm.Resolve<ExtentManager.ExtentStorage.ExtentManager>();
            
            var loaderConfig =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__InMemoryLoaderConfig);
            loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
            loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId, WorkspaceNames.WorkspaceData);

                    
            extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
            extentManager.DeleteExtent(
                loaderConfig.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId), 
                loaderConfig.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri));
            var extent = extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);

            Assert.That(extent, Is.Not.Null);
            Assert.That(extent.Extent, Is.Not.Null);

            extentManager.RemoveExtent(extent.Extent!);
            extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
        }
    }
}