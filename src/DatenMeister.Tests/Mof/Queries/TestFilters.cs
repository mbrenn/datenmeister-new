using DatenMeister.EMOF.InMemory;
using NUnit.Framework;
using System.Linq;
using DatenMeister.Runtime.Functions.Queries;

namespace DatenMeister.Tests.Mof.Queries
{
    [TestFixture]
    public class TestFilters
    {
        [Test]
        public void TestMultiplePropertyFilter()
        {
            var property1 = new object();
            var property2 = new object();
            var properties = new[] { property1, property2 };

            var mofObject = new MofObject();
            mofObject.set(property1, "55130");
            mofObject.set(property2, "Mainz");

            var mofObject2 = new MofObject();
            mofObject2.set(property1, "65474");
            mofObject2.set(property2, "Bischofsheim");

            var mofObject3 = new MofObject();

            var mofExtent = new MofUriExtent("datenmeister:///");
            Assert.That(mofExtent.elements().add(mofObject), Is.True);
            Assert.That(mofExtent.elements().add(mofObject2), Is.True);
            Assert.That(mofExtent.elements().add(mofObject3), Is.True);

            var result = Filter.WhenOneOfThePropertyContains(
                mofExtent.elements(),
                properties,
                "Mai");

            Assert.That(result.size(), Is.EqualTo(1));
            Assert.That(result.ElementAt(0), Is.EqualTo(mofObject));

            result = Filter.WhenOneOfThePropertyContains(
                mofExtent.elements(),
                properties,
                "55130");

            Assert.That(result.size(), Is.EqualTo(1));
            Assert.That(result.ElementAt(0), Is.EqualTo(mofObject));

            result = Filter.WhenOneOfThePropertyContains(
                mofExtent.elements(),
                properties,
                "Bisch");

            Assert.That(result.size(), Is.EqualTo(1));
            Assert.That(result.ElementAt(0), Is.EqualTo(mofObject2));

            result = Filter.WhenOneOfThePropertyContains(
                mofExtent.elements(),
                properties,
                "xyz");

            Assert.That(result.size(), Is.EqualTo(0));

            result = Filter.WhenOneOfThePropertyContains(
                mofExtent.elements(),
                properties,
                "i");

            Assert.That(result.size(), Is.EqualTo(2));
            Assert.That(result.Contains(mofObject), Is.True);
            Assert.That(result.Contains(mofObject2), Is.True);
        }
    }
}
