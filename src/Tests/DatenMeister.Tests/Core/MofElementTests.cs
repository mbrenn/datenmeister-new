﻿using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Provider.InMemory;
using NUnit.Framework;

namespace DatenMeister.Tests.Core
{
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

            Assert.Throws<InvalidOperationException>(() => { (element2 as ICanSetId)!.Id = "YES"; });

            Assert.That((element2 as IHasId)!.Id, Is.EqualTo("No"));
        }
    }
}