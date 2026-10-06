using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Provider;
using DatenMeister.Core.Provider;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Provider;

[TestFixture]
public class XmiProviderTests
{
    [Test]
    public void TestSuite()
    {
        var provider = new XmiProvider();
        ProviderTestSuite.TestProviderObject(provider);
    }

    [Test]
    public void TestStringReferenceInXmi()
    {
        var x =
            @"
<xml xmlns:p1=""http://www.omg.org/spec/XMI/20131001"">
    <node element=""test"" p1:id=""first"" />
    <node element=""first"" p1:id=""second"" />
</xml>";
        var xmiProvider = new XmiProvider(XDocument.Parse(x));
        var uriExtent = new MofUriExtent(xmiProvider, null);

        var first = uriExtent.elements().ElementAt(0) as IElement;
        var second = uriExtent.elements().ElementAt(1) as IElement;

        Assert.That(first, Is.Not.Null);
        Assert.That(second, Is.Not.Null);

        Assert.That((second as IHasId)!.Id, Is.EqualTo("second"));
        Assert.That((first as IHasId)!.Id, Is.EqualTo("first"));

        Assert.That(second.getOrDefault<string>("element"), Is.EqualTo("first"));
        var secondElement = second.getOrDefault<IElement>("element");
        Assert.That(secondElement, Is.Not.Null);
        Assert.That((secondElement as IHasId)!.Id, Is.EqualTo("first"));
    }

    [Test]
    public void TestAddAndRemoveOfElementsInSequenceInMemoryProvider()
    {
        var inMemoryProvider = new InMemoryProvider();
        var xmiProvider = new XmiProvider();

        var workspace = new Workspace("Test");
        var extent1 = new MofUriExtent(inMemoryProvider, "dm:///extent1", null);
        var extent2 = new MofUriExtent(xmiProvider, "dm:///extent2", null);
        workspace.AddExtent(extent1);
        workspace.AddExtent(extent2);

        var factory1 = new MofFactory(extent1);
        var factory2 = new MofFactory(extent2);

        var element1 = factory1.create(null);
        var element2 = factory2.create(null);

        extent1.elements().add(element1);
        extent2.elements().add(element2);

        var collection = element1.get<IReflectiveCollection>("queue");
        Assert.That(collection.Count(), Is.EqualTo(0));
        Assert.That(element1.get<IReflectiveCollection>("queue").Count(), Is.EqualTo(0));
        collection.add(element2);
        Assert.That(collection.Count(), Is.EqualTo(1));
        Assert.That(element1.get<IReflectiveCollection>("queue").Count(), Is.EqualTo(1));

        collection.remove(element2);
        Assert.That(collection.Count(), Is.EqualTo(0));
        Assert.That(element1.get<IReflectiveCollection>("queue").Count(), Is.EqualTo(0));
    }

    [Test]
    public void TestAddAndRemoveOfElementsInSequenceInXmiProvider()
    {
        var inMemoryProvider = new InMemoryProvider();
        var xmiProvider = new XmiProvider();

        var workspace = new Workspace("Test");
        var extent1 = new MofUriExtent(xmiProvider, "dm:///extent2", null);
        var extent2 = new MofUriExtent(inMemoryProvider, "dm:///extent1", null);

        workspace.AddExtent(extent1);
        workspace.AddExtent(extent2);

        var factory1 = new MofFactory(extent1);
        var factory2 = new MofFactory(extent2);

        var element1 = factory1.create(null);
        var element2 = factory2.create(null);

        extent1.elements().add(element1);
        extent2.elements().add(element2);

        var collection = element1.get<IReflectiveCollection>("queue");
        Assert.That(collection.Count(), Is.EqualTo(0));
        Assert.That(element1.get<IReflectiveCollection>("queue").Count(), Is.EqualTo(0));
        collection.add(element2);
        Assert.That(collection.Count(), Is.EqualTo(1));
        Assert.That(element1.get<IReflectiveCollection>("queue").Count(), Is.EqualTo(1));

        collection.remove(element2);
        Assert.That(collection.Count(), Is.EqualTo(0));
        Assert.That(element1.get<IReflectiveCollection>("queue").Count(), Is.EqualTo(0));
    }

