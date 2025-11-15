using DatenMeister.Core.Functions.Manipulation;
using DatenMeister.Core.Provider.InMemory;
using NUnit.Framework;

namespace DatenMeister.Tests.Core;

[TestFixture]
public class ManipulationTests
{
    [Test]
    public void TestKeepOnlyFields()
    {
        var item1 = InMemoryObject.CreateEmpty();
        var item2 = InMemoryObject.CreateEmpty();
        item1.set("prename", "M");
        item1.set("name", "B");
        item1.set("age", 16);
        
        item2.set("prename", "M");
        item2.set("name", "B");
        item2.set("age", 16);
        
        var list = new [] {item1, item2};
        
        FieldManipulation.KeepOnlyFields(list, ["prename", "name"]);

        foreach (var item in list)
        {
            Assert.That(string.IsNullOrEmpty(item.getOrDefault<string>("prename")), Is.False);
            Assert.That(string.IsNullOrEmpty(item.getOrDefault<string>("name")), Is.False);
            Assert.That(string.IsNullOrEmpty(item.getOrDefault<string>("age")), Is.True);
        }
    }
}