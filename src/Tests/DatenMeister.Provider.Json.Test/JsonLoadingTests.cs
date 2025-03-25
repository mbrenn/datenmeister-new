using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using NUnit.Framework;

namespace DatenMeister.Provider.Json.Test;

[TestFixture]
public class JsonLoadingTests
{

    [Test]
    public void TestImportSimpleJson()
    {
        var provider = new InMemoryProvider();
        var extent = new MofUriExtent(provider, "dm:///test", null);

        var importer = new JsonImporter(new MofFactory(extent));
        importer.ImportFromText(SimpleJson, extent.elements());
        
        Assert.That(extent.elements().Count(), Is.EqualTo(1));
        var element =  extent.elements().OfType<IElement>().First();
        Assert.That(element.getOrDefault<string>("fruit"), Is.EqualTo("Apple"));
        Assert.That(element.getOrDefault<string>("size"), Is.EqualTo("Large"));
        Assert.That(element.getOrDefault<string>("color"), Is.EqualTo("Red"));
        Assert.That(element.getOrDefault<int>("number"), Is.EqualTo(4));
    }

    [Test]
    public void TestSimpleJsonWithArray()
    {
        var provider = new InMemoryProvider();
        var extent = new MofUriExtent(provider, "dm:///test", null);

        var importer = new JsonImporter(new MofFactory(extent));
        importer.ImportFromText(SimpleJsonWithArray, extent.elements());
        
        Assert.That(extent.elements().Count(), Is.EqualTo(1));
        var element = extent.elements().OfType<IElement>().First();
        Assert.That(element.getOrDefault<string>("brand"), Is.EqualTo("Mercedes"));

        var wheels = element.getOrDefault<IReflectiveCollection>("wheels");
        Assert.That(wheels,Is.Not.Null);
        var wheelsAsList = wheels.ToList();
        Assert.That(wheelsAsList.Count, Is.EqualTo(4));

        var secondWheel = wheelsAsList.OfType<IElement>().ElementAt(1);
        Assert.That(
            secondWheel.getOrDefault<string>("name"), 
            Is.EqualTo("Wheel Front Right"));
    }

    [Test]
    public void TestMoreComplexJson()
    {
        var provider = new InMemoryProvider();
        var extent = new MofUriExtent(provider, "dm:///test", null);

        var importer = new JsonImporter(new MofFactory(extent));
        importer.ImportFromText(MoreComplexJson, extent.elements());
        
        Assert.That(extent.elements().Count(), Is.EqualTo(1));
        var element =  extent.elements().OfType<IElement>().First();
        var quiz = element.getOrDefault<IElement>("quiz");
        Assert.That(quiz, Is.Not.Null);
        var maths = quiz.getOrDefault<IElement>("maths");
        Assert.That(maths, Is.Not.Null);
        var q1 = maths.getOrDefault<IElement>("q1");
        Assert.That(q1, Is.Not.Null);

        var question = q1.getOrDefault<string>("question");
        Assert.That(question, Is.EqualTo("5 + 7 = ?"));
    }

    [Test]
    public void TestSimpleMultipleJson()
    {
        var provider = new InMemoryProvider();
        var extent = new MofUriExtent(provider, "dm:///test", null);

        var importer = new JsonImporter(new MofFactory(extent));
        importer.ImportFromText(SimpleMultipleJson, extent.elements());
        
        Assert.That(extent.elements().Count(), Is.EqualTo(2));
        var value2 = extent.elements().ElementAt(1) as IElement;
        Assert.That(value2, Is.Not.Null);
        Assert.That(value2.getOrDefault<string>("fruit"), Is.EqualTo("Peach"));
        Assert.That(value2.getOrDefault<int>("number"), Is.EqualTo(1));
    }


    public const string SimpleJson =
        """
        {
            "fruit": "Apple",
            "size": "Large",
            "color": "Red",
            "number": 4
        }
        """;
    
    public const string SimpleMultipleJson =
        """
        [
            {
                "fruit": "Apple",
                "size": "Large",
                "color": "Red",
                "number": 4
            },
            {
                "fruit": "Peach",
                "size": "Small",
                "color": "Yellow",
                "number": 1
            }
        ]
        """;

    
    
    public const string SimpleJsonWithArray =
        """
        {
            "brand": "Mercedes",
            "wheels": [
                {
                    "name": "Wheel Front Left"
                },
                {
                    "name": "Wheel Front Right"
                },
                {
                    "name": "Wheel Rear Left"
                },
                {
                    "name": "Wheel Rear Right"
                }  ],
            "driver": "The Best"
        }
        """;

    public const string MoreComplexJson =
        """
        {
            "quiz": {
                "sport": {
                    "q1": {
                        "question": "Which one is correct team name in NBA?",
                        "options": [
                            "New York Bulls",
                            "Los Angeles Kings",
                            "Golden State Warriros",
                            "Huston Rocket"
                        ],
                        "answer": "Huston Rocket"
                    }
                },
                "maths": {
                    "q1": {
                        "question": "5 + 7 = ?",
                        "options": [
                            "10",
                            "11",
                            "12",
                            "13"
                        ],
                        "answer": "12"
                    },
                    "q2": {
                        "question": "12 - 8 = ?",
                        "options": [
                            "1",
                            "2",
                            "3",
                            "4"
                        ],
                        "answer": "4"
                    }
                }
            }
        }
        """;
}