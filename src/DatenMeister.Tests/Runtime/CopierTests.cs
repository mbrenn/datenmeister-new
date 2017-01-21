using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.Copier;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime
{
    [TestFixture]
    public class CopierTests
    {
        private static string property1 = "Prop1";
        private static string property2 = "Prop2";

        [Test]
        public void TestCopyOfObject()
        {
            var mofExtent = new MofUriExtent(new InMemoryProvider(), "dm:///");
            var mofObject = new MofFactory(mofExtent).create(null);
            mofObject.set(property1, "55130");
            mofObject.set(property2, "Mainz");

            var mofObject2 = new MofFactory(mofExtent).create(null);
            mofObject2.set(property1, "65474");
            mofObject2.set(property2, "Bischofsheim");

            var copier = new ObjectCopier(new MofFactory(mofExtent));
            var result1 = copier.Copy(mofObject);
            var result2 = copier.Copy(mofObject2);

            Assert.That(result1, Is.Not.Null);
            Assert.That(result1.get(property1).ToString(), Is.EqualTo("55130"));
            Assert.That(result1.get(property2).ToString(), Is.EqualTo("Mainz"));
            Assert.That(result2.get(property1).ToString(), Is.EqualTo("65474"));
            Assert.That(result2.get(property2).ToString(), Is.EqualTo("Bischofsheim"));
        }
    }
}