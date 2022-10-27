#nullable enable

using Autofac;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
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
            loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId,
                WorkspaceNames.WorkspaceData);


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

        [Test]
        public void TestCorrectRemovalOfExtentsWhenWorkspaceIsDeleted()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();
            dm.WorkspaceLogic.AddWorkspace(
                new Workspace("Test", "Test Extent"));

            var extentManager = dm.Resolve<ExtentManager>();

            // Performs the first loading of the extent
            var loaderConfig =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__InMemoryLoaderConfig);
            loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
            loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId, "Test");
            var loadedInfo = extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
            Assert.That(loadedInfo.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));

            // Removes the workspace
            dm.WorkspaceLogic.RemoveWorkspace("Test");

            // Load again, it should work
            loadedInfo = extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
            Assert.That(loadedInfo.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));
        }


        [Test]
        public void TestGetProviderAndConfiguration()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();
            var extentManager = dm.Resolve<ExtentManager>();

            var loaderConfig =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__InMemoryLoaderConfig);
            loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
            loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId,
                WorkspaceNames.WorkspaceData);
            loaderConfig.set("Test", "test");
            
            var loadedInfo = extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);

            var result = extentManager.GetProviderLoaderAndConfiguration(
                WorkspaceNames.WorkspaceData, "dm:///test");
            
            Assert.That(result.providerLoader, Is.Not.Null);
            Assert.That(result.loadConfiguration, Is.Not.Null);
            
            Assert.That(result.providerLoader, Is.TypeOf(typeof(InMemoryProviderLoader)));
            Assert.That(result.loadConfiguration.getOrDefault<string>("Test"), Is.EqualTo("test"));
        }
    }
}