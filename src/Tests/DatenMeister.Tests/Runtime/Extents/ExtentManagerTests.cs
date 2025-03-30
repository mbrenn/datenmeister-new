#nullable enable

using System.Linq;
using Autofac;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using NUnit.Framework;
using System.Threading.Tasks;
using DatenMeister.Core.EMOF.Implementation;

namespace DatenMeister.Tests.Runtime.Extents
{
    [TestFixture]
    public class ExtentManagerTests
    {
        [Test]
        public async Task TestLoadAndUnloading()
        {
            await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
            var extentManager = dm.Resolve<ExtentManager>();

            var loaderConfig =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__InMemoryLoaderConfig);
            loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
            loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId,
                WorkspaceNames.WorkspaceData);

            await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
            await extentManager.DeleteExtent(
                loaderConfig.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId),
                loaderConfig.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri));
            var extent = await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);

            Assert.That(extent, Is.Not.Null);
            Assert.That(extent.Extent, Is.Not.Null);

            await extentManager.RemoveExtent(extent.Extent!);
            await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
        }

        [Test]
        public async Task TestCorrectRemovalOfExtentsWhenWorkspaceIsDeleted()
        {
            await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
            dm.WorkspaceLogic.AddWorkspace(
                new Workspace("Test", "Test Extent"));

            var extentManager = dm.Resolve<ExtentManager>();

            // Performs the first loading of the extent
            var loaderConfig =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__InMemoryLoaderConfig);
            loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
            loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId, "Test");
            var loadedInfo = await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
            Assert.That(loadedInfo.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));

            // Removes the workspace
            dm.WorkspaceLogic.RemoveWorkspace("Test");

            // Load again, it should work
            loadedInfo = await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
            Assert.That(loadedInfo.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));
        }

        [Test]
        public async Task TestGetProviderAndConfiguration()
        {
            await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
            var extentManager = dm.Resolve<ExtentManager>();

            var loaderConfig =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__InMemoryLoaderConfig);
            loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
            loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId,
                WorkspaceNames.WorkspaceData);
            loaderConfig.set("Test", "test");
            
            await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);

            var result = extentManager.GetProviderLoaderAndConfiguration(
                WorkspaceNames.WorkspaceData, "dm:///test");
            
            Assert.That(result.providerLoader, Is.Not.Null);
            Assert.That(result.loadConfiguration, Is.Not.Null);
            
            Assert.That(result.providerLoader, Is.TypeOf(typeof(InMemoryProviderLoader)));
            Assert.That(result.loadConfiguration.getOrDefault<string>("Test"), Is.EqualTo("test"));
        }

        [Test]
        public async Task GetExtentInformationWithoutSetWorkspace()
        {
            await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
            var extentManager = dm.Resolve<ExtentManager>();

            var loaderConfig =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__InMemoryLoaderConfig);
            loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
            
            var loadedInfo = await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
            
            // First, check if loading was success
            Assert.That(loadedInfo.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));
            
            // Second, check that extent can be retrieved
            var extent = dm.WorkspaceLogic.FindExtent(WorkspaceNames.WorkspaceData, "dm:///test");
            Assert.That(extent, Is.Not.Null);
            
            // Third, check that we can retrieve the extent info
            var extentInfo = extentManager.GetExtentInformation(WorkspaceNames.WorkspaceData, "dm:///test");
            Assert.That(extentInfo, Is.Not.Null);
            
            // Fourth
            var providerAndLoader =
                extentManager.GetProviderLoaderAndConfiguration(WorkspaceNames.WorkspaceData, "dm:///test");
            Assert.That(providerAndLoader.providerLoader, Is.Not.Null);
            Assert.That(providerAndLoader.providerLoader, Is.TypeOf<InMemoryProviderLoader>());
            
            Assert.That(providerAndLoader.loadConfiguration, Is.Not.Null);
            Assert.That(providerAndLoader.loadConfiguration?.metaclass?.equals(_DatenMeister.TheOne.ExtentLoaderConfigs.__InMemoryLoaderConfig) == true);
        }

        [Test]
        public async Task TestThatDropExistingRemovesExistingExtent()
        {
            await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
            var extentManager = dm.Resolve<ExtentManager>();

            // Create first extent 
            var loaderConfig =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__InMemoryLoaderConfig);
            loaderConfig.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
            
            var loadedInfo = await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
            Assert.That(loadedInfo.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));
            
            // Add one item
            var item  = new MofFactory(loadedInfo.Extent!).create(null);
            loadedInfo.Extent!.elements().add(item);
            
            Assert.That(loadedInfo.Extent.elements().Count(), Is.EqualTo(1));
            
            // Now reload with the expectation that the already loaded extent is empty
            var loaderConfig2 = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__InMemoryLoaderConfig);
            loaderConfig2.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
            loaderConfig2.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.dropExisting, true);
            
            
            var loadedInfo2 = await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
            Assert.That(loadedInfo2.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));
            Assert.That(loadedInfo.Extent.elements().Count(), Is.EqualTo(0));
        }
    }
}