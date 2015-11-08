using DatenMeister.EMOF.InMemory;
using NUnit.Framework;

namespace DatenMeister.Tests.Mof.Core
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für MofObjectTests
    /// </summary>
    [TestFixture]
    public class MofObjectTests
    {
        [Test]
        public void TestInMemoryMofObject()
        {
            var property1 = new object();
            var property2 = new object();

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
    }
}
