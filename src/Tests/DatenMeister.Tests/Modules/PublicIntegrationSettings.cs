using System;
using System.IO;
using System.Linq;
using DatenMeister.BootStrap.PublicSettings;
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

            var settings = PublicSettingHandler.LoadSettingsFromDirectory(directory, out var extent);
            
            Assert.That(extent, Is.Null);
            Assert.That(settings, Is.Null);
        }

        [Test]
        public void TestReadingXmiFile()
        {
            var directory = Path.GetTempPath();
            var filename = Path.Combine(directory, PublicSettingHandler.XmiFileName);

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            File.WriteAllText(filename,
                @"<xmi><settings databasePath=""test""></settings></xmi>");

            var settings = PublicSettingHandler.LoadSettingsFromDirectory(directory, out var extent);

            Assert.That(settings, Is.Not.Null);
            Assert.That(extent, Is.Not.Null);
            Assert.That(settings!.databasePath, Is.EqualTo("test"));
        }

        [Test]
        public void TestReadingXmiFileWithEnvironment()
        {
            Environment.SetEnvironmentVariable("VARIABLE", "Test");
            var directory = Path.GetTempPath();
            var filename = Path.Combine(directory, PublicSettingHandler.XmiFileName);

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            File.WriteAllText(filename,
                @"<xmi><settings databasePath=""%VARIABLE%\test""></settings></xmi>");

            var settings = PublicSettingHandler.LoadSettingsFromDirectory(directory, out _);

            Assert.That(settings, Is.Not.Null);
            Assert.That(settings!.databasePath, Is.EqualTo("Test\\test"));
        }

        [Test]
        public void TestReadingEnvironmentVariables()
        {
            var directory = Path.GetTempPath();
            var filename = Path.Combine(directory, PublicSettingHandler.XmiFileName);

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            File.WriteAllText(filename,
                @"<xmi><settings databasePath=""%USERNAME%\test"">
  <environmentVariable key=""key"" value=""value"" />
  <environmentVariable key=""key2"" value=""value2"" />
</settings></xmi>");

            var settings = PublicSettingHandler.LoadSettingsFromDirectory(directory, out _);

            Assert.That(settings, Is.Not.Null);
            Assert.That(settings!.environmentVariable.Any(x => x.key == "key"), Is.True);
            Assert.That(settings.environmentVariable.Any(x => x.key == "key2"), Is.True);

            Assert.That(Environment.GetEnvironmentVariable("key"), Is.EqualTo("value"));
        }
    }
}