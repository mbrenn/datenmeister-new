using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DependencyInjection;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Modules.ZipCodeExample;
using DatenMeister.Provider.ExtentManagement;
using DatenMeister.WebServer.Controller;
using DatenMeister.WebServer.Library.Helper;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace DatenMeister.Tests.Web
{
    [TestFixture]
    public class ItemsControllerTests
    {
        [Test]
        public async Task TestGetProperty()
        {
            var (dm, extent) = await ElementControllerTests.CreateExampleExtent();
            var itemsController = new ItemsControllerInternal(dm.WorkspaceLogic, dm.ScopeStorage);
            var found = itemsController.GetPropertyInternal(
                "Data",
                ElementControllerTests.UriTemporaryExtent + "#item1", "name");
            Assert.That(found, Is.Not.Null);
            Assert.That(found.ToString(), Is.EqualTo("item1"));

            var item1 = extent.element("#item1");
            var item2 = extent.element("#item2");
            var item3 = extent.element("#item3");
            Assert.That(item3, Is.Not.Null);

            item3.set("children", new[] {item1, item2});

            var foundChildren = itemsController.GetPropertyInternal(
                "Data",
                ElementControllerTests.UriTemporaryExtent + "#item3", "children");

            Assert.That(foundChildren, Is.Not.Null);
            Assert.That(foundChildren, Is.InstanceOf<IReflectiveCollection>());
            var asReflectiveCollection = foundChildren as IReflectiveCollection;
            Assert.That(
                asReflectiveCollection!
                    .OfType<IElement>()
                    .Any(x => x.getOrDefault<string>("name") == "item1"),
                Is.True);

            dm.Dispose();
        }

        [Test]
        public async Task TestUnsetProperty()
        {
            var (dm, example) = await ElementControllerTests.CreateExampleExtent();
            var itemsController = new ItemsController(dm.WorkspaceLogic, dm.ScopeStorage);

            var item1 = example.element("#item1");
            Assert.That(item1, Is.Not.Null);

            Assert.That(item1.getOrDefault<string>("name"), Is.EqualTo("item1"));

            itemsController.UnsetProperty(
                WorkspaceNames.WorkspaceData,
                ElementControllerTests.UriTemporaryExtent + "#item1",
                new ItemsController.UnsetPropertyParams {Property = "name"});

            Assert.That(item1.getOrDefault<string>("name"), Is.Null);

            dm.Dispose();
        }

        [Test]
        public async Task TestGetRootElements()
        {
            var (dm, extent) = await ElementControllerTests.CreateExampleExtent();

            var itemsController = new ItemsController(dm.WorkspaceLogic, dm.ScopeStorage);
            var rootElements = itemsController
                                   .GetRootElements(
                                       WorkspaceNames.WorkspaceData,
                                       ElementControllerTests.UriTemporaryExtent).Value?.ToString()
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

            dm.Dispose();
        }

        [Test]
        public async Task TestGetRootElementsOrderBy()
        {
            var (dm, extent) = await CreateExampleExtentForSorting();

            var itemsController = new ItemsControllerInternal(dm.WorkspaceLogic, dm.ScopeStorage);
            var elements = itemsController
                                   .GetRootElementsInternal(
                                       WorkspaceNames.WorkspaceData,
                                       ElementControllerTests.UriTemporaryExtent,
                                       null,
                                       new QueryFilterParameter()
                                       {
                                           OrderBy = "value"
                                       })
                
                               ?? throw new InvalidOperationException("Should not happen");

            Assert.That(elements.Count(), Is.GreaterThan(0));
            Assert.That(elements[0].getOrDefault<int>("value"), Is.EqualTo(1));
            Assert.That(elements[1].getOrDefault<int>("value"), Is.EqualTo(4));
            Assert.That(elements[2].getOrDefault<int>("value"), Is.EqualTo(55));

            dm.Dispose();
        }

        [Test]
        public async Task TestGetRootElementsOrderByDescending()
        {
            var (dm, extent) = await CreateExampleExtentForSorting();

            var itemsController = new ItemsControllerInternal(dm.WorkspaceLogic, dm.ScopeStorage);
            var elements = itemsController
                                   .GetRootElementsInternal(
                                       WorkspaceNames.WorkspaceData,
                                       ElementControllerTests.UriTemporaryExtent,
                                       null,
                                       new QueryFilterParameter()
                                       {
                                           OrderBy = "value",
                                           OrderByDescending = true
                                       })

                               ?? throw new InvalidOperationException("Should not happen");

            Assert.That(elements.Count(), Is.GreaterThan(0));
            Assert.That(elements[0].getOrDefault<int>("value"), Is.EqualTo( 55));
            Assert.That(elements[1].getOrDefault<int>("value"), Is.EqualTo(4));
            Assert.That(elements[2].getOrDefault<int>("value"), Is.EqualTo(1));

            dm.Dispose();
        }


        [Test]
        public async Task TestGetRootElementsWithMetaClass()
        {
            var (dm, zipExtent, formsController, x) = await FormControllerTests.CreateZipExtent();

            var itemsController = new ItemsController(x.WorkspaceLogic, x.ScopeStorage);
            var rootElements = itemsController.GetRootElements(
                                       WorkspaceNames.WorkspaceData,
                                       MvcUrlEncoder.EncodePath(
                                           zipExtent.contextURI()
                                           + "?metaclass=" +
                                           MvcUrlEncoder.EncodePath(
                                               "dm:///_internal/types/internal#DatenMeister.Modules.ZipCodeExample.Model.ZipCode")))
                                   .Value?.ToString()
                               ?? throw new InvalidOperationException("Should not happen");
            Assert.That(rootElements, Is.Not.Null);

            var elements = JsonConvert.DeserializeObject(rootElements);
            Assert.That(elements, Is.Not.Null);
            
            dm.Dispose();
        }

        [Test]
        public async Task TestDeleteItems()
        {
            var (dm, example) = await ElementControllerTests.CreateExampleExtent();

            var itemsController = new ItemsController(dm.WorkspaceLogic, dm.ScopeStorage);
            var rootElements = example.elements().ToList();
            Assert.That(rootElements, Is.Not.Null);

            var count = rootElements.Count;
            var second = rootElements.ElementAtOrDefault(1) as IObject;
            Assert.That(second, Is.Not.Null);

            itemsController.DeleteItem(WorkspaceNames.WorkspaceData, second.GetUri()!);

            var newRootElements = example.elements().ToList();
            Assert.That(newRootElements.Count, Is.EqualTo(count - 1));

            dm.Dispose();
        }

        [Test]
        public async Task TestSetMetaClass()
        {
            var (dm, example) = await ElementControllerTests.CreateExampleExtent();

            var itemsController = new ItemsController(dm.WorkspaceLogic, dm.ScopeStorage);
            var rootElements = example.elements().ToList();
            Assert.That(rootElements, Is.Not.Null);

            var first = rootElements.ElementAtOrDefault(0) as IElement;

            Assert.That(first, Is.Not.Null);

            var metaClass = first.getMetaClass();
            Assert.That(metaClass, Is.Null);

            var newMetaClass = "dm:///types#metaclass";
            itemsController.SetMetaClass(
                "Data",
                first.GetUri() ?? throw new InvalidOperationException("Uri is not set"),
                new ItemsController.SetMetaClassParams
                {
                    metaClass = newMetaClass
                });

            Assert.That(first.getMetaClass()?.@equals(new MofObjectShadow(newMetaClass)), Is.True);

            dm.Dispose();
        }

        [Test]
        public async Task TestAddReferenceToCollection()
        {
            var (dm, example) = await ElementControllerTests.CreateExampleExtent();

            var itemsController = new ItemsController(dm.WorkspaceLogic, dm.ScopeStorage);

            var rootElements = example.elements().ToList();
            Assert.That(rootElements, Is.Not.Null);
            var first = rootElements.ElementAtOrDefault(0) as IElement;
            Assert.That(first, Is.Not.Null);

            var list = first.getOrDefault<IReflectiveCollection>("reference");
            Assert.That(list == null || list.size() == 0, Is.True);

            itemsController.AddReferenceToCollection(
                "Data",
                ElementControllerTests.UriTemporaryExtent + "#item1",
                new ItemsController.AddReferenceToCollectionParams
                {
                    Property = "reference",
                    WorkspaceId = "Data",
                    ReferenceUri = ElementControllerTests.UriTemporaryExtent + "#item2"
                });

            list = first.getOrDefault<IReflectiveCollection>("reference");
            Assert.That(list.size() == 1, Is.True);

            itemsController.AddReferenceToCollection(
                "Data",
                ElementControllerTests.UriTemporaryExtent + "#item1",
                new ItemsController.AddReferenceToCollectionParams
                {
                    Property = "reference",
                    ReferenceUri = ElementControllerTests.UriTemporaryExtent + "#item3"
                });

            list = first.getOrDefault<IReflectiveCollection>("reference");
            Assert.That(list.size() == 2, Is.True);

            dm.Dispose();
        }

        [Test]
        public async Task TestSetPropertyReference()
        {
            var (dm, example) = await ElementControllerTests.CreateExampleExtent();

            var itemsController = new ItemsController(dm.WorkspaceLogic, dm.ScopeStorage);

            var rootElements = example.elements().ToList();
            Assert.That(rootElements, Is.Not.Null);
            var first = rootElements.ElementAtOrDefault(0) as IElement;
            Assert.That(first, Is.Not.Null);
            Assert.That(first.getOrDefault<string>("name"), Is.EqualTo("item1"));

            var list = first.getOrDefault<IReflectiveCollection>("reference");
            Assert.That(list == null || list.size() == 0, Is.True);

            itemsController.SetPropertyReference(
                "Data",
                ElementControllerTests.UriTemporaryExtent + "#item1",
                new ItemsController.SetPropertyReferenceParams
                {
                    Property = "reference",
                    WorkspaceId = "Data",
                    ReferenceUri = ElementControllerTests.UriTemporaryExtent + "#item2"
                });

            var asElement = first.getOrDefault<IElement>("reference");
            Assert.That(asElement, Is.Not.Null);
            Assert.That(asElement.getOrDefault<string>("name"), Is.EqualTo("item2"));
        }

        [Test]
        public async Task TestRemoveReferenceToCollection()
        {
            var (dm, example) = await ElementControllerTests.CreateExampleExtent();

            var itemsController = new ItemsController(dm.WorkspaceLogic, dm.ScopeStorage);

            var rootElements = example.elements().ToList();
            var first = rootElements.ElementAtOrDefault(0) as IElement;
            Assert.That(first, Is.Not.Null);

            var list = first.get<IReflectiveCollection>("reference");
            var second = example.element("#item2");
            Assert.That(second, Is.Not.Null);
            list.add(second);

            list = first.get<IReflectiveCollection>("reference");
            Assert.That(list.size() == 1, Is.True);

            itemsController.RemoveReferenceToCollection(
                "Data",
                ElementControllerTests.UriTemporaryExtent + "#item1",
                new ItemsController.RemoveReferenceToCollectionParams
                {
                    Property = "reference",
                    WorkspaceId = "Data",
                    ReferenceUri = ElementControllerTests.UriTemporaryExtent + "#item2"
                });

            list = first.get<IReflectiveCollection>("reference");
            Assert.That(list.size() == 0, Is.True);

            dm.Dispose();
        }

        [Test]
        public async Task TestGetContainer()
        {
            var (dm, extent) = await ElementControllerTests.CreateExampleExtent();

            var item1 = extent.element("item1");
            var item4 = extent.element("item4");
            var item7 = extent.element("item7");

            Assert.That(item1, Is.Not.Null);
            Assert.That(item4, Is.Not.Null);
            Assert.That(item7, Is.Not.Null);

            var itemsController = new ItemsController(dm.WorkspaceLogic, dm.ScopeStorage);

            // Get the containers of the root items. This element should be the extent and the workspace
            var container1 = itemsController.GetContainer("Data", "dm:///temp#item1")?.Value;
            Assert.That(container1.Count, Is.EqualTo(2));
            Assert.That(container1[0].name, Is.EqualTo("Test Extent"));
            Assert.That(container1[0].workspace, Is.EqualTo("Data"));
            Assert.That(container1[0].uri, Is.EqualTo("dm:///temp"));
            
            Assert.That(container1[1].name, Is.EqualTo("Data"));
            Assert.That(container1[1].workspace, Is.EqualTo("Management"));
            Assert.That(container1[1].uri, Is.EqualTo(ExtentManagementHelper.GetUrlOfWorkspace("Data")));

            // Get the containers of the root items. This element should be the item2, extent and the workspace
            var container4 = itemsController.GetContainer("Data", "dm:///temp#item4")?.Value;
            
            Assert.That(container4.Count, Is.EqualTo(3));
            Assert.That(container4[0].name, Is.EqualTo("item2"));
            Assert.That(container4[0].workspace, Is.EqualTo("Data"));
            
            Assert.That(container4[1].name, Is.EqualTo("Test Extent"));
            Assert.That(container4[1].workspace, Is.EqualTo("Data"));
            
            Assert.That(container4[2].name, Is.EqualTo("Data"));
            Assert.That(container4[2].workspace, Is.EqualTo("Management"));

            // Get the containers of the root items. This element should be the item4, item2, extent and the workspace
            var container7 = itemsController.GetContainer("Data", "dm:///temp#item7")?.Value;

            Assert.That(container7.Count, Is.EqualTo(4));
            Assert.That(container7[0].name, Is.EqualTo("item4"));
            Assert.That(container7[0].workspace, Is.EqualTo("Data"));
            
            Assert.That(container7[1].name, Is.EqualTo("item2"));
            Assert.That(container7[1].workspace, Is.EqualTo("Data"));
            
            Assert.That(container7[2].name, Is.EqualTo("Test Extent"));
            Assert.That(container7[2].workspace, Is.EqualTo("Data"));
            
            Assert.That(container7[3].name, Is.EqualTo("Data"));
            Assert.That(container7[3].workspace, Is.EqualTo("Management"));

            dm.Dispose();
        }
        

        [Test]
        public async Task TestExportXmiOfManagement()
        {
            var dm = await DatenMeisterTests.GetDatenMeisterScope();
            
            var itemsController = new ItemsController(dm.WorkspaceLogic, dm.ScopeStorage);
            var result = itemsController.ExportXmi(WorkspaceNames.WorkspaceManagement, "dm:///_internal/workspaces#Data");
            
            Assert.That(result.Value.Xmi.Contains("dm:///_internal/temp"), Is.True);
        }

        public static async Task<(IDatenMeisterScope, IUriExtent)> CreateExampleExtentForSorting()
        {
            var dm = await DatenMeisterTests.GetDatenMeisterScope(
                true,
                DatenMeisterTests.GetIntegrationSettings());
            var extentManager = new ExtentManager(dm.WorkspaceLogic, dm.ScopeStorage);

            var createdExtent = await extentManager.CreateAndAddXmiExtent(
                ElementControllerTests.UriTemporaryExtent, "./test_element.xmi");
            createdExtent.Extent!.set("name", "Test Extent");

            var factory = new MofFactory(createdExtent.Extent);
            var item1 = factory.create(null).SetProperty("name", "item1").SetProperty("value", 55).SetId("item1");
            var item2 = factory.create(null).SetProperty("name", "item2").SetProperty("value", 4).SetId("item2");
            var item3 = factory.create(null).SetProperty("name", "item3").SetProperty("value", 1).SetId("item3");
            var item4 = factory.create(null).SetProperty("name", "item4").SetId("item4");
            var item5 = factory.create(null).SetProperty("name", "item5").SetId("item5");

            createdExtent.Extent.elements().add(item1);
            createdExtent.Extent.elements().add(item2);
            createdExtent.Extent.elements().add(item3);
            createdExtent.Extent.elements().add(item4);
            createdExtent.Extent.elements().add(item5);

            var extent = createdExtent.Extent;
            return (dm, extent);
        }
    }
}