using DatenMeister.EMOF.InMemory;
using DatenMeister.Full.Integration;
using DatenMeister.Runtime.FactoryMapper;
using NUnit.Framework;

namespace DatenMeister.Tests.Integration
{
    [TestFixture]
    public class TestIntegration
    {
        [Test]
        public void TestFactoryMappingByAttribute()
        {
            var mapper = new DefaultFactoryMapper();
            mapper.PerformAutomaticMappingByAttribute();

            Assert.That(mapper.HasMappingForExtentType(typeof(MofUriExtent)), Is.True);
            Assert.That(mapper.HasMappingForExtentType(typeof(MofElement)), Is.False);
        }
    }
}