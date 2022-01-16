using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime.Functions
{
    [TestFixture]
    public class QueryTests
    {
        [Test]
        public void TestPropertyAsReflectiveCollectionDelete()
        {
            var item = InMemoryObject.CreateEmpty();
            var child1 = InMemoryObject.CreateEmpty();
            var child2 = InMemoryObject.CreateEmpty();
            var child3 = InMemoryObject.CreateEmpty();
            var child4 = InMemoryObject.CreateEmpty();

            item.set("list1", new[] {child1, child2});
            item.set("list2", new[] {child3, child4});

            var reflectiveCollection = new PropertiesAsReflectiveCollection(item);
            var elements = reflectiveCollection.ToList();
            Assert.That(elements, Is.Not.Null);
            Assert.That(elements.Count, Is.EqualTo(4));
            Assert.That(elements.Contains(child1));
            Assert.That(elements.Contains(child2));
            Assert.That(elements.Contains(child3));
            Assert.That(elements.Contains(child4));

            reflectiveCollection.remove(elements.First(x => x!.Equals(child1)));

            // Checks that item1 is lost
            var newList = item.getOrDefault<IReflectiveCollection>("list1").ToList();
            Assert.That(newList.Count, Is.EqualTo(1));

            reflectiveCollection = new PropertiesAsReflectiveCollection(item);
            elements = reflectiveCollection.ToList();
            Assert.That(elements, Is.Not.Null);
            Assert.That(elements.Count, Is.EqualTo(3));
            Assert.That(elements.Contains(child2), Is.True);
            Assert.That(elements.Contains(child3), Is.True);
            Assert.That(elements.Contains(child4), Is.True);
            Assert.That(elements.Contains(child1), Is.False);
        }

        [Test]
        public void TestOrderByProperty()
        {
            var extent = CreateQueryTestExtent();
            var ordered = extent.elements().OrderElementsBy("age").ToList<IElement>();
            Assert.That(ordered.Count, Is.EqualTo(5));
            Assert.That(ordered[0].getOrDefault<int>("age"), Is.EqualTo(15));
            Assert.That(ordered[1].getOrDefault<int>("age"), Is.EqualTo(18));
            Assert.That(ordered[2].getOrDefault<int>("age"), Is.EqualTo(20));
            Assert.That(ordered[3].getOrDefault<int>("age"), Is.EqualTo(20));
            Assert.That(ordered[4].getOrDefault<int>("age"), Is.EqualTo(20));

            ordered = extent.elements().OrderElementsBy("iq").ToList<IElement>();
            Assert.That(ordered.Count, Is.EqualTo(5));
            Assert.That(ordered[0].getOrDefault<int>("iq"), Is.EqualTo(100));
            Assert.That(ordered[1].getOrDefault<int>("iq"), Is.EqualTo(105));
            Assert.That(ordered[2].getOrDefault<int>("iq"), Is.EqualTo(109));
            Assert.That(ordered[3].getOrDefault<int>("iq"), Is.EqualTo(110));
            Assert.That(ordered[4].getOrDefault<int>("iq"), Is.EqualTo(120));
        }

        [Test]
        public void TestOrderByOrdinal()
        {
            var extent = CreateQueryTestExtent(true);
            var ordered = extent.elements().OrderElementsBy("iq").ToList<IElement>();

            ordered = extent.elements().OrderElementsBy("iq").ToList<IElement>();
            Assert.That(ordered.Count, Is.EqualTo(5));
            Assert.That(ordered[0].getOrDefault<int>("iq"), Is.EqualTo(95));
            Assert.That(ordered[1].getOrDefault<int>("iq"), Is.EqualTo(100));
            Assert.That(ordered[2].getOrDefault<int>("iq"), Is.EqualTo(109));
            Assert.That(ordered[3].getOrDefault<int>("iq"), Is.EqualTo(110));
            Assert.That(ordered[4].getOrDefault<int>("iq"), Is.EqualTo(120));
        }

        [Test]
        public void TestOrderByProperties()
        {
            var extent = CreateQueryTestExtent();
            var ordered = extent.elements().OrderElementsBy(
                new[] {"age", "iq"}).ToList<IElement>();
            Assert.That(ordered.Count, Is.EqualTo(5));
            Assert.That(ordered[0].getOrDefault<int>("age"), Is.EqualTo(15));
            Assert.That(ordered[1].getOrDefault<int>("age"), Is.EqualTo(18));
            Assert.That(ordered[2].getOrDefault<int>("age"), Is.EqualTo(20));
            Assert.That(ordered[3].getOrDefault<int>("age"), Is.EqualTo(20));
            Assert.That(ordered[4].getOrDefault<int>("age"), Is.EqualTo(20));
            Assert.That(ordered[0].getOrDefault<int>("iq"), Is.EqualTo(120));
            Assert.That(ordered[1].getOrDefault<int>("iq"), Is.EqualTo(110));
            Assert.That(ordered[2].getOrDefault<int>("iq"), Is.EqualTo(100));
            Assert.That(ordered[3].getOrDefault<int>("iq"), Is.EqualTo(105));
            Assert.That(ordered[4].getOrDefault<int>("iq"), Is.EqualTo(109));
        }

        [Test]
        public void TestOrderByPropertiesBackwards()
        {
            var extent = CreateQueryTestExtent();
            var ordered = extent.elements().OrderElementsBy(
                new[] {"age", "!iq"}).ToList<IElement>();
            Assert.That(ordered.Count, Is.EqualTo(5));
            Assert.That(ordered[0].getOrDefault<int>("age"), Is.EqualTo(15));
            Assert.That(ordered[1].getOrDefault<int>("age"), Is.EqualTo(18));
            Assert.That(ordered[2].getOrDefault<int>("age"), Is.EqualTo(20));
            Assert.That(ordered[3].getOrDefault<int>("age"), Is.EqualTo(20));
            Assert.That(ordered[4].getOrDefault<int>("age"), Is.EqualTo(20));
            Assert.That(ordered[0].getOrDefault<int>("iq"), Is.EqualTo(120));
            Assert.That(ordered[1].getOrDefault<int>("iq"), Is.EqualTo(110));
            Assert.That(ordered[2].getOrDefault<int>("iq"), Is.EqualTo(109));
            Assert.That(ordered[3].getOrDefault<int>("iq"), Is.EqualTo(105));
            Assert.That(ordered[4].getOrDefault<int>("iq"), Is.EqualTo(100));
        }

        private static IUriExtent CreateQueryTestExtent(bool ordinal = false)
        {
            var memoryProvider = new InMemoryProvider();
            var extent = new MofUriExtent(memoryProvider, "dm:///test", null);
            var factory = new MofFactory(extent);

            extent.elements().add(
                factory.create(null)
                    .SetProperties(new Dictionary<string, object> {["name"] = "person1", ["age"] = 18, ["iq"] = 110}));
            extent.elements().add(
                factory.create(null)
                    .SetProperties(new Dictionary<string, object> {["name"] = "person2", ["age"] = 15, ["iq"] = 120}));
            extent.elements().add(
                factory.create(null)
                    .SetProperties(new Dictionary<string, object>
                        {["name"] = "person3", ["age"] = 20, ["iq"] = ordinal ? 95 : 105}));
            extent.elements().add(
                factory.create(null)
                    .SetProperties(new Dictionary<string, object> {["name"] = "person4", ["age"] = 20, ["iq"] = 100}));
            extent.elements().add(
                factory.create(null)
                    .SetProperties(new Dictionary<string, object> {["name"] = "person5", ["age"] = 20, ["iq"] = 109}));

            return extent;
        }
    }
}