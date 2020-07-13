using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
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
            
            item.set("list1", new []{child1, child2});
            item.set("list2", new []{child3, child4});
            
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
    }
}