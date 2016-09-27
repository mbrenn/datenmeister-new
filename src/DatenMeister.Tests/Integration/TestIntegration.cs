using Autofac;
using Autofac.Features.ResolveAnything;
using DatenMeister.Core.EMOF.InMemory;
using DatenMeister.Integration;
using DatenMeister.Integration.DotNet;
using DatenMeister.Provider.CSV;
using DatenMeister.Provider.CSV.Runtime.Storage;
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

                Assert.That(mapper.HasMappingForExtentType(typeof(MofUriExtent)), Is.True);
                Assert.That(mapper.HasMappingForExtentType(typeof(MofElement)), Is.False);

                Assert.That(mapper.FindFactoryFor(scope, typeof(MofUriExtent)), Is.TypeOf<MofFactory>());

                var uriExtent = new MofUriExtent("dm:///localhost");
                Assert.That(mapper.FindFactoryFor(scope, uriExtent), Is.TypeOf<MofFactory>());
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