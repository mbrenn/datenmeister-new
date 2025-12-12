using System.Web;
using System.Xml.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.Hooks;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.TypeIndexAssembly;
using DatenMeister.DataView;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Plugins;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime;

[TestFixture]
public class ResolverTests
{
    [Test]
    public void TestExtentUrlNavigator()
    {
        var extent = GetTestExtent();
        var uriExtentNavigator = new ExtentUrlNavigator(extent, extent.ScopeStorage);
        var firstChild = uriExtentNavigator.element(TestUri + "#child1") as MofElement;
        var firstChildCached = uriExtentNavigator.element(TestUri + "#child1") as MofElement;

        Assert.That(firstChild, Is.Not.Null);
        Assert.That(firstChildCached, Is.Not.Null);
        Assert.That(firstChild!.getOrDefault<string>("name"), Is.EqualTo("child1"));
        Assert.That(firstChildCached!.getOrDefault<string>("name"), Is.EqualTo("child1"));
    }

    [Test]
    public void TestById()
    {
        var extent = GetTestExtent();
        var firstChild = extent.GetUriResolver().Resolve(TestUri + "#child1", ResolveType.Default)
            as MofElement;
        var firstChildCached = extent.GetUriResolver().Resolve(TestUri + "#child1", ResolveType.Default)
            as MofElement;
        var noChild = extent.GetUriResolver().Resolve(TestUri + "#none", ResolveType.Default)
            as MofElement;
        var child2Child1 = extent.GetUriResolver().Resolve(TestUri + "#child2child1", ResolveType.Default)
            as MofElement;
        var item1 = extent.GetUriResolver().Resolve(TestUri + "#item1", ResolveType.Default)
            as MofElement;
        var item12 = extent.GetUriResolver().Resolve("#item1", ResolveType.Default)
            as MofElement;

        Assert.That(firstChild, Is.Not.Null);
        Assert.That(firstChildCached, Is.Not.Null);
        Assert.That(item1, Is.Not.Null);
        Assert.That(item12, Is.Not.Null);
        Assert.That(noChild, Is.Null);
        Assert.That(child2Child1, Is.Not.Null);
        Assert.That(firstChild!.getOrDefault<string>("name"), Is.EqualTo("child1"));
        Assert.That(firstChildCached!.getOrDefault<string>("name"), Is.EqualTo("child1"));
        Assert.That(item1!.getOrDefault<string>("name"), Is.EqualTo("item1"));
        Assert.That(item12!.getOrDefault<string>("name"), Is.EqualTo("item1"));
        Assert.That(child2Child1!.getOrDefault<string>("name"), Is.EqualTo("child2child1"));
    }

    [Test]
    public void TestByFullname()
    {
        var extent = GetTestExtent();
        var firstChild = extent.GetUriResolver().Resolve(TestUri + "?fn=item2::child1", ResolveType.Default)
            as MofElement;
        var noChild = extent.GetUriResolver().Resolve(TestUri + "?fn=none", ResolveType.Default)
            as MofElement;
        var child2Child1 = extent.GetUriResolver()
                .Resolve(TestUri + "?fn=item2::child2::child2child1", ResolveType.Default)
            as MofElement;
        var item1 = extent.GetUriResolver().Resolve(TestUri + "?fn=item1", ResolveType.Default)
            as MofElement;

        Assert.That(firstChild, Is.Not.Null);
        Assert.That(item1, Is.Not.Null);
        Assert.That(noChild, Is.Null);
        Assert.That(child2Child1, Is.Not.Null);
        Assert.That(firstChild!.getOrDefault<string>("name"), Is.EqualTo("child1"));
        Assert.That(item1!.getOrDefault<string>("name"), Is.EqualTo("item1"));
        Assert.That(child2Child1!.getOrDefault<string>("name"), Is.EqualTo("child2child1"));
    }

