using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Json;
using NUnit.Framework;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DatenMeister.Tests.Json
{
    [TestFixture]
    public class DirectJsonTests
    {
        [Test]
        public void TestDirectJsonTests()
        {
             var element = InMemoryObject.CreateEmpty();
             var childElement = InMemoryObject.CreateEmpty().SetProperty("name", "child");
             var arrayElement1 = InMemoryObject.CreateEmpty().SetProperty("name", "array1");
             var arrayElement2 = InMemoryObject.CreateEmpty().SetProperty("name", "array2");
             
             element.set("child", childElement);
             element.set("array", new[] {arrayElement1, arrayElement2});

             var jsonText = DirectJsonConverter.ConvertToJsonWithDefaultParameter(element);
             
             Assert.That(jsonText.Contains("child"));
             Assert.That(jsonText.Contains("array1"));
             Assert.That(jsonText.Contains("array2"));

             var converter = JsonSerializer.Deserialize<MofObjectAsJson>(jsonText);
             Assert.That(converter, Is.Not.Null);
        }
        
        [Test]
        public void TestJsonDeconverter()
        {
            Assert.That(DirectJsonDeconverter.ConvertJsonValue(2), Is.EqualTo(2));
            Assert.That(DirectJsonDeconverter.ConvertJsonValue("ABC"), Is.EqualTo("ABC"));
            Assert.That(DirectJsonDeconverter.ConvertJsonValue(JsonDocument.Parse("true").RootElement), Is.True);
            Assert.That(DirectJsonDeconverter.ConvertJsonValue(JsonDocument.Parse("false").RootElement), Is.False);
            Assert.That(DirectJsonDeconverter.ConvertJsonValue(JsonDocument.Parse("2").RootElement), Is.EqualTo(2));
            Assert.That(DirectJsonDeconverter.ConvertJsonValue(JsonDocument.Parse("\"abc\"").RootElement), Is.EqualTo("abc"));
        }
    }
}