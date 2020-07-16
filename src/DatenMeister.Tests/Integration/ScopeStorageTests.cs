using System;
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
            scopeStorage.Add(point);

            var found = scopeStorage.Get<Point>();
            Assert.That(found, Is.Not.Null);
            Assert.That(found.X, Is.EqualTo(32));
            Assert.That(found.Y, Is.EqualTo(23));

            var settings = new CsvSettings
            {
                MetaclassUri = "dm:///TEST"
            };

            scopeStorage.Add(settings);
            var result = scopeStorage.Get<CsvSettings>();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(settings));

            Assert.Throws<InvalidOperationException>(() => scopeStorage.Get<IntegrationSettings>());
        }
    }
}