    [Test]
    public void TestByProperty()
    {
        var extent = GetTestExtent();
        var firstChild = extent.GetUriResolver()
                .Resolve(TestUri + "?fn=item2&prop=packagedElement", ResolveType.Default)
            as IReflectiveSequence;

        Assert.That(firstChild, Is.Not.Null);
        var asList = firstChild!.ToList<object>();

        Assert.That(asList.Count, Is.EqualTo(3));
        Assert.That(
            asList.OfType<IElement>().Any(x => x.getOrDefault<string>("name") == "child2"),
            Is.True);
        Assert.That(
            asList.OfType<IElement>().Any(x => x.getOrDefault<string>("name") == "child1"),
            Is.True);
    }

    [Test]
    public void TestByDataView()
    {
        var extent = GetTestExtent();
        var firstChild = extent.GetUriResolver().Resolve(
                TestUri + "?fn=item2&prop=packagedElement&dataview=%23child1Filter",
                ResolveType.Default)
            as IReflectiveSequence;

        Assert.That(firstChild, Is.Not.Null);
        var asList = firstChild!.ToList<object>();

        Assert.That(asList.Count, Is.EqualTo(1));
        Assert.That(
            asList.OfType<IElement>().All(x => x.getOrDefault<string>("name") != "child2"),
            Is.True);
        Assert.That(
            asList.OfType<IElement>().Any(x => x.getOrDefault<string>("name") == "child1"),
            Is.True);
    }

    [Test]
    public void TestByMofShadow()
    {
        var extent = GetTestExtent();
        var firstChild = extent.GetUriResolver().Resolve(TestUri + "#child1", ResolveType.Default)
            as MofElement;
        var asShadow = new MofObjectShadow(TestUri + "#child1");

        Assert.That(firstChild, Is.Not.Null);

        var resolvedChildDirectly = extent.GetUriResolver().ResolveElement(firstChild!, ResolveType.Default, false);
        Assert.That(resolvedChildDirectly, Is.Not.Null);
        Assert.That(resolvedChildDirectly, Is.EqualTo(firstChild));


        var resolvedChildShadow = extent.GetUriResolver()
            .ResolveElement(asShadow, ResolveType.IncludeExtent, false);
        Assert.That(resolvedChildShadow, Is.Not.Null);
        Assert.That(resolvedChildShadow, Is.EqualTo(firstChild));
    }

    [Test]
    public void TestGetExtentPerResolver()
    {
        var extent = GetTestExtent();
        var found = extent.GetUriResolver().Resolve(TestUri, ResolveType.Default);
        Assert.That(found, Is.Not.Null);
        Assert.That(found is IUriExtent, Is.True);
        Assert.That(found!.Equals(extent));
    }
        
    [Test]
    public void TestGetCompositesAllReferencedIncludingSelf()
    {
        var extent = GetCompositeTestExtent();
        var found = extent.GetUriResolver().Resolve(TestUri + "?composites=allReferencedIncludingSelf", ResolveType.Default, false)
            as IReflectiveCollection;
            
        Assert.That(found, Is.Not.Null);
        Assert.That(found!.OfType<IElement>().Any(x=>x.getOrDefault<string>("name") == "child2child1"), Is.True);
        Assert.That(found!.OfType<IElement>().Any(x=>x.getOrDefault<string>("name") == "item2"), Is.True);
        Assert.That(found!.Count(), Is.EqualTo(11));
    }
        
    [Test]
    public void TestGetCompositesAllReferencedOfItem()
    {
        var extent = GetCompositeTestExtent();
        var found = extent.GetUriResolver().Resolve(TestUri + "?fn=item2&composites=allReferenced", ResolveType.Default, false)
            as IReflectiveCollection;
            
        Assert.That(found, Is.Not.Null);
        Assert.That(found!.OfType<IElement>().Any(x=>x.getOrDefault<string>("name") == "child2child1"), Is.True);
        Assert.That(found!.OfType<IElement>().Any(x=>x.getOrDefault<string>("name") == "item2"), Is.False);
        Assert.That(found!.Count(), Is.EqualTo(8));
    }
        
