using Autofac;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.AutoEnumerate;
using DatenMeister.Core.EMOF.Implementation.Hooks;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DependencyInjection;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Forms;
using DatenMeister.Integration.DotNet;
using DatenMeister.Modules.ZipCodeExample;
using DatenMeister.Types;
using DatenMeister.Types.Plugin;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime.Extents;

[TestFixture]
public class ExtentTests
{
    [Test]
    public async Task TestPropertiesOfManagementProvider()
    {
        await using var builder = await DatenMeisterTests.GetDatenMeisterScope();

        await using var scope = builder.BeginLifetimeScope();
        var workspaceLogic = scope.Resolve<IWorkspaceLogic>();
        var workspaceExtent = workspaceLogic.FindExtent(WorkspaceNames.UriExtentWorkspaces);
        Assert.That(workspaceExtent, Is.Not.Null);
        var asData = workspaceExtent!.elements().Cast<IElement>()
            .First(x => x.get("id")?.ToString() == WorkspaceNames.WorkspaceData);
        var asManagement = workspaceExtent.elements().Cast<IElement>()
            .First(x => x.get("id")?.ToString() == WorkspaceNames.WorkspaceManagement);
        var asTypes = workspaceExtent.elements().Cast<IElement>()
            .First(x => x.get("id")?.ToString() == WorkspaceNames.WorkspaceTypes);
        var asMof = workspaceExtent.elements().Cast<IElement>()
            .First(x => x.get("id")?.ToString() == WorkspaceNames.WorkspaceMof);

        Assert.That(asData, Is.Not.Null);
        Assert.That(asManagement, Is.Not.Null);
        Assert.That(asTypes, Is.Not.Null);
        Assert.That(asMof, Is.Not.Null);

        // Get the extents
        var extents = (asMof.get("extents") as IEnumerable<object>)?.ToList();
        Assert.That(extents, Is.Not.Null);

        var mofExtent = extents!.Cast<IElement>()
            .First(x => x.get("uri")?.ToString() == WorkspaceNames.UriExtentMof);
        Assert.That(mofExtent, Is.Not.Null);
    }

    [Test]
    public async Task TestMetaDataInExtent()
    {
        var path = "./test.xmi";

        var loaderConfig =
            InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__XmiStorageLoaderConfig);
        loaderConfig.set(_ExtentLoaderConfigs._XmiStorageLoaderConfig.extentUri, "dm:///data");
        loaderConfig.set(_ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath, path);
        loaderConfig.set(_ExtentLoaderConfigs._XmiStorageLoaderConfig.workspaceId,
            WorkspaceNames.WorkspaceData);

        await using (var dm = await DatenMeisterTests.GetDatenMeisterScope())
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            var extentLoader = dm.Resolve<ExtentManager>();
            var loadedExtentInfo = await extentLoader.LoadExtent(loaderConfig, ExtentCreationFlags.LoadOrCreate);
            Assert.That(loadedExtentInfo.Extent, Is.Not.Null);

            var loadedExtent = loadedExtentInfo.Extent!;
            loadedExtent.set("test", "this is a test");
            loadedExtent.GetConfiguration().ExtentType = "Happy Extent";
            await extentLoader.StoreExtent(loadedExtent);

