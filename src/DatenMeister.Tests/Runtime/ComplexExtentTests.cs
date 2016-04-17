using DatenMeister.Apps.ZipCode;
using DatenMeister.DataLayer;
using DatenMeister.EMOF.InMemory;
using DatenMeister.Full.Integration;
using DatenMeister.Runtime.Extents;
using DatenMeister.Runtime.Workspaces;
using Ninject;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime
{
    public class ComplexExtentTests
    {
        [Test]
        public void TestCreatabeTypes()
        {
            var kernel = new StandardKernel();
            kernel.UseDatenMeister("Xmi");

            // Apply for zipcodes
            var integrateZipCodes = kernel.Get<Integrate>();
            integrateZipCodes.Into(kernel.Get<IWorkspaceCollection>().FindExtent("dm:///types"));
            
            var extentFunctions = kernel.Get<ExtentFunctions>();
            var dataLayers = kernel.Get<DataLayers>();
             
            var dataExtent = new MofUriExtent("dm:///test");
            var creatableTypes = extentFunctions.GetCreatableTypes(dataExtent);
            Assert.That(creatableTypes, Is.Not.Null);
            Assert.That(creatableTypes.MetaLayer, Is.EqualTo(dataLayers.Types));
            Assert.That(creatableTypes.CreatableTypes.Count, Is.EqualTo(1));
        }
    }
}