    [Test]
    public void TestGetCompositesAllReferencedAndSelfOfItem()
    {
        var extent = GetCompositeTestExtent();
        var found = extent.GetUriResolver().Resolve(TestUri + "?fn=item2&composites=allReferencedIncludingSelf", ResolveType.Default, false)
            as IReflectiveCollection;
            
        Assert.That(found, Is.Not.Null);
        Assert.That(found!.OfType<IElement>().Any(x=>x.getOrDefault<string>("name") == "child2child1"), Is.True);
        Assert.That(found!.OfType<IElement>().Any(x=>x.getOrDefault<string>("name") == "item2"), Is.True);
        Assert.That(found!.Count(), Is.EqualTo(9));
    }
        
    [Test]
    public void TestFilterByMetaClasses()
    {
        var extent = GetCompositeTestExtent();
        var found = extent.GetUriResolver().Resolve(
                TestUri + "?metaClass=" + HttpUtility.UrlEncode("dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package"), 
                ResolveType.Default, 
                false)
            as IReflectiveCollection;
            
        Assert.That(found, Is.Not.Null);
        Assert.That(found!.OfType<IElement>().Any(x=>x.getOrDefault<string>("name") == "item1"), Is.True);
        Assert.That(found!.OfType<IElement>().Any(x=>x.getOrDefault<string>("name") == "item2"), Is.False);
        Assert.That(found!.Count(), Is.EqualTo(2));
    }
        
    [Test]
    public void TestFilterByMetaClassesAndReferenced()
    {
        var extent = GetCompositeTestExtent();
        var found = extent.GetUriResolver().Resolve(
                TestUri + "?metaClass=" + HttpUtility.UrlEncode("dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package")
                + "&composites=allReferencedIncludingSelf", 
                ResolveType.Default, 
                false)
            as IReflectiveCollection;
            
        Assert.That(found, Is.Not.Null);
        Assert.That(found!.OfType<IElement>().Any(x=>x.getOrDefault<string>("name") == "item1"), Is.True);
        Assert.That(found!.OfType<IElement>().Any(x=>x.getOrDefault<string>("name") == "child2"), Is.True);
        Assert.That(found!.OfType<IElement>().Any(x=>x.getOrDefault<string>("name") == "item2"), Is.False);
        Assert.That(found!.Count(), Is.EqualTo(3));
    }

    [Test]
    public void TestResolveHook()
    {
        var extent = GetTestExtent();
        var hook = new TestResolveHookClass();
        extent.ScopeStorage!.Get<ResolveHookContainer>().Add(hook);

        Assert.That(hook.Counts, Is.EqualTo(0));
        var firstChild = extent.GetUriResolver().Resolve(TestUri + "?count=4#child1", ResolveType.Default);
        Assert.That(firstChild, Is.Not.Null);
        Assert.That(hook.Counts, Is.EqualTo(4));
    }

