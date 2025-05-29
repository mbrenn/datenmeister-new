using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using NUnit.Framework;

namespace DatenMeister.Tests.Provider;

[TestFixture]
public class InMemoryProviderTests
{
    [Test]
    public void TestSuite()
    {
        var provider = new InMemoryProvider();
        ProviderTestSuite.TestProviderObject(provider);
    }
        
    [Test]
    public void TestStringsInReflectiveCollection()
    {
        var provider = new InMemoryProvider();
        var mofExtent = new MofUriExtent(provider, "dm:///test", null);

        var element = MofFactory.CreateElement(mofExtent, null);
        mofExtent.elements().add(element);

        var list = new List<string> {"ABC", "DEF", "GHI", "JKL"};
        element.set("test", list);


        var result = element.getOrDefault<IReflectiveCollection>("test");
        Assert.That(result, Is.Not.Null);


        Assert.That(result.Count(), Is.EqualTo(4));
        Assert.That(result.ElementAt(0), Is.EqualTo("ABC"));
        Assert.That(result.ElementAt(1), Is.EqualTo("DEF"));
        Assert.That(result.ElementAt(2), Is.EqualTo("GHI"));
        Assert.That(result.ElementAt(3), Is.EqualTo("JKL"));
    }
}