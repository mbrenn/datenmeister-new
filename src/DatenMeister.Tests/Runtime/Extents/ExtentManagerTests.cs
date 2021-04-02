#nullable enable

using Autofac;
using DatenMeister.Models;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
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
            var extentManager = dm.Resolve<ExtentManager>();
            
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