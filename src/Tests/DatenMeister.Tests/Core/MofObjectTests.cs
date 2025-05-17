using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Proxies;
using NUnit.Framework;

namespace DatenMeister.Tests.Core
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

            Assert.That(mofObject.GetProperty(property1)!.ToString(), Is.EqualTo("Test"));
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

            Assert.That(mofObject.GetProperty(property1)?.ToString(), Is.EqualTo("Test"));
            Assert.That(mofObject.GetProperty(property2), Is.EqualTo(2));
        }

        [Test]
        public void TestStoreAndFindObject()
        {
            var provider = new InMemoryProvider();
            var otherMofElement = InMemoryObject.CreateEmpty();
            var mofInstance = new MofUriExtent(new InMemoryProvider(), "dm:///test", null);
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
            var uriExtent = new MofUriExtent(new InMemoryProvider(), "dm:///test", null);
            var proxiedUriExtent = new ProxyUriExtent(uriExtent).ActivateObjectConversion();

            var mofElement = InMemoryObject.CreateEmpty();
            var otherMofElement = InMemoryObject.CreateEmpty();
            proxiedUriExtent.elements().add(mofElement);
            proxiedUriExtent.elements().add(otherMofElement);

            var returned = proxiedUriExtent.elements().ElementAt(0);
            Assert.That(returned, Is.TypeOf<ProxyMofElement>());

            var proxiedElement = returned as ProxyMofElement;
            Assert.That(proxiedElement, Is.Not.Null);
            Assert.That(proxiedElement!.GetProxiedElement(), Is.TypeOf<MofElement>());

            Assert.That(proxiedUriExtent.elements().remove(proxiedElement), Is.True);
            Assert.That(proxiedUriExtent.elements().size, Is.EqualTo(1));
            proxiedUriExtent.elements().clear();

            // Check, if the dereferencing of ProxyMofElements are working
            proxiedUriExtent.elements().add(proxiedElement);
            returned = proxiedUriExtent.elements().ElementAt(0);
            Assert.That(returned, Is.TypeOf<ProxyMofElement>());

            proxiedElement = returned as ProxyMofElement;
            Assert.That(proxiedElement!.GetProxiedElement(), Is.TypeOf<MofElement>());
        }

        [Test]
        public void TestKnowsExtent()
        {
            var uriExtent = new MofUriExtent(new InMemoryProvider(), "dm:///test", null);
            var factory = new MofFactory(uriExtent);

            var mofElement = factory.create(null);
            var otherMofElement = InMemoryObject.CreateEmpty();
            var innerMofElement = factory.create(null);

            Assert.That(((IHasExtent)mofElement).Extent, Is.Null);
            Assert.That(((IHasExtent)otherMofElement).Extent, Is.Null);

            uriExtent.elements().add(mofElement);

            Assert.That(((IHasExtent)mofElement).Extent, Is.SameAs(uriExtent));
            Assert.That(((IHasExtent)otherMofElement).Extent, Is.Null);

            uriExtent.elements().add(otherMofElement);

            Assert.That(((IHasExtent)mofElement).Extent, Is.SameAs(uriExtent));
            Assert.That(((IHasExtent)otherMofElement).Extent, Is.SameAs(uriExtent));

            otherMofElement.set("Test", innerMofElement);
            Assert.That(((IHasExtent)otherMofElement.get("Test")!).Extent, Is.SameAs(uriExtent));
        }

        [Test]
        public void TestGetGenerics()
        {
            var value = InMemoryObject.CreateEmpty();
            value.set("nr", 42);
            value.set("string", "ABC");
            value.set("collection", new object[] { 43, 2, 321, 231 });
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

        /// <summary>
        /// Returns a test in which the extent needs to resolve a string
        /// when a string is returned and an IObject or an IElement has been returned
        /// by the MofObject
        /// </summary>
        [Test]
        public void TestReferenceByStringReturn()
        {
            var uriExtent = new MofUriExtent(new InMemoryProvider(), "dm:///test", null);
            var factory = new MofFactory(uriExtent);

            var mofElement = factory.create(null);
            mofElement.set("name", "Name");
            var mofElement2 = factory.create(null);
            uriExtent.elements().add(mofElement);
            uriExtent.elements().add(mofElement2);

            var uri1 = mofElement.GetUri()!;
            Assert.That(uri1, Is.Not.Null);
            
            mofElement2.set("ref", uri1);
            mofElement2.set("direct-ref", mofElement);

            var uri1ReturnedBy2 = mofElement2.getOrDefault<string>("ref");
            Assert.That(uri1ReturnedBy2, Is.EqualTo(uri1));

            var directReturn = mofElement2.getOrDefault<IElement>("direct-ref");
            Assert.That(directReturn, Is.Not.Null);
            Assert.That(directReturn.getOrDefault<string>("name"), Is.EqualTo("Name"));
            
            var indirectReturn = mofElement2.getOrDefault<IElement>("ref");
            Assert.That(indirectReturn, Is.Not.Null);
            Assert.That(indirectReturn.getOrDefault<string>("name"), Is.EqualTo("Name"));
            
            var indirectReturnByObject = mofElement2.getOrDefault<IObject>("ref");
            Assert.That(indirectReturnByObject, Is.Not.Null);
            Assert.That(indirectReturnByObject.getOrDefault<string>("name"), Is.EqualTo("Name"));
        }

        [Test]
        public void TestSetDotNet()
        {
            var uriExtent = new MofUriExtent(new InMemoryProvider(), "dm:///test", null);
            var factory = new MofFactory(uriExtent);

            var mofElement = factory.create(null);
            var value = new Point(23, 24);
            mofElement.set("point", value);

            var retrieve = mofElement.get("point");
            Assert.That(retrieve is IObject, Is.True);
            var asObject = (IObject)retrieve!;
            Assert.That(asObject.get("X"), Is.EqualTo(23));
            Assert.That(asObject.get("Y"), Is.EqualTo(24));
        }

        [Test]
        public void TestChangeOfIdAfterBeingSetToExtent()
        {
            var uriExtent = new MofUriExtent(new InMemoryProvider(), "dm:///test", null);
            var factory = new MofFactory(uriExtent);

            // Sets the object
            var mofElement = factory.create(null);
            uriExtent.elements().add(mofElement);
            var mofElement2 = factory.create(null);
            uriExtent.elements().add(mofElement2);
            var asMofElement = mofElement as MofElement;
            Assert.That(asMofElement, Is.Not.Null);
            
            // First, check that everything is working of getting an object
            Assert.That(uriExtent.element(asMofElement!.Id!), Is.Not.Null);
            
            // Tries to remove first object, should succeed
            uriExtent.elements().remove(mofElement);
            Assert.That(uriExtent.elements().Count(), Is.EqualTo(1));
            
            // Changes id of second object 
            mofElement2.SetId("Test");
            var foundElement = uriExtent.element("Test");
            Assert.That(foundElement, Is.Not.Null);
            
            // Removes it. Should succeed
            uriExtent.elements().remove(mofElement2);
            Assert.That(uriExtent.elements().Count(), Is.EqualTo(0));
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