using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Json;
using Newtonsoft.Json;
using NUnit.Framework;
using System;

namespace DatenMeister.Tests.Json
{
    [TestFixture]
    public class MofJsonTests
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

            var jsonText = MofJsonConverter.ConvertToJsonWithDefaultParameter(element);
             
            Assert.That(jsonText.Contains("child"));
            Assert.That(jsonText.Contains("array1"));
            Assert.That(jsonText.Contains("array2"));

            var converter = JsonConvert.DeserializeObject(jsonText);
            Assert.That(converter, Is.Not.Null);
        }

        [Test]
        public void TestJsonRoundTripForSimpleProperties()
        {
            var element = InMemoryObject.CreateEmpty()
                .SetProperty("name", "Brenn")
                .SetProperty("prename", "Martin")
                .SetProperty("location", "Germany")
                .SetProperty("birthday", new DateTime(1981, 11, 16))
                .SetProperty("legs", 2)
                .SetProperty("coffeePerDay", 2.8);

            var jsonText = MofJsonConverter.ConvertToJsonWithDefaultParameter(element);

            var asJsonObject = JsonConvert.DeserializeObject<MofObjectAsJson>(jsonText);

            var deconverter = new MofJsonDeconverter().ConvertToObject(asJsonObject);

            Assert.That(deconverter, Is.Not.Null);
            Assert.That(deconverter.isSet("name"), Is.True);
            Assert.That(deconverter.get<string>("name"), Is.EqualTo("Brenn"));
            Assert.That(deconverter.get<string>("prename"), Is.EqualTo("Martin"));
            Assert.That(deconverter.get<string>("location"), Is.EqualTo("Germany"));
            Assert.That(deconverter.get<DateTime>("birthday"), Is.EqualTo(new DateTime(1981, 11, 16)));
            Assert.That(deconverter.get<int>("legs"), Is.EqualTo(2));
            Assert.That(deconverter.get<double>("coffeePerDay"), Is.EqualTo(2.8).Within(0.01));
        }
    }
}