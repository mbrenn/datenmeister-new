using System;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.XMI.EMOF;
using NUnit.Framework;

namespace DatenMeister.Tests.Xmi.EMOF
{
    [TestFixture]
    public class ExtentTests
    {
        [Test]
        public void TestXmlMofObject()
        {
            var mofObject = new XmlElement(new XElement("item"));
            mofObject.set("test", "testvalue");
            Assert.That(mofObject.getMetaClass(), Is.Null);
            Assert.That(mofObject.metaclass, Is.Null);
            Assert.That(mofObject.container(), Is.Null);

            Assert.That(mofObject.isSet("none"), Is.False);
            Assert.That(mofObject.isSet("test"), Is.True);
            Assert.That(mofObject.get("test"), Is.EqualTo("testvalue"));
            mofObject.unset("test");
            Assert.That(mofObject.isSet("test"), Is.False);
        }

        [Test]
        public void TestXmlMofReflectiveSequence()
        {
            var mofObject1 = new XmlElement(new XElement("item"));
            var mofObject2 = new XmlElement(new XElement("item"));
            var mofObject3 = new XmlElement(new XElement("item"));
            var mofObject4 = new XmlElement(new XElement("item"));

            var mofReflectiveSequence = new XmlReflectiveSequence(new XElement("items"));
            Assert.That(mofReflectiveSequence.size(), Is.EqualTo(0));
            Assert.That(mofReflectiveSequence.ToArray().Count, Is.EqualTo(0));

            mofReflectiveSequence.add(mofObject1);
            mofReflectiveSequence.add(mofObject2);
            Assert.That(mofReflectiveSequence.size(), Is.EqualTo(2));
            Assert.That(mofReflectiveSequence.ToArray().Count, Is.EqualTo(2));

            Assert.That(mofReflectiveSequence.get(0).Equals(mofObject1), Is.True);
            Assert.That(mofReflectiveSequence.get(1), Is.EqualTo(mofObject2));

            mofReflectiveSequence.remove(0);
            Assert.That(mofReflectiveSequence.get(0), Is.EqualTo(mofObject2));
            Assert.That(mofReflectiveSequence.size(), Is.EqualTo(1));
            Assert.That(mofReflectiveSequence.ToArray().Count, Is.EqualTo(1));

            mofReflectiveSequence.remove(mofObject2);
            Assert.That(mofReflectiveSequence.ToArray().Count, Is.EqualTo(0));
            
            mofReflectiveSequence.add(mofObject1);
            mofReflectiveSequence.add(mofObject2);

            var otherMofReflectiveSequence = new XmlReflectiveSequence(new XElement("items"));
            otherMofReflectiveSequence.addAll(mofReflectiveSequence);
            Assert.That(otherMofReflectiveSequence.size(), Is.EqualTo(2));
            Assert.That(otherMofReflectiveSequence.ToArray().Count, Is.EqualTo(2));

            otherMofReflectiveSequence.clear();
            Assert.That(otherMofReflectiveSequence.size(), Is.EqualTo(0));
            Assert.That(otherMofReflectiveSequence.ToArray().Count, Is.EqualTo(0));


            otherMofReflectiveSequence = new XmlReflectiveSequence(new XElement("items"));
            otherMofReflectiveSequence.add(0, mofObject1);
            otherMofReflectiveSequence.add(0, mofObject2);
            otherMofReflectiveSequence.add(1, mofObject3);

            Assert.That(otherMofReflectiveSequence.size(), Is.EqualTo(3));
            Assert.That(otherMofReflectiveSequence.ElementAt(0), Is.EqualTo(mofObject2));
            Assert.That(otherMofReflectiveSequence.ElementAt(1), Is.EqualTo(mofObject3));
            Assert.That(otherMofReflectiveSequence.ElementAt(2), Is.EqualTo(mofObject1));

            otherMofReflectiveSequence.set(1, mofObject4);
            Assert.That(otherMofReflectiveSequence.size(), Is.EqualTo(3));
            Assert.That(otherMofReflectiveSequence.ElementAt(0), Is.EqualTo(mofObject2));
            Assert.That(otherMofReflectiveSequence.ElementAt(1), Is.EqualTo(mofObject4));
            Assert.That(otherMofReflectiveSequence.ElementAt(2), Is.EqualTo(mofObject1));

        }

        [Test]
        public void TestXmlExtent()
        {
            var mofObject1 = new XmlElement(new XElement("item"));
            var mofObject2 = new XmlElement(new XElement("item"));
            var mofObject3 = new XmlElement(new XElement("item"));

            var extent = new XmlUriExtent("dm:///test/");
            Assert.That(extent.contextURI(), Is.EqualTo("dm:///test/"));

            // At the moment, it is not defined whether to contain or not contain. Just to increase coverage
            Assert.That(extent.useContainment(), Is.True.Or.False);

            Assert.That(extent.elements(), Is.Not.Null);
            Assert.That(extent.elements().size(), Is.EqualTo(0));

            extent.elements().add(mofObject1);
            extent.elements().add(mofObject2);
            Assert.That(extent.elements().size(), Is.EqualTo(2));

            mofObject1.set("name", "Martin");
            mofObject1.set("lastname", "Brenn");
            
            mofObject2.set("name", "Another");
            mofObject2.set("lastname", "Brenner");

            Assert.That(mofObject1.get("name").ToString(), Is.EqualTo("Martin"));
            Assert.That(mofObject2.get("name").ToString(), Is.EqualTo("Another"));

            var uri1 = extent.uri(mofObject1);
            var uri2 = extent.uri(mofObject2);
            var id1 = ((IHasId) mofObject1).Id.ToString();
            var id2 = ((IHasId) mofObject2).Id.ToString();

            Assert.That(id1, Is.Not.Null.Or.Empty);
            Assert.That(id2, Is.Not.Null.Or.Empty);
            Assert.That(id1 != id2, Is.True);

            Assert.That(uri1 != uri2, Is.True);
            Assert.That(uri1.StartsWith("dm:///test/"), Is.True);
            Assert.That(uri2.StartsWith("dm:///test/"), Is.True);

            Assert.That(uri1.EndsWith(id1), Is.True);
            Assert.That(uri2.EndsWith(id2), Is.True);

            var found = extent.element(uri1);
            Assert.That(found, Is.Not.Null);
            Assert.That(found, Is.EqualTo(mofObject1));

            var uri3 = "dm:///test/#abc";
            Assert.That(extent.element(uri3), Is.Null);

            var uri4 = "dm:///test/";
            Assert.That(extent.element(uri4), Is.Null);

            var uri5 = "#abc";
            Assert.That(extent.element(uri5), Is.Null);

            var uri6 = "dm:///anothertest/#" + id1;
            Assert.That(extent.element(uri6), Is.Null);

            extent.elements().remove(mofObject1);
            Assert.That(extent.elements().size(), Is.EqualTo(1));

            extent.elements().remove(mofObject3);
            Assert.That(extent.elements().size(), Is.EqualTo(1));

            var mofElement = new MofElement();
            Assert.Throws<ArgumentNullException>(() => extent.uri(mofElement));

            Assert.Throws<InvalidOperationException>(() => extent.elements().add(mofElement));

        }
    }
}