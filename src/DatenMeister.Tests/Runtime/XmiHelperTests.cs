using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime
{
    [TestFixture]
    public class XmiHelperTests
    {
        public void TestConvertToXmi()
        {
            var value = InMemoryObject.CreateEmpty();
            value.set("name", "Martin");
            value.set("lastname", "Brenn");

            var result = XmiHelper.ConvertToXmiFromObject(value);
            Assert.That(result, Does.Contain("name"));
            Assert.That(result, Does.Contain("lastname"));
            Assert.That(result, Does.Contain("Martin"));
            Assert.That(result, Does.Contain("Brenn"));
        }
    }
}