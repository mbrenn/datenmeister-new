using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Provider.InMemory;
using NUnit.Framework;

namespace DatenMeister.Tests.Mof.Queries
{
    [TestFixture]
    public class TestFilters
    {
        private static string property1 = "Prop1";
        private static string property2 = "Prop2";

        [Test]
        public void TestUnion()
        {
            var first = new[]
                { InMemoryObject.CreateEmpty(), InMemoryObject.CreateEmpty(), InMemoryObject.CreateEmpty() };
            var second = new[]
                { InMemoryObject.CreateEmpty(), InMemoryObject.CreateEmpty(), InMemoryObject.CreateEmpty() };

            var firstReflection = new TemporaryReflectiveCollection(first);
            var secondReflection = new TemporaryReflectiveCollection(second);

            Assert.That(firstReflection.size(), Is.EqualTo(3));
            Assert.That(secondReflection.size(), Is.EqualTo(3));

            var union = firstReflection.Union(secondReflection);
            Assert.That(union.size(), Is.EqualTo(6));

            var list = union.ToList();
            Assert.That(list[0], Is.EqualTo(first[0]));
            Assert.That(list[1], Is.EqualTo(first[1]));
            Assert.That(list[2], Is.EqualTo(first[2]));
            Assert.That(list[3], Is.EqualTo(second[0]));
            Assert.That(list[4], Is.EqualTo(second[1]));
            Assert.That(list[5], Is.EqualTo(second[2]));


            union.clear();
            Assert.That(union.size(), Is.EqualTo(0));
            Assert.That(firstReflection.size(), Is.EqualTo(0));
            Assert.That(secondReflection.size(), Is.EqualTo(0));
        }

        [Test]
        public void TestMultiplePropertyFilter()
        {
            var mofExtent = new MofUriExtent(new InMemoryProvider(), "dm:///");
            var properties = new[] { property1, property2 };
            var factory = new MofFactory(mofExtent);

            var mofObject = factory.create(null);
            mofObject.set(property1, "55130");
            mofObject.set(property2, "Mainz");

            var mofObject2 = factory.create(null);
            mofObject2.set(property1, "65474");
            mofObject2.set(property2, "Bischofsheim");

            var mofObject3 = factory.create(null);

            Assert.That(mofExtent.elements().add(mofObject), Is.True);
            Assert.That(mofExtent.elements().add(mofObject2), Is.True);
            Assert.That(mofExtent.elements().add(mofObject3), Is.True);

            var result = mofExtent.elements().WhenOneOfThePropertyContains(properties,
                "Mai");
            var foundElement = result.ElementAt(0);

            Assert.That(result.size(), Is.EqualTo(1));
            Assert.That(foundElement, Is.EqualTo(mofObject));

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

        [Test]
        public void TestTakeFirst()
        {
            var mofExtent = new MofUriExtent(new InMemoryProvider(), "dm:///");
            var factory = new MofFactory(mofExtent);
            
            var list = new List<IObject>();
            for (var n = 0; n < 1000; n++)
            {
                var item = factory.create(null);
                item.set("name", "test");
                list.Add(item);
            }

            var collection = new TemporaryReflectiveCollection(list);
            
            Assert.That(collection.size(), Is.EqualTo(1000));
            Assert.That(collection.TakeFirst(100).size(), Is.EqualTo(100));
            Assert.That(collection.TakeFirst(2000).size(), Is.EqualTo(1000));
            Assert.That(collection.TakeFirst(100).Count(), Is.EqualTo(100));
            Assert.That(collection.TakeFirst(2000).Count(), Is.EqualTo(1000));
        }
    }
}
