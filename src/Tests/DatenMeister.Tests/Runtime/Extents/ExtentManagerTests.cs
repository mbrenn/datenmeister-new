using Autofac;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using NUnit.Framework;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Tests.Runtime.Extents;

[TestFixture]
public class ExtentManagerTests
{
    [Test]
    public async Task TestLoadAndUnloading()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var extentManager = dm.Resolve<ExtentManager>();

        var loaderConfig =
            InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__InMemoryLoaderConfig);
        loaderConfig.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
        loaderConfig.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId,
            WorkspaceNames.WorkspaceData);

        await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
        await extentManager.DeleteExtent(
            loaderConfig.getOrDefault<string>(_ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId),
            loaderConfig.getOrDefault<string>(_ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri));
        var extent = await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);

        Assert.That(extent, Is.Not.Null);
        Assert.That(extent.Extent, Is.Not.Null);

        await extentManager.RemoveExtent(extent.Extent!);
        await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
    }

    [Test]
    public async Task TestNonPersistentExtents()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var extentManager = dm.Resolve<ExtentManager>();
        
        var mofUriExtent = new MofUriExtent(new InMemoryProvider(), "dm:///test3", dm.ScopeStorage);
        extentManager.AddNonPersistentExtent(WorkspaceNames.WorkspaceManagement, mofUriExtent);
        
        // Check that extent is within workspace
        Assert.That(extentManager.WorkspaceLogic.FindExtent(WorkspaceNames.WorkspaceManagement, "dm:///test3"), Is.Not.Null);

        var loadedExtentInformation = extentManager.GetLoadedExtentInformation(mofUriExtent);
        Assert.That(loadedExtentInformation, Is.Not.Null);
        Assert.That(loadedExtentInformation!.IsExtentPersistent, Is.False);
        
        // Checks that the extent is available via the Management Extent for Workspaces
        var managementExtent =
            dm.WorkspaceLogic.FindExtent(WorkspaceNames.WorkspaceManagement, WorkspaceNames.UriExtentWorkspaces);
        Assert.That(managementExtent, Is.Not.Null);
        
        // Get workspace
        var workspace = managementExtent!.elements().OfType<IElement>().FirstOrDefault(
            x =>
                x.getOrDefault<string>(_Management._Workspace.id) == WorkspaceNames.WorkspaceManagement);
        Assert.That(workspace, Is.Not.Null);
        
        // Checks, that the extent is existing within the workspace
        var extent = workspace!
            .getOrDefault<IReflectiveSequence>(_Management._Workspace.extents).OfType<IElement>()
            .FirstOrDefault(x => x.getOrDefault<string>(_Management._Extent.uri) == "dm:///test3");
        Assert.That(extent, Is.Not.Null);
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
            InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__InMemoryLoaderConfig);
        loaderConfig.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
        loaderConfig.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId, "Test");
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
            InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__InMemoryLoaderConfig);
        loaderConfig.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
        loaderConfig.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId,
            WorkspaceNames.WorkspaceData);
        loaderConfig.set("Test", "test");
            
        await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);

        var result = extentManager.GetProviderLoaderAndConfiguration(
            WorkspaceNames.WorkspaceData, "dm:///test");
            
        Assert.That(result.providerLoader, Is.Not.Null);
        Assert.That(result.loadConfiguration, Is.Not.Null);
            
        Assert.That(result.providerLoader, Is.TypeOf(typeof(InMemoryProviderLoader))); ;
        Assert.That(result.loadConfiguration!.getOrDefault<string>("Test"), Is.EqualTo("test"));
    }

    [Test]
    public async Task GetExtentInformationWithoutSetWorkspace()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var extentManager = dm.Resolve<ExtentManager>();

        var loaderConfig =
            InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__InMemoryLoaderConfig);
        loaderConfig.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
            
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
        Assert.That(providerAndLoader.loadConfiguration?.metaclass?.equals(_ExtentLoaderConfigs.TheOne.__InMemoryLoaderConfig) == true);
    }

    [Test]
    public async Task TestThatDropExistingRemovesExistingExtent()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var extentManager = dm.Resolve<ExtentManager>();

        // Create first extent 
        var loaderConfig =
            InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__InMemoryLoaderConfig);
        loaderConfig.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
            
        var loadedInfo = await extentManager.LoadExtent(loaderConfig, ExtentCreationFlags.CreateOnly);
        Assert.That(loadedInfo.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));
            
        // Add one item
        var item  = new MofFactory(loadedInfo.Extent!).create(null);
        loadedInfo.Extent!.elements().add(item);
            
        Assert.That(loadedInfo.Extent.elements().Count(), Is.EqualTo(1));
            
        // Now reload with the expectation that the already loaded extent is empty
        var loaderConfig2 = InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__InMemoryLoaderConfig);
        loaderConfig2.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///test");
        loaderConfig2.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.dropExisting, true);
            
            
        var loadedInfo2 = await extentManager.LoadExtent(loaderConfig2, ExtentCreationFlags.CreateOnly);
        Assert.That(loadedInfo2.LoadingState, Is.EqualTo(ExtentLoadingState.Loaded));
        Assert.That(loadedInfo2.Extent, Is.Not.Null);         
        Assert.That(loadedInfo2.Extent!.elements().Count(), Is.EqualTo(0));
    }
}