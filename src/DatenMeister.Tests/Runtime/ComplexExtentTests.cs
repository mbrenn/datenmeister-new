using Autofac;
using DatenMeister.Apps.ZipCode;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Integration;
using DatenMeister.Integration.DotNet;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.Extents;
using DatenMeister.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime
{
    public class ComplexExtentTests
    {
        [Test]
        public void TestCreatabeTypes()
        {
            var kernel = new ContainerBuilder();
            var builder = kernel.UseDatenMeisterDotNet(new IntegrationSettings());
            using (var scope = builder.BeginLifetimeScope())
            {
                // Apply for zipcodes
                var integrateZipCodes = scope.Resolve<ZipCodePlugin>();
                integrateZipCodes.Start();

                var extentFunctions = scope.Resolve<ExtentFunctions>();
                var dataLayerLogic = scope.Resolve<IWorkspaceLogic>();

                var dataExtent = new MofUriExtent(new InMemoryProvider(), "dm:///test");
                var creatableTypes = extentFunctions.GetCreatableTypes(dataExtent);
                Assert.That(creatableTypes, Is.Not.Null);
                Assert.That(creatableTypes.MetaLayer, Is.EqualTo(dataLayerLogic.GetTypes()));
                Assert.That(creatableTypes.CreatableTypes.Count, Is.GreaterThan(1));
            }
        }
    }
}