using System.Xml.Linq;
using DatenMeister.Provider.XMI.EMOF;
using NUnit.Framework;

namespace DatenMeister.Tests.Provider
{
    [TestFixture]
    public class XmiProviderTests
    {
        [Test]
        public void TestGetAndSet()
        {
            var provider = new XmiProvider();
            ProviderTestHelper.TestGetAndSetOfPrimitiveTypes(provider);
        }
        
        [Test]
        public void TestLists()
        {
            var provider = new XmiProvider();
            ProviderTestHelper.TestListsWithObjects(provider);
        }

        [Test]
        public void TestMovements()
        {
            var provider = new XmiProvider();
            ProviderTestHelper.TestListMovement(provider);
        }

        [Test]
        public void TestSetReferenceAndSetValue()
        {
            var provider = new XmiProvider();
            ProviderTestHelper.TestSetReferenceAndSetValue(provider);
        }
    }
}