using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Provider.XMI.ExtentStorage;
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
            using (var scope = builder.BeginLifetimeScope())
            {
                var workspaceLogic = scope.Resolve<IWorkspaceLogic>();
                var workspaceExtent = workspaceLogic.FindExtent(ExtentOfWorkspaces.WorkspaceUri);
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

                var mofExtent = extents.Cast<IElement>().First( x=> x.get("uri").ToString() == WorkspaceNames.UriMof);
                Assert.That(mofExtent, Is.Not.Null);
            }
        }

        [Test]
        public void TestMetaDataInExtent()
        {
            var currentDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "database");
            var path = Path.Combine(currentDirectory, "test.xmi");
            var loaderConfig = new XmiStorageConfiguration
            {
                Path = path,
                ExtentUri = "dm:///data",
                Workspace = WorkspaceNames.NameData
            };
            var integrationSettings = new IntegrationSettings
            {
                DatabasePath = currentDirectory,
                EstablishDataEnvironment = true
            };

            GiveMe.DropDatenMeisterStorage(integrationSettings);

            using (var dm = GiveMe.DatenMeister(integrationSettings))
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }


                var extentLoader = dm.Resolve<IExtentManager>();
                var loadedExtent = extentLoader.LoadExtent(loaderConfig, true);
                loadedExtent.set("test", "this is a test");
                extentLoader.StoreExtent(loadedExtent);

                dm.UnuseDatenMeister();
            }

            using (var dm = GiveMe.DatenMeister(integrationSettings))
            {
                var workspaceLogic = dm.Resolve<IWorkspaceLogic>();
                var foundExtent = workspaceLogic.FindExtent("dm:///data");
                Assert.That(foundExtent, Is.Not.Null);

                Assert.That(foundExtent.get("test"), Is.EqualTo("this is a test"));

                dm.UnuseDatenMeister();
            }
        }
    }
}