    [Test]
    public async Task TestNeighborWorkspace()
    {
        var scopeStorage = new ScopeStorage();
        ResolveHookContainer.AddDefaultHooks(scopeStorage);
        var workspaceLogic = new WorkspaceLogic(scopeStorage);
        var mapper = scopeStorage.Get<ProviderToProviderLoaderMapper>();
            
        mapper.AddMapping(
            _ExtentLoaderConfigs.TheOne.__InMemoryLoaderConfig,
            _ => new InMemoryProviderLoader());
        
        workspaceLogic.AddWorkspace(new Workspace("data1", "Test"));
        workspaceLogic.AddWorkspace(new Workspace("data2", "Test"));

        var factory = InMemoryObject.TemporaryFactory;
        var extentManager = new ExtentManager(workspaceLogic, scopeStorage);

        var configuration = new ExtentLoaderConfigs.InMemoryLoaderConfig_Wrapper(factory)
        {
            name = "Data1",
            extentUri = "dm:///data1/",
            workspaceId = "data1"
        };
        var data1 = await extentManager.LoadExtent(configuration.GetWrappedElement());
        Assert.That(data1.Extent, Is.Not.Null);
        
        var configuration2 = new ExtentLoaderConfigs.InMemoryLoaderConfig_Wrapper(factory)
        {
            name = "Data2",
            extentUri = "dm:///data2/",
            workspaceId = "data2"
        };
        var data2 = await extentManager.LoadExtent(configuration2.GetWrappedElement());
        Assert.That(data2.Extent, Is.Not.Null);
        
        var typeIndexLogic = new TypeIndexLogic(workspaceLogic);
        typeIndexLogic.PerformIndexing();

        var factory1 = new MofFactory(data1.Extent!);
        var factory2 = new MofFactory(data2.Extent!);

        var item1 = factory1.create(null);
        (item1 as ICanSetId)!.Id = "item1";
        item1.set("name", "item1");
        data1.Extent!.elements().add(item1);
        var item2 = factory2.create(null);
        (item2 as ICanSetId)!.Id = "item2";
        item2.set("name", "item2");
        data2.Extent!.elements().add(item2);
        
        // Now, we do the test that we don't find it when just looking into the current workspace
        var item2FromCurrent = data1.Extent.GetUriResolver().Resolve("dm:///data2/#item2", ResolveType.IncludeWorkspace) as MofObject;
        Assert.That(item2FromCurrent, Is.Null);
        
        // Now, we do the test that we don't find it when just looking into the current extent
        item2FromCurrent = data1.Extent.GetUriResolver().Resolve("dm:///data2/#item2", ResolveType.IncludeExtent) as MofObject;
        Assert.That(item2FromCurrent, Is.Null);
        
        // Now, we do the test that we don't find it when just looking into the metaworkspaces
        item2FromCurrent = data1.Extent.GetUriResolver().Resolve("dm:///data2/#item2", ResolveType.IncludeMetaWorkspaces) as MofObject;
        Assert.That(item2FromCurrent, Is.Null);
        
        // Now, we do the test that we don't find it when just looking into the metaworkspaces
        item2FromCurrent = data1.Extent.GetUriResolver().Resolve("dm:///data2/#item2", ResolveType.IncludeAll) as MofObject;
        Assert.That(item2FromCurrent, Is.Not.Null);
        Assert.That(item2FromCurrent!.getOrDefault<string>("name"), Is.EqualTo("item2"));
        
        // Now, we do the test that we don't find it when just looking into the metaworkspaces
        item2FromCurrent = data1.Extent.GetUriResolver().Resolve("dm:///data2/#item2", ResolveType.IncludeNeighboringWorkspaces) as MofObject;
        Assert.That(item2FromCurrent, Is.Not.Null);
        Assert.That(item2FromCurrent!.getOrDefault<string>("name"), Is.EqualTo("item2"));
        
        // Now do the resolver tests
        var item1FoundFrom1 = data1.Extent.GetUriResolver().Resolve("dm:///data1/#item1", ResolveType.Default) as MofObject;
        var item2FoundFrom2 = data2.Extent.GetUriResolver().Resolve("dm:///data2/#item2", ResolveType.Default) as MofObject;
        var item2FoundFrom1 = data1.Extent.GetUriResolver().Resolve("dm:///data2/#item2", ResolveType.Default) as MofObject;
        var item1FoundFrom2 = data2.Extent.GetUriResolver().Resolve("dm:///data1/#item1", ResolveType.Default) as MofObject;
        
        Assert.That(item1FoundFrom1,Is.Not.Null);
        Assert.That(item1FoundFrom1!.getOrDefault<string>("name"), Is.EqualTo("item1"));
        Assert.That(item2FoundFrom2,Is.Not.Null);
        Assert.That(item2FoundFrom2!.getOrDefault<string>("name"), Is.EqualTo("item2"));
        
        Assert.That(item1FoundFrom2,Is.Not.Null);
        Assert.That(item1FoundFrom2!.getOrDefault<string>("name"), Is.EqualTo("item1"));
        Assert.That(item2FoundFrom1,Is.Not.Null);
        Assert.That(item2FoundFrom1!.getOrDefault<string>("name"), Is.EqualTo("item2"));
    }

