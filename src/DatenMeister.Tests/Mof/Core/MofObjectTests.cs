﻿using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.Proxies;
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
            var mofObject = new InMemoryObject();
            Assert.That(mofObject.IsPropertySet(property1), Is.False);
            mofObject.SetProperty(property1, "Test");
            mofObject.SetProperty(property2, property1);

            Assert.That(mofObject.IsPropertySet(property1),Is.True);
            Assert.That(mofObject.IsPropertySet(property2),Is.True);

            Assert.That(mofObject.GetProperty(property1).ToString(), Is.EqualTo("Test"));
            Assert.That(mofObject.GetProperty(property2), Is.EqualTo(property1));
        }

        [Test]
        public void TestSetInMemory()
        {
            var mofObject = new InMemoryObject();
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
            var mofElement = new InMemoryElement();
            var otherMofElement = new InMemoryElement();
            var mofInstance = new UriExtent(new InMemoryProvider(), "datenmeister:///test");
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
            Assert.That(found1, Is.SameAs(mofElement));
            Assert.That(found2, Is.SameAs(otherMofElement));
        }

        [Test]
        public void TestProxyForUriExtent()
        {
            var uriExtent = new UriExtent(new InMemoryProvider(), "dm:///test");
            var proxiedUriExtent = new ProxyUriExtent(uriExtent).ActivateObjectConversion();

            var mofElement = new InMemoryElement();
            var otherMofElement = new InMemoryElement();
            proxiedUriExtent.elements().add(mofElement);
            proxiedUriExtent.elements().add(otherMofElement);

            var returned = proxiedUriExtent.elements().ElementAt(0);
            Assert.That(returned, Is.TypeOf<ProxyMofElement>());

            var proxiedElement = returned as ProxyMofElement;
            Assert.That(proxiedElement, Is.Not.Null);
            Assert.That(proxiedElement.GetProxiedElement(), Is.TypeOf<InMemoryElement>());

            Assert.That(proxiedUriExtent.elements().remove(proxiedElement), Is.True);
            Assert.That(proxiedUriExtent.elements().size, Is.EqualTo(1));
            proxiedUriExtent.elements().clear();
            
            // Check, if the dereferencing of ProxyMofElements are working
            proxiedUriExtent.elements().add(proxiedElement);
            returned = proxiedUriExtent.elements().ElementAt(0);
            Assert.That(returned, Is.TypeOf<ProxyMofElement>());

            proxiedElement = returned as ProxyMofElement;
            Assert.That(proxiedElement.GetProxiedElement(), Is.TypeOf<InMemoryElement>());
        }

        [Test]
        public void TestKnowsExtent()
        {
            var uriExtent = new UriExtent(new InMemoryProvider(), "dm:///test");
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
            Assert.That(((IHasExtent)otherMofElement.get("Test")).Extent, Is.SameAs(uriExtent));
        }
    }
}
