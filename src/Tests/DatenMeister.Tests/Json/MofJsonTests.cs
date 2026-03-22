using System.Text.Json;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
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

        var jsonText = MofJsonConverter.ConvertToJsonStringWithDefaultParameter(element);

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

        var jsonText = MofJsonConverter.ConvertToJsonStringWithDefaultParameter(element);

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

        var jsonText = MofJsonConverter.ConvertToJsonStringWithDefaultParameter(element);

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

        var jsonText = MofJsonConverter.ConvertToJsonStringWithDefaultParameter(element);

        var asJsonObject = JsonSerializer.Deserialize<MofObjectAsJson>(jsonText);
        Assert.That(asJsonObject, Is.Not.Null);

        var deconverted = new DirectJsonDeconverter().ConvertToObject(asJsonObject!) as IElement;
        Assert.That(deconverted?.metaclass!.equals(new MofObjectShadow("dm:///meta1")), Is.True);

        var child2 = deconverted.get<IElement>("child");
        Assert.That(child2, Is.Not.Null);
        Assert.That(child2.metaclass!.equals(new MofObjectShadow("dm:///meta2")), Is.True);
    }

    [Test]
    public void TestReverseConversionWithReference()
    {
        var json = """
                   {
                     "id": "local_17",
                     "v": {
                       "name": [true, "Collection"],
                       "items": [true, {
                         "0": {
                           "id": "local_15",
                           "v": {
                             "name": [true, "Object 1"],
                             "test": [true, {
                               "r": "#local_16",
                               "id": "local_16"
                             }]
                           }
                         },
                         "1": {
                           "id": "local_16",
                           "v": {
                             "name": [true, "Object 2"]
                           }
                         }
                       }]
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

    [Test]
    public void TestQueryObjectExample()
    {
       var json = """
                  {
                     "m":{
                        "uri":"dm:///_internal/types/internal#b8333b8d-ac49-4a4e-a7f4-c3745e0a0237",
                        "workspace":"Types"
                     },
                     "id":"local_169",
                     "v":{
                        "_AddQueryInPackageAction":[
                           true,
                           "test"
                        ],
                        "targetPackageUri":[
                           true,
                           "dm:///_internal/forms/user"
                        ],
                        "targetPackageWorkspace":[
                           true,
                           "Management"
                        ],
                        "query":[
                           true,
                           {
                              "m":{
                                 "uri":"dm:///_internal/types/internal#DatenMeister.Models.DataViews.QueryStatement",
                                 "workspace":"Types"
                              },
                              "id":"local_63",
                              "v":{
                                 "nodes":[
                                    true,
                                    {
                                       "0":{
                                          "m":{
                                             "uri":"dm:///_internal/types/internal#DatenMeister.Models.DataViews.DynamicSourceNode",
                                             "workspace":"Types"
                                          },
                                          "id":"local_64",
                                          "v":{
                                             "nodeName":[
                                                true,
                                                "input"
                                             ],
                                             "name":[
                                                true,
                                                "Dynamic source input"
                                             ]
                                          }
                                       },
                                       "1":{
                                          "m":{
                                             "uri":"dm:///_internal/types/internal#d705b34b-369f-4b44-9a00-013e1daa759f",
                                             "workspace":"Types"
                                          },
                                          "id":"local_66",
                                          "v":{
                                             "input":[
                                                true,
                                                {
                                                   "r":"#local_64",
                                                   "id":"local_64"
                                                }
                                             ],
                                             "amount":[
                                                true,
                                                101
                                             ],
                                             "name":[
                                                true,
                                                "Limit to 101 elements"
                                             ]
                                          }
                                       }
                                    }
                                 ],
                                 "resultNode":[
                                    true,
                                    {
                                       "r":"#local_66",
                                       "id":"local_66"
                                    }
                                 ],
                                 "name":[
                                    true,
                                    "test"
                                 ]
                              }
                           }
                        ]
                     }
                  }
                  """;

        var asJsonObject = JsonSerializer.Deserialize<MofObjectAsJson>(json);
        var deconverted = new DirectJsonDeconverter().ConvertToObject(asJsonObject!) as IElement;
        Assert.That(deconverted, Is.Not.Null);
        
        var query = deconverted.get<IElement>("query");
        Assert.That(query, Is.Not.Null);
        Assert.That(query.getOrDefault<string>("name"), Is.EqualTo("test"));
        
        var nodes = query.get<IReflectiveCollection>("nodes");
        Assert.That(nodes, Is.Not.Null);
        Assert.That(nodes.size(), Is.EqualTo(2));
        
        var node1 = nodes.OfType<IElement>().FirstOrDefault(x => x.getOrDefault<string>("name") == "Dynamic source input");
        Assert.That(node1, Is.Not.Null);
        Assert.That(node1!.getOrDefault<string>("name"), Is.EqualTo("Dynamic source input"));
        
        var node2 = nodes.OfType<IElement>().FirstOrDefault(x => x.getOrDefault<string>("name") == "Limit to 101 elements");
        Assert.That(node2, Is.Not.Null);
        Assert.That(node2!.getOrDefault<int>("amount"), Is.EqualTo(101));
        Assert.That(node2.getOrDefault<string>("name"), Is.EqualTo("Limit to 101 elements"));
        var input = node2.get<IElement>("input");
        Assert.That(input, Is.Not.Null);
        Assert.That(input.getOrDefault<string>("name"), Is.EqualTo("Dynamic source input"));
        Assert.That(input.equals(node1), Is.True);
        
        var resultNode = query.get<IElement>("resultNode");
        Assert.That(resultNode, Is.Not.Null);
        Assert.That(resultNode.getOrDefault<string>("name"), Is.EqualTo("Limit to 101 elements"));
    }
}