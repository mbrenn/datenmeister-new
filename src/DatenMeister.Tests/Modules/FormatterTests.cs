using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Modules.Formatter;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules
{
    [TestFixture]
    public class FormatterTests
    {
        [Test]
        public void TestStringFormatter()
        {
            var value = InMemoryObject.CreateEmpty();
            value.set("id", "abc");
            value.set("title", "def");

            var formatter = new StringFormatter();
            Assert.That(formatter.Format(value, "fksdal"), Is.EqualTo("fksdal"));
            Assert.That(formatter.Format(value, "{{id}}"), Is.EqualTo("abc"));
            Assert.That(formatter.Format(value, "A{{id}}B"), Is.EqualTo("AabcB"));
            Assert.That(formatter.Format(value, "{{id}} - {{title}}"), Is.EqualTo("abc - def"));
            Assert.That(formatter.Format(value, "{{id}} - {{title"), Is.EqualTo("abc - {{title"));
        }
    }
}