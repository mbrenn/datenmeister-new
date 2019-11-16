using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.ZipExample;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime.Extents
{
    [TestFixture]
    public class ExtentTests
    {
        [Test]
        public void ExtentTest()
        {
            var kernel = new ContainerBuilder();
            var builder = kernel.UseDatenMeister(new IntegrationSettings());
            using var scope = builder.BeginLifetimeScope();
            var workspaceLogic = scope.Resolve<IWorkspaceLogic>();
            var workspaceExtent = workspaceLogic.FindExtent(WorkspaceNames.ExtentManagementExtentUri);
            Assert.That(workspaceExtent, Is.Not.Null);
            var asData = workspaceExtent.elements().Cast<IElement>().First(x => x.get("id").ToString() == WorkspaceNames.NameData);
            var asManagement = workspaceExtent.elements().Cast<IElement>().First(x => x.get("id").ToString() == WorkspaceNames.NameManagement);
            var asTypes = workspaceExtent.elements().Cast<IElement>().First(x => x.get("id").ToString() == WorkspaceNames.NameTypes);
            var asMof = workspaceExtent.elements().Cast<IElement>().First(x => x.get("id").ToString() == WorkspaceNames.NameMof);

            Assert.That(asData, Is.Not.Null);
            Assert.That(asManagement, Is.Not.Null);
            Assert.That(asTypes, Is.Not.Null);
            Assert.That(asMof, Is.Not.Null);

            // Get the extents
            var extents = (asMof.get("extents") as IEnumerable<object>)?.ToList();
            Assert.That(extents, Is.Not.Null);

            var mofExtent = extents.Cast<IElement>().First( x=> x.get("uri").ToString() == WorkspaceNames.UriMofExtent);
            Assert.That(mofExtent, Is.Not.Null);
        }

        [Test]
        public void TestMetaDataInExtent()
        {
            var path = "./test.xmi";
            var loaderConfig = new XmiStorageConfiguration
            {
                filePath = path,
                extentUri = "datenmeister:///data",
                workspaceId = WorkspaceNames.NameData
            };
            
            using (var dm = DatenMeisterTests.GetDatenMeisterScope())
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                var extentLoader = dm.Resolve<IExtentManager>();
                var loadedExtent = extentLoader.LoadExtent(loaderConfig, ExtentCreationFlags.LoadOrCreate);
                loadedExtent.set("test", "this is a test");
                loadedExtent.SetExtentType("Happy Extent");
                extentLoader.StoreExtent(loadedExtent);

                dm.UnuseDatenMeister();
            }

            using (var dm = DatenMeisterTests.GetDatenMeisterScope(dropDatabase: false))
            {
                var workspaceLogic = dm.Resolve<IWorkspaceLogic>();
                var foundExtent = workspaceLogic.FindExtent("datenmeister:///data");
                Assert.That(foundExtent, Is.Not.Null);

                Assert.That(foundExtent.get("test"), Is.EqualTo("this is a test"));
                Assert.That(foundExtent.GetExtentType(), Is.EqualTo("Happy Extent"));

                dm.UnuseDatenMeister();
            }
        }

        [Test]
        public void TestDefaultExtentType()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();
            var workspaceLogic = dm.Resolve<IWorkspaceLogic>();
            var zipCodeExample = dm.Resolve<ZipCodeExampleManager>();
            var typesWorkspace = workspaceLogic.GetTypesWorkspace();
            var zipCodeModel =
                typesWorkspace.FindElementByUri("datenmeister:///_internal/types/internal?" +
                                                ZipCodeModel.PackagePath);

            var dataWorkspace = workspaceLogic.GetDataWorkspace();

            var zipExample = zipCodeExample.AddZipCodeExample(dataWorkspace);
            var setDefaultTypePackage = zipExample.GetDefaultTypePackages()?.ToList();

            Assert.That(setDefaultTypePackage, Is.Not.Null);
            Assert.That(zipCodeModel, Is.Not.Null);

            Assert.That(setDefaultTypePackage.FirstOrDefault(), Is.EqualTo(zipCodeModel));
        }

        [Test]
        public void TestAddDefaultExtentType()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();
            var workspaceLogic = dm.Resolve<IWorkspaceLogic>();
            var zipCodeExample = dm.Resolve<ZipCodeExampleManager>();
            var typesWorkspace = workspaceLogic.GetTypesWorkspace();
            var zipCodeModel =
                typesWorkspace.FindElementByUri("datenmeister:///_internal/types/internal?" +
                                                ZipCodeModel.PackagePath) as IElement;
            Assert.That(zipCodeModel, Is.Not.Null);

            var dataWorkspace = workspaceLogic.GetDataWorkspace();
                
            var zipExample = zipCodeExample.AddZipCodeExample(dataWorkspace);
                
            // Per Default, one is included
            var setDefaultTypePackage = zipExample.GetDefaultTypePackages()?.ToList();
            Assert.That(setDefaultTypePackage, Is.Not.Null);
            Assert.That(setDefaultTypePackage.Count, Is.EqualTo(1));
            Assert.That(setDefaultTypePackage.FirstOrDefault(), Is.EqualTo(zipCodeModel));

            // Checks, if adding another one does not work
            zipExample.AddDefaultTypePackages(new[] {zipCodeModel});
            setDefaultTypePackage = zipExample.GetDefaultTypePackages()?.ToList();
            Assert.That(setDefaultTypePackage, Is.Not.Null);
            Assert.That(setDefaultTypePackage.Count, Is.EqualTo(1));
            Assert.That(setDefaultTypePackage.FirstOrDefault(), Is.EqualTo(zipCodeModel));

            // Checks, if removing works
            zipExample.SetDefaultTypePackages(new IElement[] { });
            setDefaultTypePackage = zipExample.GetDefaultTypePackages()?.ToList();
            Assert.That(setDefaultTypePackage, Is.Not.Null);
            Assert.That(setDefaultTypePackage.Count, Is.EqualTo(0));
                
            // Checks, if adding works now correctly
            zipExample.AddDefaultTypePackages(new[] {zipCodeModel});
            setDefaultTypePackage = zipExample.GetDefaultTypePackages()?.ToList();
            Assert.That(setDefaultTypePackage, Is.Not.Null);
            Assert.That(setDefaultTypePackage.Count, Is.EqualTo(1));
            Assert.That(setDefaultTypePackage.FirstOrDefault(), Is.EqualTo(zipCodeModel));
        }

        [Test]
        public void TestStoringOfExtentTypes()
        {
            const string csvExtentUri = "dm:///csvtest";
            const string xmiExtentUri = "dm:///moftest";

            using (var dm = DatenMeisterTests.GetDatenMeisterScope())
            {
                var extentManager = dm.Resolve<IExtentManager>();
                var csvExtent = extentManager.LoadExtent(
                    new CsvExtentLoaderConfig
                    {
                        filePath = "./test.csv",
                        extentUri = csvExtentUri
                    }, ExtentCreationFlags.LoadOrCreate);

                var mofExtent = extentManager.LoadExtent(
                    new XmiStorageConfiguration
                    {
                        filePath = "./test.xmi",
                        extentUri = xmiExtentUri
                    }, ExtentCreationFlags.LoadOrCreate);

                csvExtent.SetExtentType("CSVExtent");
                mofExtent.SetExtentType("XMIExtent");

                dm.UnuseDatenMeister();
            }

            using (var dm = DatenMeisterTests.GetDatenMeisterScope(false))
            {
                var workspaceLogic = dm.Resolve<IWorkspaceLogic>();
                var csvExtent = workspaceLogic.FindExtent(csvExtentUri);
                var xmiExtent = workspaceLogic.FindExtent(xmiExtentUri);

                Assert.That(csvExtent, Is.Not.Null);
                Assert.That(xmiExtent, Is.Not.Null);

                Assert.That(csvExtent.GetExtentType(), Is.EqualTo("CSVExtent"));
                Assert.That(xmiExtent.GetExtentType(), Is.EqualTo("XMIExtent"));

                dm.UnuseDatenMeister();
            }
        }

        [Test]
        public static void TestAlternativeUris()
        {
            var mofExtent = new MofUriExtent(new InMemoryProvider(), "dm:///a");
            mofExtent.AlternativeUris.Add("dm:///test");
            mofExtent.AlternativeUris.Add("dm:///test2");

            Assert.That(mofExtent.AlternativeUris.Count, Is.EqualTo(2));
            Assert.That(mofExtent.AlternativeUris.Contains("dm:///test"), Is.True);
            Assert.That(mofExtent.AlternativeUris.Contains("dm:///test2"), Is.True);
        }
    }
}