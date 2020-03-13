using System;
using System.IO;
using DatenMeister.Modules.PublicSettings;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules
{
    [TestFixture]
    public class PublicIntegrationSettings
    {
        [Test]
        public void TestNoFile()
        {
            var directory = Environment.CurrentDirectory;
            var filename = Path.Combine(directory, PublicSettingHandler.XmiFileName);

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }


            var settings = PublicSettingHandler.LoadSettings(directory);

            Assert.That(settings, Is.Null);
        }

        [Test]
        public void TestReadingXmiFile()
        {
            var directory = Environment.CurrentDirectory;
            var filename = Path.Combine(directory, PublicSettingHandler.XmiFileName);

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            File.WriteAllText(filename,
                @"<xmi><settings databasePath=""test""></settings></xmi>");

            var settings = PublicSettingHandler.LoadSettings(directory);

            Assert.That(settings, Is.Not.Null);
            Assert.That(settings.databasePath, Is.EqualTo("test"));
        }
    }
}