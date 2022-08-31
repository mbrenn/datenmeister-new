﻿using System;
using System.Linq;
using System.Text.Json;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
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
    }
}