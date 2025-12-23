using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.TypeIndexAssembly;
using DatenMeister.Core.TypeIndexAssembly.Model;
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
        var typeIndexLogic = new TypeIndexLogic(dm.WorkspaceLogic);
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
        
        var foundClassModel = foundAsExtent!.FindModel(_CommonTypes.TheOne.OSIntegration.__CommandLineApplication.Uri);
        Assert.That(foundClassModel, Is.Not.Null);
    }

    [Test] public async Task TestFindByModel()
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
        
        var factory = new MofFactory(found!);
        var element = new CommonTypes.OSIntegration.CommandLineApplication_Wrapper(factory)
        {
            name = "Test",
            applicationPath = "path"
        };
        
        found!.elements().add(element.GetWrappedElement());
        
        // Now check that the InMemoryObject is working correctly
        var asInMemoryObject = (element.GetWrappedElement() as MofElement) as MofObject;
        Assert.That(asInMemoryObject, Is.Not.Null);

        var classModel = asInMemoryObject!.GetClassModel();
        Assert.That(classModel, Is.Not.Null);
        Assert.That(classModel!.Name, Is.EqualTo("CommandLineApplication"));
    }

    [Test]
    public async Task TestFindInProviderModel()
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
    public async Task TestFindByProviderObject()
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
        
        var factory = new MofFactory(found!);
        var element = new CommonTypes.OSIntegration.CommandLineApplication_Wrapper(factory)
        {
            name = "Test",
            applicationPath = "path"
        };
        
        found!.elements().add(element.GetWrappedElement());
        
        // Now check that the InMemoryObject is working correctly
        var asInMemoryObject = (element.GetWrappedElement() as MofElement)?.ProviderObject as InMemoryObject;
        Assert.That(asInMemoryObject, Is.Not.Null);

        var classModel = asInMemoryObject!.GetClassModel();
        Assert.That(classModel, Is.Not.Null);
        Assert.That(classModel!.Name, Is.EqualTo("CommandLineApplication"));
    }
    
    [Test]
    public async Task FindByMetaClass()
    {
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>()
                             ?? throw new InvalidOperationException("TypeIndexStore not found");
        typeIndexStore.WaitForAvailabilityOfIndexStore();
        
        var typeIndexLogic = new TypeIndexLogic(dm.WorkspaceLogic);
        var foundClassModel = typeIndexLogic.FindClassModelByMetaClass(
            _CommonTypes.TheOne.OSIntegration.__CommandLineApplication);
        
        Assert.That(foundClassModel,Is.Not.Null);
        Assert.That(foundClassModel!.Name, Is.EqualTo("CommandLineApplication"));
    }

    [Test]
    public async Task TestMetaAttributeIsSet()
    {
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>()
                             ?? throw new InvalidOperationException("TypeIndexStore not found");
        typeIndexStore.WaitForAvailabilityOfIndexStore();

        var typeIndexLogic = new TypeIndexLogic(dm.WorkspaceLogic);
        var foundClassModel = typeIndexLogic.FindClassModelByMetaClass(
            _CommonTypes.TheOne.OSIntegration.__CommandLineApplication);

        Assert.That(foundClassModel, Is.Not.Null);
        Assert.That(foundClassModel!.Attributes.Count, Is.GreaterThan(0));
        
        foreach (var attribute in foundClassModel.Attributes)
        {
            Assert.That(attribute.MetaAttribute, Is.Not.Null, $"MetaAttribute for {attribute.Name} should not be null");
        }
    }

    [Test]
    public void TestAttributeModelClone()
    {
        var model = new AttributeModel
        {
            Id = "testId",
            Name = "testName",
            Url = "testUrl",
            IsComposite = true,
            TypeUrl = "testTypeUrl",
            DefaultValue = "testDefaultValue",
            IsMultiple = true,
            IsInherited = true
        };

        var clone = model.Clone();

        Assert.That(clone.Id, Is.EqualTo(model.Id));
        Assert.That(clone.Name, Is.EqualTo(model.Name));
        Assert.That(clone.Url, Is.EqualTo(model.Url));
        Assert.That(clone.IsComposite, Is.EqualTo(model.IsComposite));
        Assert.That(clone.TypeUrl, Is.EqualTo(model.TypeUrl));
        Assert.That(clone.DefaultValue, Is.EqualTo(model.DefaultValue));
        Assert.That(clone.IsMultiple, Is.EqualTo(model.IsMultiple));
        Assert.That(clone.IsInherited, Is.EqualTo(model.IsInherited));
    }

    [Test]
    public void TestClassModelFindAttribute()
    {
        var classModel = new ClassModel();
        classModel.Attributes.Add(new AttributeModel { Name = "Attr1" });
        classModel.Attributes.Add(new AttributeModel { Name = "Attr2" });

        // Before indexing
        Assert.That(classModel.FindAttribute("Attr1"), Is.Not.Null);

        // Create index
        classModel.CreateIndex();

        Assert.That(classModel.FindAttribute("Attr1"), Is.Not.Null);
        Assert.That(classModel.FindAttribute("Attr1")?.Name, Is.EqualTo("Attr1"));
        Assert.That(classModel.FindAttribute("Attr2"), Is.Not.Null);
        Assert.That(classModel.FindAttribute("Attr3"), Is.Null);
    }

    [Test]
    public void TestTypeIndexDataFindClassModelByUri()
    {
        var typeIndexData = new TypeIndexData();
        var workspaceModel = new WorkspaceModel { WorkspaceId = "TestWorkspace" };
        var classModel = new ClassModel { Uri = "dm:///testclass", Name = "TestClass" };
        workspaceModel.ClassModels.Add(classModel);
        typeIndexData.Workspaces.Add(workspaceModel);

        // Before indexing
        var foundBefore = typeIndexData.FindClassModelByUri("dm:///testclass");
        Assert.That(foundBefore, Is.EqualTo(classModel));

        // After indexing
        typeIndexData.CreateIndex();
        var foundAfter = typeIndexData.FindClassModelByUri("dm:///testclass");
        Assert.That(foundAfter, Is.EqualTo(classModel));

        // Non-existent
        Assert.That(typeIndexData.FindClassModelByUri("dm:///nonexistent"), Is.Null);
    }
}