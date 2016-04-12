using DatenMeister.CSV;
using DatenMeister.CSV.Runtime.Storage;
using DatenMeister.EMOF.InMemory;
using DatenMeister.Full.Integration;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.FactoryMapper;
using Ninject;
using NUnit.Framework;

namespace DatenMeister.Tests.Integration
{
    [TestFixture]
    public class TestIntegration
    {
        [Test]
        public void TestFactoryMappingByAttributeForFactories()
        {
            var kernel = new StandardKernel();
            var mapper = new DefaultFactoryMapper();
            mapper.PerformAutomaticMappingByAttribute(kernel);

            Assert.That(mapper.HasMappingForExtentType(typeof(MofUriExtent)), Is.True);
            Assert.That(mapper.HasMappingForExtentType(typeof(MofElement)), Is.False);

            Assert.That(mapper.FindFactoryFor(typeof (MofUriExtent)), Is.TypeOf<MofFactory>());

            var uriExtent = new MofUriExtent("dm:///localhost");
            Assert.That(mapper.FindFactoryFor(uriExtent), Is.TypeOf<MofFactory>());
        }

        [Test]
        public void TestFactoryMappingByAttributeForExtentLoaders()
        {
            var kernel = new StandardKernel();
            kernel.UseDatenMeister("Xmi");

            var mapper = new ManualConfigurationToExtentStorageMapper();
            mapper.PerformMappingForConfigurationOfExtentLoaders(kernel);

            Assert.That(mapper.HasMappingFor(typeof(CSVStorageConfiguration)), Is.True);
            Assert.That(mapper.HasMappingFor(typeof(CSVDataProvider)), Is.False);
        }
    }
}