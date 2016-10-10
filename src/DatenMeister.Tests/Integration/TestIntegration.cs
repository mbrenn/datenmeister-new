using Autofac;
using Autofac.Features.ResolveAnything;
using DatenMeister.Integration;
using DatenMeister.Integration.DotNet;
using DatenMeister.Provider.CSV;
using DatenMeister.Provider.CSV.Runtime.Storage;
using DatenMeister.Provider.InMemory;
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
            var kernel = new ContainerBuilder();
            kernel.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            var builder = kernel.Build();

            using (var scope = builder.BeginLifetimeScope())
            {
                var mapper = new DefaultFactoryMapper();
                mapper.PerformAutomaticMappingByAttribute();

                Assert.That(mapper.HasMappingForExtentType(typeof(InMemoryUriExtent)), Is.True);
                Assert.That(mapper.HasMappingForExtentType(typeof(InMemoryElement)), Is.False);

                Assert.That(mapper.FindFactoryFor(scope, typeof(InMemoryUriExtent)), Is.TypeOf<InMemoryFactory>());

                var uriExtent = new InMemoryUriExtent("dm:///localhost");
                Assert.That(mapper.FindFactoryFor(scope, uriExtent), Is.TypeOf<InMemoryFactory>());
            }
        }

        [Test]
        public void TestFactoryMappingByAttributeForExtentLoaders()
        {
            var kernel = new ContainerBuilder();
            var builder = kernel.UseDatenMeisterDotNet(new IntegrationSettings());
            using (var scope = builder.BeginLifetimeScope())
            {

                var mapper = new ManualConfigurationToExtentStorageMapper();
                mapper.PerformMappingForConfigurationOfExtentLoaders();

                Assert.That(mapper.HasMappingFor(typeof(CSVStorageConfiguration)), Is.True);
                Assert.That(mapper.HasMappingFor(typeof(CSVDataProvider)), Is.False);
            }
        }
    }
}