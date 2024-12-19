using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime;
using DatenMeister.DependencyInjection;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.WebServer.Controller;
using NUnit.Framework;

namespace DatenMeister.Tests.Web
{
    [TestFixture]
    public class ElementControllerTests
    {
        public const string UriTemporaryExtent = "dm:///temp";

        [Test]
        public async Task TestWorkspaces()
        {
            var (dm, extent) = await CreateExampleExtent();
            var workspaceLogic = dm.WorkspaceLogic;
            var scopeStorage = dm.ScopeStorage;

            var elementsController = new ElementsController(workspaceLogic, scopeStorage);
            var result = elementsController.GetComposites(null, null);

            // Check, that the workspaces are in... 
            Assert.That(result.Value.Any(x => x.name == "Data"));
            Assert.That(result.Value.Any(x => x.id == "Data"));
            Assert.That(result.Value.Any(x => x.name == "Management"));

            dm.Dispose();
        }

        [Test]
        public async Task TestExtents()
        {
            var (dm, extent) = await CreateExampleExtent();
            var workspaceLogic = dm.WorkspaceLogic;
            var scopeStorage = dm.ScopeStorage;

            var elementsController = new ElementsController(workspaceLogic, scopeStorage);
            var result = elementsController.GetComposites("Data", null);

            // Check, that the workspaces are in... 
            Assert.That(result.Value.Any(x => x.name == "Test Extent"));
            Assert.That(result.Value.Any(x => x.id == "Data_dm:///temp"));

            dm.Dispose();

        }

        [Test]
        public async Task TestRootElements()
        {
            var (dm, extent) = await CreateExampleExtent();
            var workspaceLogic = dm.WorkspaceLogic;
            var scopeStorage = dm.ScopeStorage;

            var elementsController = new ElementsController(workspaceLogic, scopeStorage);
            var result = elementsController.GetComposites("Data", "dm:///temp");

            // Check, that the workspaces are in... 
            Assert.That(result.Value.Any(x => x.name == "item1"));
            Assert.That(result.Value.Any(x => x.name == "item2"));

            dm.Dispose();
        }

        [Test]
        public async Task TestSubElementsWithSingleChild()
        {
            var (dm, extent) = await CreateExampleExtent();
            var workspaceLogic = dm.WorkspaceLogic;
            var scopeStorage = dm.ScopeStorage;

            var elementsController = new ElementsController(workspaceLogic, scopeStorage);

            var firstElement =
                extent.elements()
                    .WhenPropertyContains("name", "item1").OfType<IElement>()
                    .FirstOrDefault();

            var result = elementsController.GetComposites(
                "Data", "dm:///temp#" + (firstElement as IHasId)?.Id);

            Assert.That(result.Value.Any(x => x.name == "item3"));
            dm.Dispose();
        }

        [Test]
        public async Task TestSubElementsWithMultipleChildren()
        {
            var (dm, extent) = await CreateExampleExtent();
            var workspaceLogic = dm.WorkspaceLogic;
            var scopeStorage = dm.ScopeStorage;

            var elementsController = new ElementsController(workspaceLogic, scopeStorage);

            var firstElement =
                extent.elements()
                    .WhenPropertyContains("name", "item2").OfType<IElement>()
                    .FirstOrDefault();

            var result = elementsController.GetComposites(
                "Data", "dm:///temp#" + (firstElement as IHasId)?.Id);

            Assert.That(result.Value.Any(x => x.name == "item4"));
            Assert.That(result.Value.Any(x => x.name == "item5"));
            dm.Dispose();
        }

        [Test]
        public async Task TestFindBySearchString()
        {
            var (dm, extent) = await CreateExampleExtent();
            var workspaceLogic = dm.WorkspaceLogic;
            var scopeStorage = dm.ScopeStorage;

            var elementsController = new ElementsControllerInternal(workspaceLogic, scopeStorage);

            // Nothing should be found
            var result = elementsController.FindBySearchString("fiuosdaiu");
            Assert.That(result.resultType, Is.EqualTo(ElementsController.FindBySearchStringResult.ResultTypeNone));

            result = elementsController.FindBySearchString(UriTemporaryExtent + "#item1");
            Assert.That(result.resultType, Is.EqualTo(ElementsController.FindBySearchStringResult.ResultTypeReference));
            Assert.That(result.reference!.id, Is.EqualTo("item1"));
            Assert.That(result.reference.uri, Is.EqualTo("dm:///temp#item1"));
            Assert.That(result.reference.extentUri, Is.EqualTo("dm:///temp"));
            Assert.That(result.reference.name, Is.EqualTo("item1"));
            Assert.That(result.reference.workspace, Is.EqualTo("Data"));

            result = elementsController.FindBySearchString(UriTemporaryExtent);
            Assert.That(result.resultType,
                Is.EqualTo(ElementsController.FindBySearchStringResult.ResultTypeReferenceExtent));
            Assert.That(result.reference!.uri, Is.EqualTo("dm:///temp"));
            Assert.That(result.reference.extentUri, Is.EqualTo("dm:///temp"));
            Assert.That(result.reference.workspace, Is.EqualTo("Data"));
            Assert.That(result.reference.name, Is.EqualTo("Test Extent"));

            dm.Dispose();
        }

        public static async Task<(IDatenMeisterScope, IUriExtent)> CreateExampleExtent()
        {            
            var dm = await DatenMeisterTests.GetDatenMeisterScope(
            true,
            DatenMeisterTests.GetIntegrationSettings());
            var extentManager = new ExtentManager(dm.WorkspaceLogic, dm.ScopeStorage);

            var createdExtent =
                await extentManager.CreateAndAddXmiExtent(
                    UriTemporaryExtent,
                    "./test_element.xmi");

            createdExtent.Extent!.set("name", "Test Extent");

            var factory = new MofFactory(createdExtent.Extent);
            var item1 = factory.create(null).SetProperty("name", "item1").SetProperty("value", 55).SetId("item1");
            var item2 = factory.create(null).SetProperty("name", "item2").SetProperty("value", 4).SetId("item2");
            var item3 = factory.create(null).SetProperty("name", "item3").SetProperty("value", 1).SetId("item3");
            var item4 = factory.create(null).SetProperty("name", "item4").SetId("item4");
            var item5 = factory.create(null).SetProperty("name", "item5").SetId("item5");

            var propertyName = DefaultClassifierHints.GetDefaultPackagePropertyName(item1);

            item1.set(propertyName, item3);
            item2.set(propertyName, new[] { item4, item5 });
            createdExtent.Extent.elements().add(item1);
            createdExtent.Extent.elements().add(item2);
            var item6 = factory.create(null).SetProperty("name", "item6").SetId("item6");
            var item7 = factory.create(null).SetProperty("name", "item7").SetId("item7");
            item4.set(propertyName, new[] { item6, item7 });

            var extent = createdExtent.Extent;

            return (dm, extent);

        }
    }
}