using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Json;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text.Json;
using DatenMeister.Core.EMOF.Interface.Common;

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

            var converter = JsonSerializer.Deserialize<MofObjectAsJson>(jsonText);
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

            var asJsonObject = JsonSerializer.Deserialize<MofObjectAsJson>(jsonText);

            var deconverted = DirectJsonDeconverter.ConvertToObject(asJsonObject);

            Assert.That(deconverted, Is.Not.Null);
            Assert.That(deconverted.isSet("name"), Is.True);
            Assert.That(deconverted.get<string>("name"), Is.EqualTo("Brenn"));
            Assert.That(deconverted.get<string>("prename"), Is.EqualTo("Martin"));
            Assert.That(deconverted.get<string>("location"), Is.EqualTo("Germany"));
            Assert.That(deconverted.get<DateTime>("birthday"), Is.EqualTo(new DateTime(1981, 11, 16)));
            Assert.That(deconverted.get<int>("legs"), Is.EqualTo(2));
            Assert.That(deconverted.get<double>("coffeePerDay"), Is.EqualTo(2.8).Within(0.01));
        }

        [Test]
        public void TestJsonRoundWithObjectAndArray()
        {
            var element = InMemoryObject.CreateEmpty();
            var childElement = InMemoryObject.CreateEmpty().SetProperty("name", "child");
            var arrayElement1 = InMemoryObject.CreateEmpty().SetProperty("name", "array1");
            var arrayElement2 = InMemoryObject.CreateEmpty().SetProperty("name", "array2");

            element.set("name", "parent");
            element.set("child", childElement);
            element.set("array", new[] { arrayElement1, arrayElement2 });

            var jsonText = MofJsonConverter.ConvertToJsonWithDefaultParameter(element);

            var asJsonObject = JsonSerializer.Deserialize<MofObjectAsJson>(jsonText);
            Assert.That(asJsonObject, Is.Not.Null);

            var deconverted = DirectJsonDeconverter.ConvertToObject(asJsonObject);
            Assert.That(deconverted, Is.Not.Null);
            Assert.That(deconverted.getOrDefault<string>("name"), Is.EqualTo("parent"));

            var child2 = deconverted.get<IElement>("child");
            Assert.That(child2, Is.Not.Null);
            Assert.That(child2.getOrDefault<string>("name"), Is.EqualTo("child"));

            var reflectiveCollection = deconverted.get<IReflectiveCollection>("array");
            Assert.That(reflectiveCollection, Is.Not.Null);
            Assert.That(reflectiveCollection.size(), Is.EqualTo(2));

            var arrayElements = reflectiveCollection.OfType<IElement>().ToList();
            Assert.That(arrayElements.Count, Is.EqualTo(2));
            
            Assert.That(arrayElements[0].getOrDefault<string>("name"), Is.EqualTo("array1"));
            Assert.That(arrayElements[1].getOrDefault<string>("name"), Is.EqualTo("array2"));
        }
    }
}