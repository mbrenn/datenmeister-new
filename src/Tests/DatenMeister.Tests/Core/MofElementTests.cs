using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Provider.InMemory;
using NUnit.Framework;

namespace DatenMeister.Tests.Core;

[TestFixture]
public class MofElementTests
{
    [Test]
    public void TestNoDoubleId()
    {
        var extent = new MofUriExtent(new InMemoryProvider(), null);

        var element1 = MofFactory.CreateElement(extent, null);
        var element2 = MofFactory.CreateElement(extent, null);

        extent.elements().add(element1);
        extent.elements().add(element2);

        (element1 as ICanSetId)!.Id = "YES";
        Assert.That((element1 as IHasId)!.Id, Is.EqualTo("YES"));
        (element1 as ICanSetId)!.Id = "YES";
        Assert.That((element1 as IHasId)!.Id, Is.EqualTo("YES"));
        (element2 as ICanSetId)!.Id = "No";
        Assert.That((element2 as IHasId)!.Id, Is.EqualTo("No"));

        Assert.Throws<IdIsAlreadySetException>(() => { (element2 as ICanSetId)!.Id = "YES"; });

        Assert.That((element2 as IHasId)!.Id, Is.EqualTo("No"));
    }

    [Test]
    public void SetAndGetId()
    {
        var extent = new MofUriExtent(new InMemoryProvider(), null);

        var element1 = MofFactory.CreateElement(extent, null);

        var oldId = element1.GetId();
        Assert.That(oldId, Is.Not.Null.And.Not.Empty);

        element1.SetId("Test1");
        Assert.That(element1.GetId(), Is.EqualTo("Test1"));
    }

    [Test]
    public void SetIdWithSameValue()
    {
        var extent = new MofUriExtent(new InMemoryProvider(), null);

        var element1 = MofFactory.CreateElement(extent, null);

        var oldId = element1.GetId();
        (element1 as ICanSetId)!.Id = oldId;

        Assert.That(element1.GetId(), Is.EqualTo(oldId));
        Assert.That((element1 as IHasId)!.Id, Is.EqualTo(oldId));
    }
}