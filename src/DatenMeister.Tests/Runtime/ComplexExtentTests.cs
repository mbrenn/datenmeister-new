using Autofac;
using DatenMeister.Apps.ZipCode;
using DatenMeister.DataLayer;
using DatenMeister.EMOF.InMemory;
using DatenMeister.Integration;
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
            kernel.UseDatenMeister(new IntegrationSettings {PathToXmiFiles = "Xmi"});
            var builder = kernel.Build();
            using (var scope = builder.BeginLifetimeScope())
            {
                // Apply for zipcodes
                var integrateZipCodes = scope.Resolve<Integrate>();
                integrateZipCodes.Into(scope.Resolve<IWorkspaceCollection>().FindExtent("dm:///types"));

                var extentFunctions = scope.Resolve<ExtentFunctions>();
                var dataLayers = scope.Resolve<DataLayers>();

                var dataExtent = new MofUriExtent("dm:///test");
                var creatableTypes = extentFunctions.GetCreatableTypes(dataExtent);
                Assert.That(creatableTypes, Is.Not.Null);
                Assert.That(creatableTypes.MetaLayer, Is.EqualTo(dataLayers.Types));
                Assert.That(creatableTypes.CreatableTypes.Count, Is.EqualTo(1));
            }
        }
    }
}