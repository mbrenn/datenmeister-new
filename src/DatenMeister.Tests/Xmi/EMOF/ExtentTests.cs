using System.Linq;
using System.Xml.Linq;
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
            var mofObject = new XmlObject(new XElement("item"));
            mofObject.set("test", "testvalue");

            Assert.That(mofObject.isSet("none"), Is.False);
            Assert.That(mofObject.isSet("test"), Is.True);
            Assert.That(mofObject.get("test"), Is.EqualTo("testvalue"));
            mofObject.unset("test");
            Assert.That(mofObject.isSet("test"), Is.False);
        }

        [Test]
        public void TestXmlMofReflectiveSequence()
        {
            var mofObject1 = new XmlObject(new XElement("item"));
            var mofObject2 = new XmlObject(new XElement("item"));

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
        }
    }
}