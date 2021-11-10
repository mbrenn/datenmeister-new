using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using NUnit.Framework;

namespace DatenMeister.Tests.Core
{
    [TestFixture]
    public class HelperTests
    {
        /// <summary>
        /// Tests the deletion of the items
        /// </summary>
        [Test]
        public void TestDeletion()
        {
            var extent = new MofUriExtent(new InMemoryProvider());

            var element1 = MofFactory.Create(extent, null);
            var element2 = MofFactory.Create(extent, null);

            extent.elements().add(element1);
            extent.elements().add(element2);

            Assert.That(extent.elements().Count(), Is.EqualTo(2));
            Assert.That(ObjectHelper.DeleteObject(element1), Is.True);
            
            Assert.That(extent.elements().Count(), Is.EqualTo(1));
            
            Assert.That(extent.elements().Any(x=> element1.Equals(x)), Is.False);
            Assert.That(extent.elements().Any(x=> element2.Equals(x)), Is.True);
            
            Assert.That(ObjectHelper.DeleteObject(element1), Is.False);
            Assert.That(extent.elements().Count(), Is.EqualTo(1));
            
            Assert.That(ObjectHelper.DeleteObject(element2), Is.True);
            Assert.That(extent.elements().Count(), Is.EqualTo(0));
        }
        
        /// <summary>
        /// Tests the deletion of the items
        /// </summary>
        [Test]
        public void TestDeletionOfProperty()
        {
            var extent = new MofUriExtent(new InMemoryProvider());

            var element1 = MofFactory.Create(extent, null);
            var element2 = MofFactory.Create(extent, null);
            var element3 = MofFactory.Create(extent, null);

            extent.elements().add(element1);
            extent.elements().add(element2);
            
            element1.set("child", element3);
            
            Assert.That(extent.elements().Count(), Is.EqualTo(2));
            Assert.That(element1.getOrDefault<IObject>("child")?.Equals(element3), Is.True);
            
            Assert.That(ObjectHelper.DeleteObject(element3), Is.True);
            
            Assert.That(extent.elements().Count(), Is.EqualTo(2));
            Assert.That(element1.isSet("child"), Is.False);
            
            Assert.That(extent.elements().Any(x=> element1.Equals(x)), Is.True);
            Assert.That(extent.elements().Any(x=> element2.Equals(x)), Is.True);
            
            
            Assert.That(ObjectHelper.DeleteObject(element3), Is.False);
        }
        
        /// <summary>
        /// Tests the deletion of the items
        /// </summary>
        [Test]
        public void TestDeletionOfCollection()
        {
            var extent = new MofUriExtent(new InMemoryProvider());

            var element1 = MofFactory.Create(extent, null);
            var element2 = MofFactory.Create(extent, null);
            var element3 = MofFactory.Create(extent, null);
            var element4 = MofFactory.Create(extent, null);

            extent.elements().add(element1);
            extent.elements().add(element2);

            element1.set("children", new[] { element3, element4 });
            
            Assert.That(extent.elements().Count(), Is.EqualTo(2));
            var collection = element1.getOrDefault<IReflectiveCollection>("children");
            Assert.That(collection, Is.Not.Null);
            Assert.That(collection.Any(x=> element2.Equals(x)), Is.False);
            Assert.That(collection.Any(x=> element3.Equals(x)), Is.True);
            Assert.That(collection.Any(x=> element4.Equals(x)), Is.True);
            
            Assert.That(ObjectHelper.DeleteObject(element3), Is.True);
            
            collection = element1.getOrDefault<IReflectiveCollection>("children");
            Assert.That(collection, Is.Not.Null);
            Assert.That(collection.Any(x=> element2.Equals(x)), Is.False);
            Assert.That(collection.Any(x=> element3.Equals(x)), Is.False);
            Assert.That(collection.Any(x=> element4.Equals(x)), Is.True);
        }
    }
}