            await dm.UnuseDatenMeister();
        }

        await using (var dm = await DatenMeisterTests.GetDatenMeisterScope(dropDatabase: false))
        {
            var workspaceLogic = dm.Resolve<IWorkspaceLogic>();
            var foundExtent = workspaceLogic.FindExtent("dm:///data");
            Assert.That(foundExtent, Is.Not.Null);

            Assert.That(foundExtent!.get("test"), Is.EqualTo("this is a test"));
            Assert.That(foundExtent.GetConfiguration().ExtentType, Is.EqualTo("Happy Extent"));

            await dm.UnuseDatenMeister();
        }
    }

    [Test]
    public async Task TestDefaultExtentType()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var workspaceLogic = dm.Resolve<IWorkspaceLogic>();
        var zipCodeExample = dm.Resolve<ZipCodeExampleManager>();
        var typesWorkspace = workspaceLogic.GetTypesWorkspace();
        var zipCodeModel =
            typesWorkspace.FindObject("dm:///_internal/types/internal?fn=" +
                                      ZipCodeModel.PackagePath);

        var dataWorkspace = workspaceLogic.GetDataWorkspace();

        var zipExample = await zipCodeExample.AddZipCodeExample(dataWorkspace);
        var setDefaultTypes = zipExample.GetConfiguration().GetDefaultTypes().ToList();

        Assert.That(setDefaultTypes, Is.Not.Null);
        Assert.That(zipCodeModel, Is.Not.Null);

        Assert.That(setDefaultTypes!.FirstOrDefault(), Is.EqualTo(zipCodeModel));
    }

    [Test]
    public void TestFindAlternativeUriByFindExtent()
    {
        var scopeStorage = new ScopeStorage();
        ResolveHookContainer.AddDefaultHooks(scopeStorage);
        var workspaceLogic = new WorkspaceLogic(scopeStorage);
        var dataWorkspace = workspaceLogic.GetOrCreateWorkspace("Data");

        var extent = new MofUriExtent(new InMemoryProvider(), "dm:///test", scopeStorage);
        extent.AddAlternativeUri("dm:///otheruri");
        dataWorkspace.AddExtent(extent);

        var otherUri = workspaceLogic.FindExtent("dm:///otheruri");
        Assert.That(otherUri, Is.Not.Null);
        Assert.That(otherUri!.contextURI(), Is.EqualTo("dm:///test"));
    }

    [Test]
    public async Task TestExtentSettingsExtentType()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var extentSettings = dm.ScopeStorage.Get<ExtentSettings>();
        Assert.That(extentSettings.extentTypeSettings.Any(x => x.name == UmlPlugin.ExtentType), Is.True);
        Assert.That(extentSettings.extentTypeSettings.Any(x => x.name == FormMethods.FormExtentType), Is.True);
        Assert.That(extentSettings.extentTypeSettings.Any(x => x.name == ZipCodePlugin.ZipCodeExtentType), Is.True);
    }

    [Test]
    public async Task TestAutoEnumerateType()
    {
        var path = "./test.xmi";

        var loaderConfig =
            InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__XmiStorageLoaderConfig);
        loaderConfig.set(_ExtentLoaderConfigs._XmiStorageLoaderConfig.extentUri, "dm:///data");
        loaderConfig.set(_ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath, path);
        loaderConfig.set(_ExtentLoaderConfigs._XmiStorageLoaderConfig.workspaceId,
            WorkspaceNames.WorkspaceData);

        await using (var dm = await DatenMeisterTests.GetDatenMeisterScope())
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            var extentLoader = dm.Resolve<ExtentManager>();
            var loadedExtent = await extentLoader.LoadExtent(loaderConfig, ExtentCreationFlags.LoadOrCreate);

            Assert.That(loadedExtent.Extent, Is.Not.Null);
            loadedExtent.Extent!.GetConfiguration().AutoEnumerateType = AutoEnumerateType.Ordinal;
            await extentLoader.StoreExtent(loadedExtent.Extent!);

            await dm.UnuseDatenMeister();
        }

        await using (var dm = await DatenMeisterTests.GetDatenMeisterScope(dropDatabase: false))
        {
            var workspaceLogic = dm.Resolve<IWorkspaceLogic>();
            var foundExtent = workspaceLogic.FindExtent("dm:///data");
            Assert.That(foundExtent, Is.Not.Null);
            Assert.That(foundExtent!.GetConfiguration().AutoEnumerateType, Is.EqualTo(AutoEnumerateType.Ordinal));

            foundExtent!.GetConfiguration().AutoEnumerateType = AutoEnumerateType.Guid;
            Assert.That(foundExtent!.GetConfiguration().AutoEnumerateType, Is.EqualTo(AutoEnumerateType.Guid));

            await dm.UnuseDatenMeister();
        }
    }

    [Test]
    public async Task TestAddDefaultExtentType()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();

        var workspaceLogic = dm.Resolve<IWorkspaceLogic>();
        var zipCodeExample = dm.Resolve<ZipCodeExampleManager>();
        var typesWorkspace = workspaceLogic.GetTypesWorkspace();
        var zipCodeModel =
            typesWorkspace.FindObject("dm:///_internal/types/internal?fn=" +
                                      ZipCodeModel.PackagePath) as IElement;
        Assert.That(zipCodeModel, Is.Not.Null);

        var dataWorkspace = workspaceLogic.GetDataWorkspace();

        var zipExample = await zipCodeExample.AddZipCodeExample(dataWorkspace);

        // Per Default, one is included
        var setDefaultTypes = zipExample.GetConfiguration().GetDefaultTypes()?.ToList();
        Assert.That(setDefaultTypes, Is.Not.Null);
        Assert.That(setDefaultTypes!.Count, Is.EqualTo(1));
        Assert.That(setDefaultTypes.FirstOrDefault(), Is.EqualTo(zipCodeModel));

        // Checks, if adding another one does not work
        zipExample.GetConfiguration().AddDefaultTypes(new[] { zipCodeModel! });
        setDefaultTypes = zipExample.GetConfiguration().GetDefaultTypes().ToList();
        Assert.That(setDefaultTypes, Is.Not.Null);
        Assert.That(setDefaultTypes!.Count, Is.EqualTo(1));
        Assert.That(setDefaultTypes.FirstOrDefault(), Is.EqualTo(zipCodeModel));

        // Checks, if removing works
        zipExample.GetConfiguration().SetDefaultTypes(new IElement[] { });
        setDefaultTypes = zipExample.GetConfiguration().GetDefaultTypes()?.ToList();
        Assert.That(setDefaultTypes, Is.Not.Null);
        Assert.That(setDefaultTypes!.Count, Is.EqualTo(0));

        // Checks, if adding works now correctly
        zipExample.GetConfiguration().AddDefaultTypes(new[] { zipCodeModel! });
        setDefaultTypes = zipExample.GetConfiguration().GetDefaultTypes().ToList();
        Assert.That(setDefaultTypes, Is.Not.Null);
        Assert.That(setDefaultTypes!.Count, Is.EqualTo(1));
        Assert.That(setDefaultTypes.FirstOrDefault(), Is.EqualTo(zipCodeModel));
    }

    [Test]
    public void TestExtentTypes()
    {
        var extent = new MofUriExtent(new InMemoryProvider(), null);
        var configuration = new ExtentConfiguration(extent)
        {
            ExtentType = "abc"
        };

        var list = configuration.ExtentTypes.ToList();
        Assert.That(list, Is.Not.Null);
        Assert.That(list.Count, Is.EqualTo(1));
        Assert.That(list[0], Is.EqualTo("abc"));

        configuration.ExtentType = "abc def";
        list = configuration.ExtentTypes.ToList();
        Assert.That(list, Is.Not.Null);
        Assert.That(list.Count, Is.EqualTo(2));
        Assert.That(list[0], Is.EqualTo("abc"));
        Assert.That(list[1], Is.EqualTo("def"));

        configuration.ExtentTypes = new[] { "abc", "def" };
        Assert.That(configuration.ExtentType, Is.EqualTo("abc def"));
        list = configuration.ExtentTypes.ToList();
        Assert.That(list, Is.Not.Null);
        Assert.That(list.Count, Is.EqualTo(2));
        Assert.That(list[0], Is.EqualTo("abc"));
        Assert.That(list[1], Is.EqualTo("def"));
    }

    [Test]
    public static void TestAlternativeUris()
    {
        var mofExtent = new MofUriExtent(new InMemoryProvider(), "dm:///a", null);
        mofExtent.AlternativeUris.Add("dm:///test");
        mofExtent.AlternativeUris.Add("dm:///test2");

        Assert.That(mofExtent.AlternativeUris.Count, Is.EqualTo(2));
        Assert.That(mofExtent.AlternativeUris.Contains("dm:///test"), Is.True);
        Assert.That(mofExtent.AlternativeUris.Contains("dm:///test2"), Is.True);
    }

    [Test]
    public static void TestAlternativeUrisWithXmiProvider()
    {
        var xmiProvider = new XmiProvider();
        var mofExtent = new MofUriExtent(xmiProvider, "dm:///a", null);
        mofExtent.AlternativeUris.Add("dm:///test");
        mofExtent.AlternativeUris.Add("dm:///test2");

        Assert.That(mofExtent.AlternativeUris.Count, Is.EqualTo(2));
        Assert.That(mofExtent.AlternativeUris.Contains("dm:///test"), Is.True);
        Assert.That(mofExtent.AlternativeUris.Contains("dm:///test2"), Is.True);
    }


    [Test]
    public static void TestAutoSettingOfIds()
    {
        var mofExtent = new MofUriExtent(new InMemoryProvider(), "dm:///a", null);
        var mofFactory = new MofFactory(mofExtent);

        var element1 = mofFactory.create(null);
        mofExtent.elements().add(element1);
        var element2 = mofFactory.create(null);
        mofExtent.elements().add(element2);

        var n1 = ExtentHelper.SetAvailableId(mofExtent, element1, "name");
        var n2 = ExtentHelper.SetAvailableId(mofExtent, element2, "name");

        Assert.That(n1, Is.EqualTo("name"));
        Assert.That(n2, Is.EqualTo("name-1"));

        Assert.That((element1 as IHasId)?.Id, Is.EqualTo("name"));
        Assert.That((element2 as IHasId)?.Id, Is.Not.EqualTo("name"));
        Assert.That((element2 as IHasId)?.Id, Contains.Substring("name"));
    }

    [Test]
    public static async Task TestDefaultValue()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();

        var extent = CreateType(dm, out var type);

        var extentFactory = new MofFactory(extent);

        var createdType = extentFactory.create(type);
        Assert.That(createdType, Is.Not.Null);
        Assert.That(createdType.getOrDefault<int>("age"), Is.EqualTo(18));
    }

    [Test]
    public static async Task TestAutoEnumerateGuidProperty()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();

        var extent = CreateType(dm, out var type);

        var extentFactory = new MofFactory(extent);

        extent.GetConfiguration().AutoEnumerateType = AutoEnumerateType.Guid;
        var createdType = extentFactory.create(type);
        Assert.That(createdType, Is.Not.Null);
        Assert.That(createdType.getOrDefault<string>("id"), Is.Not.Null);

        var id = createdType.getOrDefault<string>("id");
        Assert.That(id.Length, Is.EqualTo(36));

        var createdType2 = extentFactory.create(type);
        Assert.That(createdType2, Is.Not.Null);
        Assert.That(createdType2.getOrDefault<string>("id"), Is.Not.Null);

        var id2 = createdType2.getOrDefault<string>("id");
        Assert.That(id2.Length, Is.EqualTo(36));
        Assert.That(id2, Is.Not.EqualTo(id));
        var setId = (createdType2 as IHasId)?.Id;
        Assert.That(setId, Is.EqualTo(id2));
    }

    [Test]
    public static async Task TestAutoEnumerateOrdinalProperty()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var extent = CreateType(dm, out var type);

        var extentFactory = new MofFactory(extent);

        extent.GetConfiguration().AutoEnumerateType = AutoEnumerateType.Ordinal;
        var createdType = extentFactory.create(type);
        Assert.That(createdType, Is.Not.Null);
        Assert.That(createdType.getOrDefault<string>("id"), Is.Not.Null);

        var id = createdType.getOrDefault<int>("id");
        Assert.That(id, Is.EqualTo(1));

        var createdType2 = extentFactory.create(type);
        Assert.That(createdType2, Is.Not.Null);
        Assert.That(createdType2.getOrDefault<int>("id"), Is.Not.Null);

        var id2 = createdType2.getOrDefault<int>("id");
        Assert.That(id2, Is.EqualTo(2));
        var setId = (createdType2 as IHasId)?.Id;
        Assert.That(setId, Is.EqualTo(id2.ToString()));

        extent.unset(AutoEnumerateHandler.AutoEnumerateTypeValue);


        var createdType3 = extentFactory.create(type);
        Assert.That(createdType3, Is.Not.Null);
        Assert.That(createdType3.getOrDefault<int>("id"), Is.Not.Null);

        var id3 = createdType3.getOrDefault<int>("id");
        Assert.That(id3, Is.EqualTo(1));

        extent.elements().add(createdType);
        extent.elements().add(createdType2);
        extent.elements().add(createdType3);

        extent.unset(AutoEnumerateHandler.AutoEnumerateTypeValue);

        var createdType4 = extentFactory.create(type);
        Assert.That(createdType4, Is.Not.Null);
        Assert.That(createdType4.getOrDefault<int>("id"), Is.Not.Null);


        var id4 = createdType4.getOrDefault<int>("id");
        Assert.That(id4, Is.EqualTo(3));
    }

    private static MofExtent CreateType(IDatenMeisterScope dm, out IElement type)
    {
        var localTypeSupport = dm.Resolve<LocalTypeSupport>();
        var userTypes = localTypeSupport.GetUserTypeExtent();
        var factory = new MofFactory(userTypes);

        var extent = new MofUriExtent(new InMemoryProvider(), null);

        type = factory.create(_UML.TheOne.StructuredClassifiers.__Class);
        var property1 = factory.create(_UML.TheOne.Classification.__Property);
        property1.set(_UML._Classification._Property.isID, true);
        property1.set(_UML._CommonStructure._NamedElement.name, "id");

        var property2 = factory.create(_UML.TheOne.Classification.__Property);
        property2.set(_UML._CommonStructure._NamedElement.name, "name");

        var property3 = factory.create(_UML.TheOne.Classification.__Property);
        property3.set(_UML._CommonStructure._NamedElement.name, "age");
        property3.set(_UML._Classification._Property.defaultValue, 18);

        type.set(_UML._StructuredClassifiers._StructuredClassifier.ownedAttribute,
            new[] { property1, property2, property3 });
        type.set(_UML._CommonStructure._NamedElement.name, "your");

        userTypes.elements().add(type);
        return extent;
    }

    [Test]
    public void TestQueryItemById()
    {
        var uriExtent = CreateLittleExtent();
        var package1 = uriExtent.element("dm:///test#p1");
        Assert.That(package1, Is.Not.Null);
        Assert.That(package1.getOrDefault<string>(_UML._CommonStructure._NamedElement.name),
            Is.EqualTo("package1"));


        var element1 = uriExtent.element("dm:///test#e1");
        Assert.That(element1, Is.Not.Null);
        Assert.That(element1.getOrDefault<string>(_UML._CommonStructure._NamedElement.name),
            Is.EqualTo("element1"));
    }

    [Test]
    public void TestCreatingGettingAndDeletion()
    {
        var provider = new InMemoryProvider();
        var uriExtent = new MofUriExtent(provider, "dm:///unittest", null);

        var factory = new MofFactory(uriExtent);
        var item1 = factory.create(null);
        var item1Id = (item1 as IHasId)?.Id!;

        uriExtent.elements().add(item1);

        var found = uriExtent.Resolve(item1Id, ResolveType.Default) as MofElement;
        Assert.That(found, Is.Not.Null);
        Assert.That((found as IHasId)?.Id!, Is.EqualTo(item1Id));
            
        // Deletes all elements
        uriExtent.elements().RemoveAll();
            
        // Checks that the element is not somewhere hidden in a cache
        var found2 = uriExtent.Resolve(item1Id,ResolveType.Default) as MofElement;
        Assert.That(found2, Is.Null);
    }

    [Test]
    public void TestCreatingGettingAndDeletionWithXmi()
    {
        var provider = new XmiProvider();
        var uriExtent = new MofUriExtent(provider, "dm:///unittest", null);

        var factory = new MofFactory(uriExtent);
        var item1 = factory.create(null);

        uriExtent.elements().add(item1);
        var item1Id = (item1 as IHasId)?.Id!;

        var found = uriExtent.Resolve(item1Id, ResolveType.Default) as MofElement;
        Assert.That(found, Is.Not.Null);
        Assert.That((found as IHasId)?.Id!, Is.EqualTo(item1Id));
            
        // Deletes all elements
        uriExtent.elements().RemoveAll();
            
        // Checks that the element is not somewhere hidden in a cache
        var found2 = uriExtent.Resolve(item1Id,ResolveType.Default) as MofElement;
        Assert.That(found2, Is.Null);
    }

    /// <summary>
    /// Creates a little uriextent for testing
    /// </summary>
    /// <returns>Uri extent to be tested</returns>
    public IUriExtent CreateLittleExtent()
    {
        var uriExtent = new MofUriExtent(new InMemoryProvider(), "dm:///test", null);
        var factory = new MofFactory(uriExtent);

        var package1 = factory.create(null);
        var package2 = factory.create(null);
        var element1 = factory.create(null);

        (package1 as ICanSetId)!.Id = "p1";
        (package2 as ICanSetId)!.Id = "p2";
        (element1 as ICanSetId)!.Id = "e1";

        package1.set(_UML._CommonStructure._NamedElement.name, "package1");
        package2.set(_UML._CommonStructure._NamedElement.name, "package2");
        element1.set(_UML._CommonStructure._NamedElement.name, "element1");

        package1.set(_UML._Packages._Package.packagedElement, new[] { element1 });

        uriExtent.elements().add(package1);
        uriExtent.elements().add(package2);
        uriExtent.elements().add(element1);

        return uriExtent;
    }
}