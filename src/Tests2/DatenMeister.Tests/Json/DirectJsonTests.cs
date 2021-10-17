using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Json;
using Newtonsoft.Json;
using NUnit.Framework;

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

             var converter = JsonConvert.DeserializeObject(jsonText);
             Assert.That(converter, Is.Not.Null);
        }
    }
}