    [Test]
    public void TestStringsInReflectiveCollection()
    {
        var provider = new XmiProvider();
        var mofExtent = new MofUriExtent(provider, "dm:///test", null);

        var element = MofFactory.CreateElement(mofExtent, null);
        mofExtent.elements().add(element);

        var list = new List<string> {"ABC", "DEF", "GHI", "JKL"};
        element.set("test", list);

        var result = element.getOrDefault<IReflectiveCollection>("test");
        Assert.That(result, Is.Not.Null);

        Assert.That(result.Count(), Is.EqualTo(4));
        Assert.That(result.ElementAt(0), Is.EqualTo("ABC"));
        Assert.That(result.ElementAt(1), Is.EqualTo("DEF"));
        Assert.That(result.ElementAt(2), Is.EqualTo("GHI"));
        Assert.That(result.ElementAt(3), Is.EqualTo("JKL"));
    }

    [Test]
    public void TestStringsInReflectiveCollectionByCml()
    {
        var document = XDocument.Parse(
            "<item>" +
            "  <element>" +
            "    <test>ABC</test>" +
            "    <test>DEF</test>" +
            "    <test>GHI</test>" +
            "    <test>JKL</test>" +
            "  </element>" +
            "</item>");
        var provider = new XmiProvider(document);
        var mofExtent = new MofUriExtent(provider, "dm:///test", null);

        var element = mofExtent.elements().First() as IElement;
        Assert.That(element, Is.Not.Null);

        var result = element!.getOrDefault<IReflectiveCollection>("test");
        Assert.That(result, Is.Not.Null);

        Assert.That(result.Count(), Is.EqualTo(4));
        Assert.That(result.ElementAt(0), Is.EqualTo("ABC"));
        Assert.That(result.ElementAt(1), Is.EqualTo("DEF"));
        Assert.That(result.ElementAt(2), Is.EqualTo("GHI"));
        Assert.That(result.ElementAt(3), Is.EqualTo("JKL"));
    }

    /// <summary>
    /// In Issue #13, the Xml Node is moved before the meta extent, if it was already the first extent
    /// </summary>
    [Test]
    public void AvoidMovingUpBeforeMetaExtent()
    {
        var provider = new XmiProvider();
        var mofExtent = new MofUriExtent(provider, "dm:///test", null);
        var factory = new MofFactory(mofExtent);
            
        mofExtent.GetMetaObject().set("Name", "Martin");

        Assert.That(provider.Document.Root!.Elements().First().Name.LocalName == "meta", Is.True);

        var element1 = factory.create(null);
        mofExtent.elements().add(element1);
        var element2 = factory.create(null);
        element2.set("Name", "Martin2");
        mofExtent.elements().add(element2);
        mofExtent.elements().add(factory.create(null));
            
        Assert.That(provider.Document.Root!.Elements().First().Name.LocalName == "meta", Is.True);

        CollectionHelper.MoveElementUp(mofExtent.elements(), element1);

        Assert.That(provider.Document.Root!.Elements().First().Name.LocalName == "meta", Is.True);
            
        CollectionHelper.MoveElementUp(mofExtent.elements(), element2);
        CollectionHelper.MoveElementUp(mofExtent.elements(), element2);
        Assert.That(provider.Document.Root!.Elements().First().Name.LocalName == "meta", Is.True);
        Assert.That(provider.Document.Root!.Elements().First().Name.LocalName == "meta", Is.True);
            
        // Checks, that the element2 has been really moved up
        Assert.That(mofExtent.elements().OfType<IElement>().First().get("Name"), Is.EqualTo("Martin2"));
    }

