using System.Collections.Generic;
using DatenMeister.Core.EMOF.Implementation;
using NUnit.Framework;

namespace DatenMeister.Tests.Core
{
    [TestFixture]
    public class MofReflectiveCollectionTests
    {
        [Test]
        public void TestDeletionEvent()
        {
            var list = new List<object>
            {
                "abc",
                "def",
                "ghi"
            };
            var z = 0;
            var deleted = new List<object>();

            var temporaryReflectionCollection = new TemporaryReflectiveCollection(list);
            temporaryReflectionCollection.OnDelete += (x, y) =>
            {
                z++;
                deleted.Add(y.DeleteObject);
            };
            
            Assert.That(z, Is.EqualTo(0));

            temporaryReflectionCollection.remove("abc");
            Assert.That(z, Is.EqualTo(1));
            Assert.That(deleted.Contains("abc"));
            
            temporaryReflectionCollection.clear();
            Assert.That(z, Is.EqualTo(3));
            Assert.That(deleted.Contains("def"));
        }
    }
}