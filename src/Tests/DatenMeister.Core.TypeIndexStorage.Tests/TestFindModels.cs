using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.TypeIndexAssembly;
using DatenMeister.Extent.Manager.ExtentStorage;
using NUnit.Framework;

namespace DatenMeister.Core.TypeIndexStorage.Tests;

[TestFixture]
public class TestFindModels
{
    [Test]
    public async Task TestFindInLogicItself()
    {
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>()
                             ?? throw new InvalidOperationException("TypeIndexStore not found");
        var typeIndexLogic = new TypeIndexLogic(dm.WorkspaceLogic, dm.ScopeStorage);
        typeIndexStore.WaitForAvailabilityOfIndexStore();
        
        // First, perform the test in the workspacelogic itself
        var classModel = typeIndexLogic.FindClassModelByUrlWithinWorkspace(
            WorkspaceNames.WorkspaceTypes,
            _CommonTypes.TheOne.OSIntegration.__CommandLineApplication.Uri);
        
        Assert.That(classModel, Is.Not.Null);
        
        // Secondly, perform the test within the metaworkspaces
        classModel = typeIndexLogic.FindClassModelByUrlWithinMetaWorkspaces(
            WorkspaceNames.WorkspaceData,
            _CommonTypes.TheOne.OSIntegration.__CommandLineApplication.Uri);
        
        Assert.That(classModel, Is.Not.Null);
    }

    [Test]
    public async Task TestFindInExtent()
    {
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>()
                             ?? throw new InvalidOperationException("TypeIndexStore not found");
        typeIndexStore.WaitForAvailabilityOfIndexStore();

        var inMemoryConfig = ExtentLoaderConfigs.InMemoryLoaderConfig_Wrapper.Create(
            InMemoryObject.TemporaryFactory);
        inMemoryConfig.name = "test";
        inMemoryConfig.extentUri = "dm:///test";
        inMemoryConfig.workspaceId = WorkspaceNames.WorkspaceData;
        
        var extentManager = new ExtentManager(dm.WorkspaceLogic, dm.ScopeStorage);
        var found =( await extentManager.LoadExtent(inMemoryConfig.GetWrappedElement())).Extent;
        Assert.That(found, Is.Not.Null);
        
        // Ok, we got the extent, now we query it whether it knows the right class model
        var foundAsExtent = found as MofUriExtent;
        Assert.That(foundAsExtent, Is.Not.Null);

        var inMemoryProvider = foundAsExtent!.Provider as InMemoryProvider;
        Assert.That(inMemoryProvider, Is.Not.Null);

        var classModel = inMemoryProvider!.FindClassModel(_CommonTypes.TheOne.OSIntegration.__CommandLineApplication.Uri);
        Assert.That(classModel, Is.Not.Null);
        Assert.That(classModel!.Name, Is.EqualTo("CommandLineApplication"));
    }
    
    [Test]
    public async Task TestFindByInMemoryObjectProvider()
    {
        
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>()
                             ?? throw new InvalidOperationException("TypeIndexStore not found");
        typeIndexStore.WaitForAvailabilityOfIndexStore();
        
        var typesWorkspace = typeIndexStore.GetCurrentIndexStore().FindWorkspace(WorkspaceNames.WorkspaceTypes);
    }
    
    [Test]
    public async Task TestFindByObject()
    {
        
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>()
                             ?? throw new InvalidOperationException("TypeIndexStore not found");
        typeIndexStore.WaitForAvailabilityOfIndexStore();
        
        var typesWorkspace = typeIndexStore.GetCurrentIndexStore().FindWorkspace(WorkspaceNames.WorkspaceTypes);
    }
    
    
    
}