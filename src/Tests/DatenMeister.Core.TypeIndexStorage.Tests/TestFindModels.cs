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

        var inMemoryConfig = new ExtentLoaderConfigs.InMemoryLoaderConfig_Wrapper(InMemoryObject.TemporaryFactory)
        {
            name = "test",
            extentUri = "dm:///test",
            workspaceId = WorkspaceNames.WorkspaceData
        };

        var extentManager = new ExtentManager(dm.WorkspaceLogic, dm.ScopeStorage);
        var found = (await extentManager.LoadExtent(inMemoryConfig.GetWrappedElement())).Extent;
        Assert.That(found, Is.Not.Null);

        // Ok, we got the extent, now we query it whether it knows the right class model
        var foundAsExtent = found as MofUriExtent;
        Assert.That(foundAsExtent, Is.Not.Null);

        var inMemoryProvider = foundAsExtent!.Provider as InMemoryProvider;
        Assert.That(inMemoryProvider, Is.Not.Null);

        var classModel =
            inMemoryProvider!.FindClassModel(_CommonTypes.TheOne.OSIntegration.__CommandLineApplication.Uri);
        Assert.That(classModel, Is.Not.Null);
        Assert.That(classModel!.Name, Is.EqualTo("CommandLineApplication"));
    }

    [Test]
    public async Task TestFindByObject()
    {
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>()
                             ?? throw new InvalidOperationException("TypeIndexStore not found");
        typeIndexStore.WaitForAvailabilityOfIndexStore();
        
        var inMemoryConfig = new ExtentLoaderConfigs.InMemoryLoaderConfig_Wrapper(InMemoryObject.TemporaryFactory)
        {
            name = "test",
            extentUri = "dm:///test",
            workspaceId = WorkspaceNames.WorkspaceData
        };
        
        var extentManager = new ExtentManager(dm.WorkspaceLogic, dm.ScopeStorage);
        var found = (await extentManager.LoadExtent(inMemoryConfig.GetWrappedElement())).Extent;
        Assert.That(found, Is.Not.Null);
        
        var factory = new MofFactory(found);
        var element = new CommonTypes.OSIntegration.CommandLineApplication_Wrapper(factory)
        {
            name = "Test",
            applicationPath = "path"
        };
        
        found.elements().add(element.GetWrappedElement());
        
        // Now check that the InMemoryObject is working correctly
        var asInMemoryObject = (element.GetWrappedElement() as MofElement)?.ProviderObject as InMemoryObject;
        Assert.That(asInMemoryObject, Is.Not.Null);

        var classModel = asInMemoryObject!.GetClassModel();
        Assert.That(classModel!.Name, Is.EqualTo("CommandLineApplication"));
    }
}