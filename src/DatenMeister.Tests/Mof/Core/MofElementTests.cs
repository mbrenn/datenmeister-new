using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.InMemory;
using NUnit.Framework;

namespace DatenMeister.Tests.Mof.Core
{
    [TestFixture]
    public class MofElementTests
    {
        [Test]
        public void TestNoDoubleId()
        {
            var extent = new MofUriExtent(new InMemoryProvider());

            var element1 = MofFactory.Create(extent, null);
            var element2 = MofFactory.Create(extent, null);

            extent.elements().add(element1);
            extent.elements().add(element2);

            (element1 as ICanSetId)!.Id = "YES";
            Assert.That((element1 as IHasId)!.Id, Is.EqualTo("YES"));
            (element1 as ICanSetId)!.Id = "YES";
            Assert.That((element1 as IHasId)!.Id, Is.EqualTo("YES"));
            (element2 as ICanSetId)!.Id = "No";
            Assert.That((element2 as IHasId)!.Id, Is.EqualTo("No"));

            Assert.Throws<InvalidOperationException>(() =>
            {
                (element2 as ICanSetId)!.Id = "YES";
            });
            
            Assert.That((element2 as IHasId)!.Id, Is.EqualTo("No"));
        }
    }
}