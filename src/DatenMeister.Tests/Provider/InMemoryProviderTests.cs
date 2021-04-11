using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using NUnit.Framework;

namespace DatenMeister.Tests.Provider
{
    [TestFixture]
    public class InMemoryProviderTests
    {
        [Test]
        public void TestGetAndSet()
        {
            var provider = new InMemoryProvider();
            ProviderTestHelper.TestGetAndSetOfPrimitiveTypes(provider);
        }

        [Test]
        public void TestLists()
        {
            var provider = new InMemoryProvider();
            ProviderTestHelper.TestListsWithObjects(provider);
        }

        [Test]
        public void TestMovements()
        {
            var provider = new InMemoryProvider();
            ProviderTestHelper.TestListMovement(provider);
        }
        
        [Test]
        public void TestSetReferenceAndSetValue()
        {
            var provider = new InMemoryProvider();
            ProviderTestHelper.TestSetReferenceAndSetValue(provider);
        }

        [Test]
        public void TestStringsInReflectiveCollection()
        {
            var provider = new InMemoryProvider();
            var mofExtent = new MofUriExtent(provider, "dm:///test");

            var element = MofFactory.Create(mofExtent, null);
            mofExtent.elements().add(element);

            var list = new List<string>() {"ABC", "DEF", "GHI", "JKL"};
            element.set("test", list);


            var result = element.getOrDefault<IReflectiveCollection>("test");
            Assert.That(result, Is.Not.Null );


            Assert.That(result.Count(), Is.EqualTo(4));
            Assert.That(result.ElementAt(0), Is.EqualTo("ABC"));
            Assert.That(result.ElementAt(1), Is.EqualTo("DEF"));
            Assert.That(result.ElementAt(2), Is.EqualTo("GHI"));
            Assert.That(result.ElementAt(3), Is.EqualTo("JKL"));
        }
    }
}