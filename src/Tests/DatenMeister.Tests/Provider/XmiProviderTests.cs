using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Provider
{
    [TestFixture]
    public class XmiProviderTests
    {
        [Test]
        public void TestGetAndSet()
        {
            var provider = new XmiProvider();
            ProviderTestHelper.TestGetAndSetOfPrimitiveTypes(provider);
        }

        [Test]
        public void TestLists()
        {
            var provider = new XmiProvider();
            ProviderTestHelper.TestListsWithObjects(provider);
        }

        [Test]
        public void TestMovements()
        {
            var provider = new XmiProvider();
            ProviderTestHelper.TestListMovement(provider);
        }

        [Test]
        public void TestSetReferenceAndSetValue()
        {
            var provider = new XmiProvider();
            ProviderTestHelper.TestSetReferenceAndSetValue(provider);
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

            var element = MofFactory.Create(mofExtent, null);
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

            var result = element.getOrDefault<IReflectiveCollection>("test");
            Assert.That(result, Is.Not.Null);

            Assert.That(result.Count(), Is.EqualTo(4));
            Assert.That(result.ElementAt(0), Is.EqualTo("ABC"));
            Assert.That(result.ElementAt(1), Is.EqualTo("DEF"));
            Assert.That(result.ElementAt(2), Is.EqualTo("GHI"));
            Assert.That(result.ElementAt(3), Is.EqualTo("JKL"));
        }
    }
}