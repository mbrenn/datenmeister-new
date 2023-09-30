using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Json;
using DatenMeister.WebServer.Controller;
using NUnit.Framework;

namespace DatenMeister.Tests.Web
{
    [TestFixture]
    public class ExtentControllerTests
    {
        [Test]
        public void TestSetProperties()
        {
            var (workspaceLogic, scopeStorage) = DatenMeisterTests.GetDmInfrastructure();
            var newExtent = new MofUriExtent(new InMemoryProvider(), "dm:///test", scopeStorage);
            workspaceLogic.AddExtent(workspaceLogic.GetDataWorkspace(), newExtent);

            var extentController = new ExtentController(workspaceLogic, scopeStorage);

            var data = InMemoryObject.CreateEmpty();
            data.set(ExtentConfiguration.NameProperty, "name");
            data.set(ExtentConfiguration.ExtentTypeProperty, "extentType");

            var asJson = MofJsonConverter.ConvertToJsonWithDefaultParameter(data);
            var deserialized = JsonSerializer.Deserialize<MofObjectAsJson>(asJson)
                               ?? throw new InvalidOperationException("Serialization failed");

            extentController.SetProperties(WorkspaceNames.WorkspaceData, "dm:///test", deserialized);

            var extentConfiguration = new ExtentConfiguration(newExtent);
            Assert.That(extentConfiguration.Name, Is.EqualTo("name"));
            Assert.That(extentConfiguration.ExtentType, Is.EqualTo("extentType"));
        }

        [Test]
        public void TestGetProperties()
        {
            var (workspaceLogic, scopeStorage) = DatenMeisterTests.GetDmInfrastructure();
            var newExtent = new MofUriExtent(new InMemoryProvider(), "dm:///test", scopeStorage);
            workspaceLogic.AddExtent(workspaceLogic.GetDataWorkspace(), newExtent);

            var extentController = new ExtentController(workspaceLogic, scopeStorage);

            var data = InMemoryObject.CreateEmpty();
            data.set(ExtentConfiguration.NameProperty, "name");
            data.set(ExtentConfiguration.ExtentTypeProperty, "extentType");

            var asJson = MofJsonConverter.ConvertToJsonWithDefaultParameter(data);
            var deserialized = JsonSerializer.Deserialize<MofObjectAsJson>(asJson)
                               ?? throw new InvalidOperationException("Serialization failed");

            extentController.SetProperties(WorkspaceNames.WorkspaceData, "dm:///test", deserialized);

            var result = extentController.GetProperties(WorkspaceNames.WorkspaceData, "dm:///test");
            var deserializedGetProperties = JsonSerializer.Deserialize<MofObjectAsJson>(result.Value!);
            var getProperties = new DirectJsonDeconverter().ConvertToObject(deserializedGetProperties!);
            Assert.That(getProperties.getOrDefault<string>(ExtentConfiguration.NameProperty), Is.EqualTo("name"));
            Assert.That(getProperties.getOrDefault<string>(ExtentConfiguration.ExtentTypeProperty),
                Is.EqualTo("extentType"));
        }

        [Test]
        public void TestAddAndDeleteExtent()
        {
            var dm = DatenMeisterTests.GetDatenMeisterScope();

            var extentController = new ExtentController(dm.WorkspaceLogic, dm.ScopeStorage);

            var n = dm.WorkspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceData)!.extent.Count();
            extentController.CreateXmi(
                new ExtentController.CreateXmiExtentParams
                {
                    Workspace = WorkspaceNames.WorkspaceData,
                    ExtentUri = "dm:///test",
                    FilePath = "./test.xmi"
                });

