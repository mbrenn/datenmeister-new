using System.Linq;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.Functions.Queries;
using NUnit.Framework;

namespace DatenMeister.Tests.Mof.Queries
{
    [TestFixture]
    public class TestFilters
    {
        private static string property1 = "Prop1";
        private static string property2 = "Prop2";

        [Test]
        public void TestMultiplePropertyFilter()
        {
            var properties = new[] { property1, property2 };

            var mofObject = new InMemoryObject();
            mofObject.set(property1, "55130");
            mofObject.set(property2, "Mainz");

            var mofObject2 = new InMemoryObject();
            mofObject2.set(property1, "65474");
            mofObject2.set(property2, "Bischofsheim");

            var mofObject3 = new InMemoryObject();

            var mofExtent = new InMemoryUriExtent("datenmeister:///");
            Assert.That(mofExtent.elements().add(mofObject), Is.True);
            Assert.That(mofExtent.elements().add(mofObject2), Is.True);
            Assert.That(mofExtent.elements().add(mofObject3), Is.True);

            var result = mofExtent.elements().WhenOneOfThePropertyContains(properties,
                "Mai");

            Assert.That(result.size(), Is.EqualTo(1));
            Assert.That(result.ElementAt(0), Is.EqualTo(mofObject));

            result = mofExtent.elements().WhenOneOfThePropertyContains(properties,
                "55130");

            Assert.That(result.size(), Is.EqualTo(1));
            Assert.That(result.ElementAt(0), Is.EqualTo(mofObject));

            result = mofExtent.elements().WhenOneOfThePropertyContains(properties,
                "Bisch");

            Assert.That(result.size(), Is.EqualTo(1));
            Assert.That(result.ElementAt(0), Is.EqualTo(mofObject2));

            result = mofExtent.elements().WhenOneOfThePropertyContains(properties,
                "xyz");

            Assert.That(result.size(), Is.EqualTo(0));

            result = mofExtent.elements().WhenOneOfThePropertyContains(properties,
                "i");

            Assert.That(result.size(), Is.EqualTo(2));
            Assert.That(result.Contains(mofObject), Is.True);
            Assert.That(result.Contains(mofObject2), Is.True);
        }
    }
}
