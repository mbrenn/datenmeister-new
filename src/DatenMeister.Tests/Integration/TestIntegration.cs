using DatenMeister.CSV;
using DatenMeister.CSV.Runtime.Storage;
using DatenMeister.EMOF.InMemory;
using DatenMeister.Full.Integration;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.FactoryMapper;
using NUnit.Framework;

namespace DatenMeister.Tests.Integration
{
    [TestFixture]
    public class TestIntegration
    {
        [Test]
        public void TestFactoryMappingByAttributeForFactories()
        {
            var mapper = new DefaultFactoryMapper();
            mapper.PerformAutomaticMappingByAttribute();

            Assert.That(mapper.HasMappingForExtentType(typeof(MofUriExtent)), Is.True);
            Assert.That(mapper.HasMappingForExtentType(typeof(MofElement)), Is.False);

            Assert.That(mapper.FindFactoryFor(typeof (MofUriExtent)), Is.TypeOf<MofFactory>());

            var uriExtent = new MofUriExtent("dm:///localhost");
            Assert.That(mapper.FindFactoryFor(uriExtent), Is.TypeOf<MofFactory>());
        }

        [Test]
        public void TestFactoryMappingByAttributeForExtentLoaders()
        {
            var mapper = new ManualConfigurationToExtentStorageMapper();
            mapper.PerformMappingForConfigurationOfExtentLoaders();

            Assert.That(mapper.HasMappingFor(typeof(CSVStorageConfiguration)), Is.True);
            Assert.That(mapper.HasMappingFor(typeof(CSVDataProvider)), Is.False);
        }
    }
}