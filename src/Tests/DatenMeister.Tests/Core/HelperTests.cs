using System;
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
            var extent = new MofUriExtent(new InMemoryProvider(), null);

            var element1 = MofFactory.Create(extent, null);
            var element2 = MofFactory.Create(extent, null);

            extent.elements().add(element1);
            extent.elements().add(element2);

            Assert.That(extent.elements().Count(), Is.EqualTo(2));
            Assert.That(ObjectHelper.DeleteObject(element1), Is.True);

            Assert.That(extent.elements().Count(), Is.EqualTo(1));

            Assert.That(extent.elements().Any(x => element1.Equals(x)), Is.False);
            Assert.That(extent.elements().Any(x => element2.Equals(x)), Is.True);

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
            var extent = new MofUriExtent(new InMemoryProvider(), null);

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

            Assert.That(extent.elements().Any(x => element1.Equals(x)), Is.True);
            Assert.That(extent.elements().Any(x => element2.Equals(x)), Is.True);


            Assert.That(ObjectHelper.DeleteObject(element3), Is.False);
        }

        /// <summary>
        /// Tests the deletion of the items
        /// </summary>
        [Test]
        public void TestDeletionOfCollection()
        {
            var extent = new MofUriExtent(new InMemoryProvider(), null);

            var element1 = MofFactory.Create(extent, null);
            var element2 = MofFactory.Create(extent, null);
            var element3 = MofFactory.Create(extent, null);
            var element4 = MofFactory.Create(extent, null);

            extent.elements().add(element1);
            extent.elements().add(element2);

            element1.set("children", new[] {element3, element4});

            Assert.That(extent.elements().Count(), Is.EqualTo(2));
            var collection = element1.getOrDefault<IReflectiveCollection>("children");
            Assert.That(collection, Is.Not.Null);
            Assert.That(collection.Any(x => element2.Equals(x)), Is.False);
            Assert.That(collection.Any(x => element3.Equals(x)), Is.True);
            Assert.That(collection.Any(x => element4.Equals(x)), Is.True);

            Assert.That(ObjectHelper.DeleteObject(element3), Is.True);

            collection = element1.getOrDefault<IReflectiveCollection>("children");
            Assert.That(collection, Is.Not.Null);
            Assert.That(collection.Any(x => element2.Equals(x)), Is.False);
            Assert.That(collection.Any(x => element3.Equals(x)), Is.False);
            Assert.That(collection.Any(x => element4.Equals(x)), Is.True);
        }

        [Test]
        public void TestDateTimeConversions()
        {
            var date1 = new DateTime(1980, 3, 10);
            var date2 = new DateTime(1980, 3, 10, 0,0,0, DateTimeKind.Local);
            var date3 = new DateTime(1980, 3, 10, 0,0,0, DateTimeKind.Utc);
            var date4 = new DateTime(1980, 3, 10,1,2,3);

            var text1 = DotNetHelper.AsString(date1);
            var text2 = DotNetHelper.AsString(date2);
            var text3 = DotNetHelper.AsString(date3);
            var text4 = DotNetHelper.AsString(date4);

            var newDate1 = DotNetHelper.AsDateTime(text1);
            var newDate2 = DotNetHelper.AsDateTime(text2);
            var newDate3 = DotNetHelper.AsDateTime(text3);
            var newDate4 = DotNetHelper.AsDateTime(text4);
            
            Assert.That(newDate1, Is.EqualTo(date1));
            Assert.That(newDate2, Is.EqualTo(date2));
            Assert.That(newDate3, Is.EqualTo(date3));
            Assert.That(newDate4, Is.EqualTo(date4));
        }
    }
}