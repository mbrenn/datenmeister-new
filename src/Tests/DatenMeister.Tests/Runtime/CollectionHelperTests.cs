using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime
{
    [TestFixture]
    public class CollectionHelperTests
    {
        [Test]
        public void TestMovingOfElements()
        {
            var extent = new MofUriExtent(new InMemoryProvider(), "dm:///a", null);
            var factory = new MofFactory(extent);

            var list = factory.create(null);
            var child1 = factory.create(null);
            child1.set("name", "A");
            var child2 = factory.create(null);
            child2.set("name", "B");
            var child3 = factory.create(null);
            child3.set("name", "C");

            list.set("list", new[] {child1, child2, child3});

            var reflectiveSequence = list.get<IReflectiveSequence>("list");
            var propertyList = reflectiveSequence.OfType<IObject>().ToList();

            Assert.That(propertyList.Count, Is.EqualTo(3));
            Assert.That(propertyList[0], Is.EqualTo(child1));
            Assert.That(propertyList[1], Is.EqualTo(child2));
            Assert.That(propertyList[2], Is.EqualTo(child3));


            reflectiveSequence.MoveElementUp(propertyList[1]);

            reflectiveSequence = list.get<IReflectiveSequence>("list");
            propertyList = reflectiveSequence.OfType<IObject>().ToList();
            Assert.That(propertyList.Count, Is.EqualTo(3));
            Assert.That(propertyList[0], Is.EqualTo(child2));
            Assert.That(propertyList[1], Is.EqualTo(child1));
            Assert.That(propertyList[2], Is.EqualTo(child3));

            reflectiveSequence.MoveElementUp(propertyList[1]);
            reflectiveSequence = list.get<IReflectiveSequence>("list");
            propertyList = reflectiveSequence.OfType<IObject>().ToList();

            Assert.That(propertyList.Count, Is.EqualTo(3));
            Assert.That(propertyList[0], Is.EqualTo(child1));
            Assert.That(propertyList[1], Is.EqualTo(child2));
            Assert.That(propertyList[2], Is.EqualTo(child3));


            reflectiveSequence.MoveElementDown(propertyList[1]);

            reflectiveSequence = list.get<IReflectiveSequence>("list");
            propertyList = reflectiveSequence.OfType<IObject>().ToList();
            Assert.That(propertyList.Count, Is.EqualTo(3));
            Assert.That(propertyList[0], Is.EqualTo(child1));
            Assert.That(propertyList[1], Is.EqualTo(child3));
            Assert.That(propertyList[2], Is.EqualTo(child2));

            reflectiveSequence.MoveElementDown(propertyList[1]);
            reflectiveSequence = list.get<IReflectiveSequence>("list");
            propertyList = reflectiveSequence.OfType<IObject>().ToList();

            Assert.That(propertyList.Count, Is.EqualTo(3));
            Assert.That(propertyList[0], Is.EqualTo(child1));
            Assert.That(propertyList[1], Is.EqualTo(child2));
            Assert.That(propertyList[2], Is.EqualTo(child3));
        }
    }
}