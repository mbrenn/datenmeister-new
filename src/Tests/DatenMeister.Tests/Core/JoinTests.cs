using DatenMeister.Core.Functions.Manipulation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using NUnit.Framework;

namespace DatenMeister.Tests.Core;

[TestFixture]
public class JoinTests
{
    [Test]
    public void TestSimpleJoinWithStrings()
    {
        var item1 = InMemoryObject.CreateEmpty();
        var item2 = InMemoryObject.CreateEmpty();
        item1.set("prename", "M");
        item1.set("name", "B");
        item1.set("age", 16);
        item1.set("color", "red");

        item2.set("prename", "M");
        item2.set("name", "X");
        item2.set("color", "green");

        var colorRed = InMemoryObject.CreateEmpty();
        colorRed.set("name", "red");
        colorRed.set("R", 255);
        colorRed.set("G", 0);
        colorRed.set("B", 0);

        var colorGreen = InMemoryObject.CreateEmpty();
        colorGreen.set("name", "green");
        colorGreen.set("R", 0);
        colorGreen.set("G", 255);
        colorGreen.set("B", 0);

        var colorBlue = InMemoryObject.CreateEmpty();
        colorBlue.set("name", "blue");
        colorBlue.set("R", 0);
        colorBlue.set("G", 0);
        colorBlue.set("B", 255);

        JoinManipulation.JoinSimple(
            [item1, item2],
            [colorRed, colorGreen, colorBlue],
            "color",
            "name");

        Assert.That(item1.getOrDefault<string>("name"), Is.EqualTo("B"));
        Assert.That(item1.getOrDefault<int>("R"), Is.EqualTo(255));
        Assert.That(item1.getOrDefault<int>("G"), Is.EqualTo(0));
        Assert.That(item1.getOrDefault<int>("B"), Is.EqualTo(0));

        Assert.That(item2.getOrDefault<string>("name"), Is.EqualTo("X"));
        Assert.That(item2.getOrDefault<int>("R"), Is.EqualTo(0));
        Assert.That(item2.getOrDefault<int>("G"), Is.EqualTo(255));
        Assert.That(item2.getOrDefault<int>("B"), Is.EqualTo(0));
    }
}