    [Test]
    public void TestMoveElementUpAndDownPrimitiveValues()
    {
        var provider = new XmiProvider();
        var root = (XmiProviderObject)provider.CreateElement(null);
        provider.AddElement(root);

        root.AddToProperty("items", "first");
        root.AddToProperty("items", "second");
        root.AddToProperty("items", "third");

        Assert.That(root.GetProperty("items"), Is.EquivalentTo(new[] { "first", "second", "third" }));

        // Move "second" up -> should be "second", "first", "third"
        var movedUp = root.MoveElementUp("items", "second");
        Assert.That(movedUp, Is.True);
        Assert.That(root.GetProperty("items"), Is.EqualTo(new[] { "second", "first", "third" }));

        // Move "second" up again -> already at top, remains unchanged
        movedUp = root.MoveElementUp("items", "second");
        Assert.That(movedUp, Is.True);
        Assert.That(root.GetProperty("items"), Is.EqualTo(new[] { "second", "first", "third" }));

        // Move "second" down -> should be "first", "second", "third"
        var movedDown = root.MoveElementDown("items", "second");
        Assert.That(movedDown, Is.True);
        Assert.That(root.GetProperty("items"), Is.EqualTo(new[] { "first", "second", "third" }));

        // Move "third" down -> already at bottom, remains unchanged
        movedDown = root.MoveElementDown("items", "third");
        Assert.That(movedDown, Is.True);
        Assert.That(root.GetProperty("items"), Is.EqualTo(new[] { "first", "second", "third" }));
    }

    [Test]
    public void TestMoveElementBoundariesAcrossMultipleProperties()
    {
        var provider = new XmiProvider();
        var root = (XmiProviderObject)provider.CreateElement(null);
        provider.AddElement(root);

        root.AddToProperty("propA", "a1");
        root.AddToProperty("propA", "a2");
        root.AddToProperty("propB", "b1");
        root.AddToProperty("propB", "b2");

        // Try moving b1 up in propB - should stay at top of propB and NOT move into propA
        var movedUp = root.MoveElementUp("propB", "b1");
        Assert.That(movedUp, Is.True);
        Assert.That(root.GetProperty("propA"), Is.EqualTo(new[] { "a1", "a2" }));
        Assert.That(root.GetProperty("propB"), Is.EqualTo(new[] { "b1", "b2" }));

        // Try moving a2 down in propA - should stay at bottom of propA and NOT move past propB
        var movedDown = root.MoveElementDown("propA", "a2");
        Assert.That(movedDown, Is.True);
        Assert.That(root.GetProperty("propA"), Is.EqualTo(new[] { "a1", "a2" }));
        Assert.That(root.GetProperty("propB"), Is.EqualTo(new[] { "b1", "b2" }));
    }

    [Test]
    public void TestMoveElementWithUriReference()
    {
        var provider = new XmiProvider();
        var root = (XmiProviderObject)provider.CreateElement(null);
        provider.AddElement(root);

        var ref1 = new UriReference("dm:///ref1");
        var ref2 = new UriReference("dm:///ref2");
        var ref3 = new UriReference("dm:///ref3");

        root.AddToProperty("refs", ref1);
        root.AddToProperty("refs", ref2);
        root.AddToProperty("refs", ref3);

        var movedUp = root.MoveElementUp("refs", ref2);
        Assert.That(movedUp, Is.True);

        var list = ((IEnumerable<object>)root.GetProperty("refs")!).Cast<UriReference>().Select(r => r.Uri).ToList();
        Assert.That(list, Is.EqualTo(new[] { "dm:///ref1", "dm:///ref2", "dm:///ref3" }.OrderBy(x => x == "dm:///ref2" ? 0 : 1).ToList()));
        Assert.That(list[0], Is.EqualTo("dm:///ref2"));
        Assert.That(list[1], Is.EqualTo("dm:///ref1"));
        Assert.That(list[2], Is.EqualTo("dm:///ref3"));
    }

    [Test]
    public void TestSetCollectionPropertyDirectly()
    {
        var provider = new XmiProvider();
        var root = (XmiProviderObject)provider.CreateElement(null);
        provider.AddElement(root);

        var items = new List<string> { "item1", "item2", "item3" };
        root.SetProperty("myList", items);

        var retrieved = root.GetProperty("myList");
        Assert.That(retrieved, Is.EquivalentTo(items));
    }
}