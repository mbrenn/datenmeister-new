using System.Text.Json;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Web.Json;
using NUnit.Framework;

namespace DatenMeister.Tests.Json;

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
        element.set("array", new[] { arrayElement1, arrayElement2 });

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
            .SetProperty("birthday", new DateTime(1981, 12, 20, 0, 0, 0))
            .SetProperty("legs", 2)
            .SetProperty("coffeePerDay", 2.8);

        var jsonText = MofJsonConverter.ConvertToJsonWithDefaultParameter(element);

        var asJsonObject = JsonSerializer.Deserialize<MofObjectAsJson>(jsonText);
        Assert.That(asJsonObject, Is.Not.Null);

        var deconverted = new DirectJsonDeconverter().ConvertToObject(asJsonObject!);
            
        Assert.That(deconverted, Is.Not.Null);
        Assert.That(deconverted!.isSet("name"), Is.True);
        Assert.That(deconverted.get<string>("name"), Is.EqualTo("Brenn"));
        Assert.That(deconverted.get<string>("prename"), Is.EqualTo("Martin"));
        Assert.That(deconverted.get<string>("location"), Is.EqualTo("Germany"));
        Assert.That(deconverted.get<DateTime>("birthday"),
            Is.EqualTo(new DateTime(1981, 12, 20, 0, 0, 0)));
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

        var deconverted = new DirectJsonDeconverter().ConvertToObject(asJsonObject!);
        Assert.That(deconverted, Is.Not.Null);
        Assert.That(deconverted!.getOrDefault<string>("name"), Is.EqualTo("parent"));

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

    [Test]
    public void TestMetaClass()
    {
        var element = InMemoryObject.CreateEmpty("dm:///meta1");
        var childElement = InMemoryObject.CreateEmpty("dm:///meta2").SetProperty("name", "child");
        element.set("name", "parent");
        element.set("child", childElement);

        var jsonText = MofJsonConverter.ConvertToJsonWithDefaultParameter(element);

        var asJsonObject = JsonSerializer.Deserialize<MofObjectAsJson>(jsonText);
        Assert.That(asJsonObject, Is.Not.Null);

        var deconverted = new DirectJsonDeconverter().ConvertToObject(asJsonObject!) as IElement;
        Assert.IsTrue(deconverted?.metaclass!.equals(new MofObjectShadow("dm:///meta1")));

        var child2 = deconverted.get<IElement>("child");
        Assert.That(child2, Is.Not.Null);
        Assert.IsTrue(child2.metaclass!.equals(new MofObjectShadow("dm:///meta2")));
    }

    [Test]
    public void TestReverseConversionWithReference()
    {
        var json = """
                   {
                     "id": "local_17",
                     "v": {
                       "name": "Collection",
                       "items": {
                         "0": {
                           "id": "local_15",
                           "v": {
                             "name": "Object 1",
                             "test": {
                               "r": "#local_16",
                               "id": "local_16"
                             }
                           }
                         },
                         "1": {
                           "id": "local_16",
                           "v": {
                             "name": "Object 2"
                           }
                         }
                       }
                     }
                   }                   
                   """;

        var asJsonObject = JsonSerializer.Deserialize<MofObjectAsJson>(json);
        var deconverted = new DirectJsonDeconverter().ConvertToObject(asJsonObject!) as IElement;
        Assert.That(deconverted, Is.Not.Null);
        Assert.That(deconverted!.getOrDefault<string>("name"), Is.EqualTo("Collection"));
        var items = deconverted.get<IReflectiveCollection>("items");
        Assert.That(items, Is.Not.Null);
        Assert.That(items.size(), Is.EqualTo(2));
        var item1 = items.OfType<IElement>().FirstOrDefault(x => x.getOrDefault<string>("name") == "Object 1");
        Assert.That(item1, Is.Not.Null);
        var item2 = items.OfType<IElement>().FirstOrDefault(x => x.getOrDefault<string>("name") == "Object 2");
        Assert.That(item2, Is.Not.Null);

        var referenceItem1 = item1.get<IElement>("test");
        Assert.That(referenceItem1, Is.Not.Null);

        // Checks that name of referenceItem1 is Object 2
        Assert.That(referenceItem1.getOrDefault<string>("name"), Is.EqualTo("Object 2"));

        // Assert that even the reference is the same to item2
        Assert.That(referenceItem1, Is.EqualTo(item2));

        // We check it also by converting the property of the element and then checking whether the reference's property also has changed

        item2!.set("name", "New Name");
        Assert.That(referenceItem1.getOrDefault<string>("name"), Is.EqualTo("New Name"));
    }
}