    public class TestResolveHookClass : IResolveHook
    {
        public int Counts { get; set; }

        public object? Resolve(ResolveHookParameters hookParameters)
        {
            if (hookParameters.QueryString["count"] != null)
            {
                Counts += Convert.ToInt32(hookParameters.QueryString["count"]);
            }

            return hookParameters.CurrentItem;
        }
    }

    /// <summary>
    /// Defines the test uri
    /// </summary>
    private const string TestUri = "dm:///Test";

    /// <summary>
    /// Gets the test extent
    /// </summary>
    /// <returns>The extent being used for test extent</returns>
    private static MofUriExtent GetTestExtent()
    {
        var document = @"
<item xmlns:p1=""http://www.omg.org/spec/XMI/20131001"">
    <item p1:id=""item1"" name=""item1"" />
    <item p1:id=""item2"" name=""item2"">
        <packagedElement p1:id=""child1"" name=""child1"" />
        <packagedElement p1:id=""child2"" name=""child2"">
            <packagedElement p1:id=""child2child1"" name=""child2child1"" />
        </packagedElement>
        <packagedElement p1:id=""child3"" name=""child3"" />
    </item>
    <item p1:id=""item3"" name=""item3"" /> 
    
    <item p1:type=""dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package"" name=""Filter Open"">
        <packagedElement p1:type=""dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterByPropertyValueNode""
            name=""Open Issues""
            input-ref=""#filterNode1"" property=""name"" value=""child1"" comparisonMode=""Equal""
                p1:id=""child1Filter"" />
        <packagedElement
            p1:type=""dm:///_internal/types/internal#DatenMeister.Models.DataViews.DynamicSourceNode""
            name=""input"" p1:id=""filterNode1""/>
    </item>
</item>";

        var scopeStorage = new ScopeStorage();
        ResolveHookContainer.AddDefaultHooks(scopeStorage);
        var workspaceLogic = new WorkspaceLogic(scopeStorage);
        var dataViewPlugin = new DataViewPlugin(
            workspaceLogic, new DataViewLogic(workspaceLogic, scopeStorage), scopeStorage);
        dataViewPlugin.StartThrough();
            
            

        var provider = new XmiProvider(XDocument.Parse(document));
        return new MofUriExtent(provider, TestUri, scopeStorage);
    }
        

    /// <summary>
    /// Gets the test extent
    /// </summary>
    /// <returns>The extent being used for test extent</returns>
    private static MofUriExtent GetCompositeTestExtent()
    {
        var document = @"
<item xmlns:p1=""http://www.omg.org/spec/XMI/20131001"">
    <item p1:id=""item1"" name=""item1"" p1:type=""dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package""/>
    <item p1:id=""item2"" name=""item2""  >
        <packagedElement p1:id=""child1"" name=""child1"" />
        <packagedElement p1:id=""child2"" name=""child2"" p1:type=""dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package"">
            <packagedElement p1:id=""child2child1"" name=""child2child1"" />
            <packagedElement p1:id=""child2child2"" name=""child2child2"" />
            <packagedElement p1:id=""child2child3"" name=""child2child3"" />
        </packagedElement>
        <packagedElement p1:id=""child3"" name=""child3"">
            <packagedElement p1:id=""child3child1"" name=""child3child1"" />
            <packagedElement p1:id=""child3child2"" name=""child3child2"" />            
        </packagedElement>
    </item>
    <item p1:id=""item3"" name=""item3"" p1:type=""dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package"" /> 
</item>";

        var scopeStorage = new ScopeStorage();
        ResolveHookContainer.AddDefaultHooks(scopeStorage);
        var provider = new XmiProvider(XDocument.Parse(document));
        return new MofUriExtent(provider, TestUri, scopeStorage);
    }
}