            Assert.That(dm.WorkspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceData)!.extent.Count(),
                Is.EqualTo(n + 1));
            Assert.That(
                dm.WorkspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceData)!.extent.OfType<IUriExtent>()
                    .Any(x => x.contextURI() == "dm:///test"),
                Is.True);

            extentController.DeleteExtent(new ExtentController.DeleteExtentParams
            {
                Workspace = WorkspaceNames.WorkspaceData,
                ExtentUri = "dm:///test"
            });

            Assert.That(dm.WorkspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceData)!.extent.Count(),
                Is.EqualTo(n));
        }

        [Test]
        public void TestClearExtent()
        {
            var (workspaceLogic, scopeStorage) = DatenMeisterTests.GetDmInfrastructure();

            var extentController = new ExtentController(workspaceLogic, scopeStorage);
            var newExtent = new MofUriExtent(new InMemoryProvider(), "dm:///test", scopeStorage);
            workspaceLogic.AddExtent(workspaceLogic.GetDataWorkspace(), newExtent);

            var factory = new MofFactory(newExtent);
            var martin = factory.create(null);
            var martinson = factory.create(null);
            newExtent.elements().add(martinson);
            newExtent.elements().add(martin);
            
            Assert.That(newExtent.elements().Count(), Is.EqualTo(2));
            
            var result = extentController.ClearExtent(new ExtentController.ClearExtentParams
            {
                Workspace =WorkspaceNames.WorkspaceData,
                ExtentUri = "dm:///test"
            });
            
            Assert.That(newExtent.elements().Count(), Is.EqualTo(0));
            Assert.That(result.Value?.Success, Is.True);
        }

        [Test]
        public void TestExportXmi()
        {
            var (workspaceLogic, scopeStorage) = DatenMeisterTests.GetDmInfrastructure();

            var extentController = new ExtentController(workspaceLogic, scopeStorage);
            var newExtent = new MofUriExtent(new InMemoryProvider(), "dm:///test", scopeStorage);
            workspaceLogic.AddExtent(workspaceLogic.GetDataWorkspace(), newExtent);

            var factory = new MofFactory(newExtent);
            var martin = factory.create(null);
            var martinson = factory.create(null);
            martin.set("name", "Martin");
            martinson.set("name", "Martinson");
            martin.set("child", martinson);

            newExtent.elements().add(martin);

            var result = extentController.ExportXmi(WorkspaceNames.WorkspaceData, "dm:///test");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.Xmi.Contains("Martin"), Is.True);
            Assert.That(result.Value.Xmi.Contains("child"), Is.True);
            Assert.That(result.Value.Xmi.Contains("Martinson"), Is.True);
        }

        [Test]
        public void TestExportXmiOfManagement()
        {
            var dm = DatenMeisterTests.GetDatenMeisterScope();
            
            var extentController = new ExtentController(dm.WorkspaceLogic, dm.ScopeStorage);
            var result = extentController.ExportXmi(WorkspaceNames.WorkspaceManagement, "dm:///_internal/workspaces");
            
            Assert.That(result.Value.Xmi.Contains("dm:///_internal/temp"), Is.True);
        }
        

        [Test]
        public async Task TestImportXmi()
        {
            var (workspaceLogic, scopeStorage) = DatenMeisterTests.GetDmInfrastructure();

            var extentController = new ExtentController(workspaceLogic, scopeStorage);
            var newExtent = new MofUriExtent(new InMemoryProvider(), "dm:///test", scopeStorage);
            workspaceLogic.AddExtent(workspaceLogic.GetDataWorkspace(), newExtent);

            var xmi = @"<xmi>
    <meta p2:id=""1ca6dd3b-be32-4cd4-896b-23506a23a81d"" __uri=""dm:///export"" xmlns:p2=""http://www.omg.org/spec/XMI/20131001"" xmlns=""http://datenmeister.net/"" />
    <item p2:type=""dm:///_internal/types/internal#IssueMeister.Issue"" p2:id=""2"" description=""Link to item does link to 404 and not to Item1"" state=""Closed"" name=""Detail Form - Bread Crumb1"" id=""2"" _toBeCleanedUp=""09/18/2022 12:22:44"" xmlns:p2=""http://www.omg.org/spec/XMI/20131001"" />
    <item p2:type=""dm:///_internal/types/internal#IssueMeister.Issue"" p2:id=""1"" _toBeCleanedUp=""09/18/2022 12:22:02"" id=""1"" name=""Moving up and Down"" state=""Closed"" description=""List Tables shall support the move up and move down of items&#xA;&#xA;fdsa"" xmlns:p2=""http://www.omg.org/spec/XMI/20131001"" />
</xmi>";

            await extentController.ImportXmi(
                WorkspaceNames.WorkspaceData,
                "dm:///test",
                new ExtentController.ImportXmiParams
                {
                    Xmi = xmi
                });

            Assert.That(newExtent.elements().Count(), Is.EqualTo(2));
            Assert.That(newExtent.elements().OfType<IElement>().First().getOrDefault<string>("state"),
                Is.EqualTo("Closed"));
            Assert.That(newExtent.elements().OfType<IElement>().ElementAt(1).getOrDefault<string>("name"),
                Is.EqualTo("Moving up and Down"));
        }
    }
}