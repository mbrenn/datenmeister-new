using System.Drawing;
using DatenMeister.Integration;
using DatenMeister.Provider.CSV;
using NUnit.Framework;

namespace DatenMeister.Tests.Integration
{
    [TestFixture]
    public class ScopeStorageTests
    {
        [Test]
        public void TestAddingAndGetting()
        {
            var scopeStorage = new ScopeStorage();
            
            var point = new Point(32, 23);
            scopeStorage.AddStorageItem(point);

            var found = scopeStorage.GetStorageItem<Point>();
            Assert.That(found, Is.Not.Null);
            Assert.That(found.X, Is.EqualTo(32));
            Assert.That(found.Y, Is.EqualTo(23));
            
            var settings = new CsvSettings();
            settings.MetaclassUri = "dm:///TEST";

            scopeStorage.AddStorageItem(settings);
            var result = scopeStorage.GetStorageItem<CsvSettings>();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(settings));

        }
    }
}