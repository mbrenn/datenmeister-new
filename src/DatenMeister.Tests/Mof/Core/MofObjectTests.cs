using System;
using System.Text;
using System.Collections.Generic;
using DatenMeister.MOF.InMemory;
using NUnit.Framework;

namespace DatenMeister.Tests.Mof.Core
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für MofObjectTests
    /// </summary>
    [TestFixture]
    public class MofObjectTests
    {
        [Test]
        public void TestInMemoryMofObject()
        {
            var property1 = new object();
            var property2 = new object();

            var mofObject = new MofObject();
            Assert.That(mofObject.isSet(property1), Is.False);
            mofObject.set(property1, "Test");
            mofObject.set(property2, property1);

            Assert.That(mofObject.isSet(property1),Is.True);
            Assert.That(mofObject.isSet(property2),Is.True);

            Assert.That(mofObject.get(property1).ToString(), Is.EqualTo("Test"));
            Assert.That(mofObject.get(property2), Is.EqualTo(property1));
        }
    }
}
