using DatenMeister.Provider.InMemory;
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
    }
}