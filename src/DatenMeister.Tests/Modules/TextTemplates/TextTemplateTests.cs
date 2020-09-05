using System.Collections.Generic;
using DatenMeister.Modules.TextTemplates;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.TextTemplates
{
    [TestFixture]
    public class TextTemplateTests
    {
        [Test]
        public void TestTrivial()
        {
            var value = InMemoryObject.CreateEmpty();
            value.set("name", "Martin");

            var template = "Hallo.";
            Assert.That(
                TextTemplateEngine.Parse(
                    template,
                    new Dictionary<string, object> {["i"] = value}),
                Is.EqualTo(template));

            template = "Hallo {{ i.name }}.";
            Assert.That(
                TextTemplateEngine.Parse(
                    template,
                    new Dictionary<string, object> {["i"] = value}),
                Is.EqualTo("Hallo Martin."));

        }

        [Test]
        public void TestNull()
        {
            var template = "Hallo.";
            Assert.That(
                TextTemplateEngine.Parse(
                    template,
                    new Dictionary<string, object> {["i"] = InMemoryObject.CreateEmpty()}),
                Is.EqualTo(template));

            template = "Hallo {{i.name}}";

            Assert.That(
                TextTemplateEngine.Parse( template,
                    new Dictionary<string, object> {["i"] = InMemoryObject.CreateEmpty()}),
                Is.EqualTo("Hallo "));
                
            template = "Hallo {{i.department.name}}";

            Assert.That(
                TextTemplateEngine.Parse( template,
                    new Dictionary<string, object> {["i"] = InMemoryObject.CreateEmpty()}),
                Is.EqualTo("Hallo "));
        }

        [Test]
        public void TestNested()
        {
            var value = InMemoryObject.CreateEmpty();
            value.set("name", "Martin");


            var value2 = InMemoryObject.CreateEmpty();
            value2.set("person", value);
            value2.set("department", "developer");


            var template = "Hallo {{i.person.name}} from {{i.department}}.";
            Assert.That(
                TextTemplateEngine.Parse(
                    template,
                    new Dictionary<string, object> {["i"] = value2}),
                Is.EqualTo("Hallo Martin from developer."));
        }

        [Test]
        public void TestSetting()
        {
            var value = InMemoryObject.CreateEmpty();

            var template = "Hallo {{i.name = \"abc\"; i.name}}.";
            Assert.That(
                TextTemplateEngine.Parse(
                    template,
                    new Dictionary<string, object> {["i"] = value}),
                Is.EqualTo("Hallo abc."));

            Assert.That(value.getOrDefault<string>("name"), Is.EqualTo("abc"));
        }
    }
}