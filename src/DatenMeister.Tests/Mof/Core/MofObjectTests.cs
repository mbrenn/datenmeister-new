using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Proxies;
using DatenMeister.Runtime;
using NUnit.Framework;

namespace DatenMeister.Tests.Mof.Core
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für MofObjectTests
    /// </summary>
    [TestFixture]
    public class MofObjectTests
    {
        private static string property1 = "Prop1";
        private static string property2 = "Prop2";

        [Test]
        public void TestInMemoryMofObject()
        {
            var provider = new InMemoryProvider();
            var mofObject = new InMemoryObject(provider);
            Assert.That(mofObject.IsPropertySet(property1), Is.False);
            mofObject.SetProperty(property1, "Test");
            mofObject.SetProperty(property2, property1);

            Assert.That(mofObject.IsPropertySet(property1), Is.True);
            Assert.That(mofObject.IsPropertySet(property2), Is.True);

            Assert.That(mofObject.GetProperty(property1).ToString(), Is.EqualTo("Test"));
            Assert.That(mofObject.GetProperty(property2), Is.EqualTo(property1));
        }

        [Test]
        public void TestSetInMemory()
        {
            var provider = new InMemoryProvider();
            var mofObject = new InMemoryObject(provider);
            Assert.That(mofObject.IsPropertySet(property1), Is.False);
            mofObject.SetProperty(property1, "Test");
            mofObject.SetProperty(property2, 2);

            Assert.That(mofObject.IsPropertySet(property1), Is.True);
            Assert.That(mofObject.IsPropertySet(property2), Is.True);

            Assert.That(mofObject.GetProperty(property1).ToString(), Is.EqualTo("Test"));
            Assert.That(mofObject.GetProperty(property2), Is.EqualTo(2));
        }

        [Test]
        public void TestStoreAndFindObject()
        {
            var provider = new InMemoryProvider();
            var otherMofElement = InMemoryObject.CreateEmpty();
            var mofInstance = new MofUriExtent(new InMemoryProvider(), "dm:///test");
            var mofElement = new MofElement(new InMemoryObject(provider), mofInstance);
            mofInstance.elements().add(mofElement);
            mofInstance.elements().add(otherMofElement);

            // Gets the uris
            var uri1 = mofInstance.uri(mofElement);
            var uri2 = mofInstance.uri(otherMofElement);

            Assert.That(uri1, Is.Not.Null);

            // Gets the instances
            var found1 = mofInstance.element(uri1);
            var found2 = mofInstance.element(uri2);

            Assert.That(found1, Is.Not.Null);
            Assert.That(found2, Is.Not.Null);
            Assert.That(found1, Is.EqualTo(mofElement));
            Assert.That(found2, Is.EqualTo(otherMofElement));
        }

        [Test]
        public void TestProxyForUriExtent()
        {
            var uriExtent = new MofUriExtent(new InMemoryProvider(), "dm:///test");
            var proxiedUriExtent = new ProxyUriExtent(uriExtent).ActivateObjectConversion();

            var mofElement = InMemoryObject.CreateEmpty();
            var otherMofElement = InMemoryObject.CreateEmpty();
            proxiedUriExtent.elements().add(mofElement);
            proxiedUriExtent.elements().add(otherMofElement);

            var returned = proxiedUriExtent.elements().ElementAt(0);
            Assert.That(returned, Is.TypeOf<ProxyMofElement>());

            var proxiedElement = returned as ProxyMofElement;
            Assert.That(proxiedElement, Is.Not.Null);
            Assert.That(proxiedElement.GetProxiedElement(), Is.TypeOf<MofElement>());

            Assert.That(proxiedUriExtent.elements().remove(proxiedElement), Is.True);
            Assert.That(proxiedUriExtent.elements().size, Is.EqualTo(1));
            proxiedUriExtent.elements().clear();
            
            // Check, if the dereferencing of ProxyMofElements are working
            proxiedUriExtent.elements().add(proxiedElement);
            returned = proxiedUriExtent.elements().ElementAt(0);
            Assert.That(returned, Is.TypeOf<ProxyMofElement>());

            proxiedElement = returned as ProxyMofElement;
            Assert.That(proxiedElement.GetProxiedElement(), Is.TypeOf<MofElement>());
        }

        [Test]
        public void TestKnowsExtent()
        {
            var uriExtent = new MofUriExtent(new InMemoryProvider(), "dm:///test");
            var factory = new MofFactory(uriExtent);

            var mofElement = factory.create(null);
            var otherMofElement = InMemoryObject.CreateEmpty(); 
            var innerMofElement = factory.create(null); 

            Assert.That(((IHasExtent)mofElement).Extent, Is.Null);
            Assert.That(((IHasExtent) otherMofElement).Extent, Is.Null);

            uriExtent.elements().add(mofElement);

            Assert.That(((IHasExtent)mofElement).Extent, Is.SameAs(uriExtent));
            Assert.That(((IHasExtent) otherMofElement).Extent, Is.Null);

            uriExtent.elements().add(otherMofElement);

            Assert.That(((IHasExtent)mofElement).Extent, Is.SameAs(uriExtent));
            Assert.That(((IHasExtent)otherMofElement).Extent, Is.SameAs(uriExtent));

            otherMofElement.set("Test", innerMofElement);
            Assert.That(((IHasExtent)otherMofElement.get("Test")).Extent, Is.SameAs(uriExtent));
        }

        [Test]
        public void TestGetGenerics()
        {
            var value = InMemoryObject.CreateEmpty();
            value.set("nr", 42);
            value.set("string", "ABC");
            value.set("collection", new object[] {43, 2, 321, 231});
            var otherValue = InMemoryObject.CreateEmpty();
            otherValue.set("name", "Martin");

            value.set("mof", otherValue);

            Assert.That(value.getOrDefault<string>("nr"), Is.EqualTo("42"));
            Assert.That(value.getOrDefault<int>("nr"), Is.EqualTo(42));
            Assert.That(value.getOrDefault<string>("string"), Is.EqualTo("ABC"));
            Assert.That(value.getOrDefault<IReflectiveCollection>("collection").Count(), Is.EqualTo(4));
            Assert.That(value.getOrDefault<IReflectiveCollection>("collection").ElementAt(2), Is.EqualTo(321));
            Assert.That(value.getOrDefault<IObject>("mof").get<string>("name"), Is.EqualTo("Martin"));
            Assert.That(value.getOrDefault<IElement>("mof").get<string>("name"), Is.EqualTo("Martin"));
        }

        [Test]
        public void TestSetDotNet()
        {
            var uriExtent = new MofUriExtent(new InMemoryProvider(), "dm:///test");
            var factory = new MofFactory(uriExtent);

            var mofElement = factory.create(null);
            var value = new Point(23, 24);
            mofElement.set("point", value);

            var retrieve = mofElement.get("point");
            Assert.That(retrieve is IObject, Is.True);
            var asObject = (IObject) retrieve;
            Assert.That(asObject.get("X"), Is.EqualTo(23));
            Assert.That(asObject.get("Y"), Is.EqualTo(24));
        }

        /// <summary>
        /// Defines the point class being used for the test above
        /// </summary>
        public class Point
        {
            /// <summary>
            /// Initializes a new instance of the Point class
            /// </summary>
            /// <param name="x">The first coordinate</param>
            /// <param name="y">The second coordinate</param>
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            /// <summary>
            /// Gets the first coordinate
            /// </summary>
            public int X { get; }

            /// <summary>
            /// Gets the second coordinate
            /// </summary>
            public int Y { get; }
        }
    }
}
