﻿using System.Linq;
using DatenMeister.Core.EMOF.InMemory;
using DatenMeister.Core.EMOF.Interface.Reflection;
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
            var mofObject = new MofObject();
            Assert.That(mofObject.isSet(property1), Is.False);
            mofObject.set(property1, "Test");
            mofObject.set(property2, property1);

            Assert.That(mofObject.isSet(property1),Is.True);
            Assert.That(mofObject.isSet(property2),Is.True);

            Assert.That(mofObject.get(property1).ToString(), Is.EqualTo("Test"));
            Assert.That(mofObject.get(property2), Is.EqualTo(property1));
        }

        [Test]
        public void TestStoreAndFindObject()
        {
            var mofElement = new MofElement();
            var otherMofElement = new MofElement();
            var mofInstance = new MofUriExtent("datenmeister:///test");
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
            var uriExtent = new MofUriExtent("dm:///test");
            var proxiedUriExtent = new ProxyUriExtent(uriExtent).ActivateObjectConversion();

            var mofElement = new MofElement();
            var otherMofElement = new MofElement();
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
            var uriExtent = new MofUriExtent("dm:///test");

            var mofElement = new MofElement();
            var otherMofElement = new MofElement();

            Assert.That(((IObjectKnowsExtent)mofElement).Extents.FirstOrDefault(), Is.Null);
            Assert.That(((IObjectKnowsExtent)otherMofElement).Extents.FirstOrDefault(), Is.Null);

            uriExtent.elements().add(mofElement);

            Assert.That(((IObjectKnowsExtent)mofElement).Extents.First(), Is.SameAs(uriExtent));
            Assert.That(((IObjectKnowsExtent)otherMofElement).Extents.FirstOrDefault(), Is.Null);

            uriExtent.elements().add(otherMofElement);

            Assert.That(((IObjectKnowsExtent)mofElement).Extents.First(), Is.SameAs(uriExtent));
            Assert.That(((IObjectKnowsExtent)otherMofElement).Extents.First(), Is.SameAs(uriExtent));
        }
    }
}
