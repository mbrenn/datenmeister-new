using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DependencyInjection;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.WebServer.Controller;
using NUnit.Framework;

namespace DatenMeister.Tests.Web
{
    [TestFixture]
    public class ElementControllerTests
    {
        [Test]
        public void TestWorkspaces()
        {
            var (dm, extent) = CreateExampleExtent();
            var workspaceLogic = dm.WorkspaceLogic;
            var scopeStorage = dm.ScopeStorage;

            var elementsController = new ElementsController(workspaceLogic, scopeStorage);
            var result = elementsController.GetComposites(null, null);
            
            // Check, that the workspaces are in... 
            Assert.That(result.Value.Any(x => x.name == "Data"));
            Assert.That(result.Value.Any(x => x.id == WorkspaceNames.UriExtentWorkspaces +  "#Data"));
            Assert.That(result.Value.Any(x => x.name == "Management"));
            
            dm.Dispose();
        }
        
        [Test]
        public void TestExtents()
        {
            var (dm, extent) = CreateExampleExtent();
            var workspaceLogic = dm.WorkspaceLogic;
            var scopeStorage = dm.ScopeStorage;

            var elementsController = new ElementsController(workspaceLogic, scopeStorage);
            var result = elementsController.GetComposites("Data", null);
            
            // Check, that the workspaces are in... 
            Assert.That(result.Value.Any(x => x.name == "Test Extent"));
            Assert.That(result.Value.Any(x => x.id == "dm:///temp"));
            
            dm.Dispose();
        }
        
        [Test]
        public void TestRootElements()
        {
            var (dm, extent) = CreateExampleExtent();
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
        public void TestSubElementsWithSingleChild()
        {
            var (dm, extent) = CreateExampleExtent();
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
        public void TestSubElementsWithMultipleChildren()
        {
            var (dm, extent) = CreateExampleExtent();
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

        public static (IDatenMeisterScope dm, IUriExtent extent) CreateExampleExtent()
        {
            var dm = DatenMeisterTests.GetDatenMeisterScope();
            var extentManager = new ExtentManager(dm.WorkspaceLogic, dm.ScopeStorage);

            var createdExtent = extentManager.CreateAndAddXmiExtent("dm:///temp", "./test.xmi");
            createdExtent.Extent.set("name", "Test Extent");

            var factory = new MofFactory(createdExtent.Extent);
            var item1 = factory.create(null).SetProperty("name", "item1");
            var item2 = factory.create(null).SetProperty("name", "item2");
            var item3 = factory.create(null).SetProperty("name", "item3");
            var item4 = factory.create(null).SetProperty("name", "item4");
            var item5 = factory.create(null).SetProperty("name", "item5");

            var propertyName = DefaultClassifierHints.GetDefaultPackagePropertyName(item1);
            
            item1.set(propertyName, item3);
            item2.set(propertyName, new[] { item4, item5 });
            createdExtent.Extent.elements().add(item1);
            createdExtent.Extent.elements().add(item2);
            
            return (dm, createdExtent.Extent);
        }
    }
}