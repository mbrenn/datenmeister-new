using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.WebServer.Controller;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace DatenMeister.Tests.Web
{
    [TestFixture]
    public class ItemsControllerTests
    {
        [Test]
        public void TestParentItems()
        {
            var dm = DatenMeisterTests.GetDatenMeisterScope();
            var extentManager = new ExtentManager(dm.WorkspaceLogic, dm.ScopeStorage);

            var createdExtent = extentManager.CreateAndAddXmiExtent("dm:///temp", "./test.xmi");
            createdExtent.Extent!.set("name", "Test Extent");

            var factory = new MofFactory(createdExtent.Extent);
            var item1 = factory.create(null).SetProperty("name", "item1").SetId("1");
            var item2 = factory.create(null).SetProperty("name", "item2").SetId("2");
            var item3 = factory.create(null).SetProperty("name", "item3").SetId("3");
            var item4 = factory.create(null).SetProperty("name", "item4").SetId("4");
            var item5 = factory.create(null).SetProperty("name", "item5").SetId("5");
            var item6 = factory.create(null).SetProperty("name", "item6").SetId("6");
            var item7 = factory.create(null).SetProperty("name", "item7").SetId("7");

            var propertyName = DefaultClassifierHints.GetDefaultPackagePropertyName(item1);
            
            item1.set(propertyName, item3);
            item2.set(propertyName, new[] { item4, item5 });
            createdExtent.Extent.elements().add(item1);
            createdExtent.Extent.elements().add(item2);
            
            item4.set(propertyName, new[] { item6, item7 });
        }

        [Test]
        public void TestRootElements()
        {
            var (dm, example) = ElementControllerTests.CreateExampleExtent();

            var itemsController = new ItemsController(dm.WorkspaceLogic, dm.ScopeStorage);
            var rootElements = itemsController.GetRootElements(WorkspaceNames.WorkspaceData, ElementControllerTests.UriTemporaryExtent).Value?.ToString()
                               ?? throw new InvalidOperationException("Should not happen");
            Assert.That(rootElements, Is.Not.Null);

            object elements = JsonConvert.DeserializeObject(rootElements);
            Assert.That(elements, Is.Not.Null);

            var asEnumeration = (elements as IEnumerable<object>)?.ToArray();
            Assert.That(asEnumeration, Is.Not.Null);

            var found = false;
            foreach (var item in asEnumeration!.OfType<JObject>())
            {
                if ((item.GetValue("v") as JObject)?.GetValue("name")?.ToString() == "name1")
                {
                    found = true;
                }
            }

            Assert.That(found, Is.Not.Null);
        }
    }
}