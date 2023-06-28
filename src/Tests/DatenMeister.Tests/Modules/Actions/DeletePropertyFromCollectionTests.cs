using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using DatenMeister.Actions;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions
{
    [TestFixture]
    public class DeletePropertyFromCollectionTests
    {
        [Test]
        public async Task TestDeletePropertyFromExtent()
        {
            var actionLogic = ActionSetTests.CreateActionLogic();
            var testExtent = CreateTestExtent(actionLogic);
            
            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__DeletePropertyFromCollectionAction)
                .SetProperties(new Dictionary<string, object>
                {
                    [_DatenMeister._Actions._DeletePropertyFromCollectionAction.collectionUrl] = "dm:///source/",
                    [_DatenMeister._Actions._DeletePropertyFromCollectionAction.propertyName] = "test"
                });
            
            await actionLogic.ExecuteAction(action);

            var source1 = testExtent.element("dm:///source/#source1");
            var source2 = testExtent.element("dm:///source/#source2");
            var source1_1 = testExtent.element("dm:///source/#source1.1");
            var source1_2 = testExtent.element("dm:///source/#source1.2");
            
            Assert.That(source1, Is.Not.Null);
            Assert.That(source1_1, Is.Not.Null);
            Assert.That(source1_2, Is.Not.Null);
            Assert.That(source2, Is.Not.Null);
            
            Assert.That(source1.getOrDefault<string>("test"), Is.EqualTo(null));
            Assert.That(source1_1.getOrDefault<string>("test"), Is.EqualTo("test"));
            Assert.That(source1_2.getOrDefault<string>("test"), Is.EqualTo("test"));
            Assert.That(source2.getOrDefault<string>("test"), Is.EqualTo(null));
        }
        
        [Test]
        public async Task TestDeletePropertyFromPropertyCollection()
        {
            var actionLogic = ActionSetTests.CreateActionLogic();
            var testExtent = CreateTestExtent(actionLogic);
            
            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__DeletePropertyFromCollectionAction)
                .SetProperties(new Dictionary<string, object>
                {
                    [_DatenMeister._Actions._DeletePropertyFromCollectionAction.collectionUrl] = "dm:///source/?fn=source1&prop=packagedElement",
                    [_DatenMeister._Actions._DeletePropertyFromCollectionAction.propertyName] = "test"
                });
            
            await actionLogic.ExecuteAction(action);

            var source1 = testExtent.element("dm:///source/#source1");
            var source2 = testExtent.element("dm:///source/#source2");
            var source1_1 = testExtent.element("dm:///source/#source1.1");
            var source1_2 = testExtent.element("dm:///source/#source1.2");
            
            Assert.That(source1, Is.Not.Null);
            Assert.That(source1_1, Is.Not.Null);
            Assert.That(source1_2, Is.Not.Null);
            Assert.That(source2, Is.Not.Null);
            
            Assert.That(source1.getOrDefault<string>("test"), Is.EqualTo("test"));
            Assert.That(source1_1.getOrDefault<string>("test"), Is.EqualTo(null));
            Assert.That(source1_2.getOrDefault<string>("test"), Is.EqualTo(null));
            Assert.That(source2.getOrDefault<string>("test"), Is.EqualTo("test"));
        }

        [Test]
        public async Task TestDeletePropertyFromPropertyCollectionWithMetaClass()
        {
            var actionLogic = ActionSetTests.CreateActionLogic();
            var testExtent = CreateTestExtent(actionLogic);

            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__DeletePropertyFromCollectionAction)
                .SetProperties(new Dictionary<string, object>
                {
                    [_DatenMeister._Actions._DeletePropertyFromCollectionAction.collectionUrl]
                        = "dm:///source/?fn=source1&prop=packagedElement&metaclass="
                          + HttpUtility.UrlEncode(_DatenMeister.TheOne.Actions.__EchoAction.Uri),
                    [_DatenMeister._Actions._DeletePropertyFromCollectionAction.propertyName] = "test"
                });

            await actionLogic.ExecuteAction(action);

            var source1 = testExtent.element("dm:///source/#source1");
            var source2 = testExtent.element("dm:///source/#source2");
            var source1_1 = testExtent.element("dm:///source/#source1.1");
            var source1_2 = testExtent.element("dm:///source/#source1.2");

            Assert.That(source1, Is.Not.Null);
            Assert.That(source1_1, Is.Not.Null);
            Assert.That(source1_2, Is.Not.Null);
            Assert.That(source2, Is.Not.Null);

            Assert.That(source1.getOrDefault<string>("test"), Is.EqualTo("test"));
            Assert.That(source1_1.getOrDefault<string>("test"), Is.EqualTo(null));
            Assert.That(source1_2.getOrDefault<string>("test"), Is.EqualTo("test"));
            Assert.That(source2.getOrDefault<string>("test"), Is.EqualTo("test"));
        }

        public IUriExtent CreateTestExtent(ActionLogic actionLogic)
        {
            var workspaceLogic = actionLogic.WorkspaceLogic;

            var sourceProvider = new InMemoryProvider();
            var sourceExtent = new MofUriExtent(sourceProvider, "dm:///source/", actionLogic.ScopeStorage);
            
            workspaceLogic.AddExtent(workspaceLogic.GetDataWorkspace(), sourceExtent);
            
            var sourceFactory = new MofFactory(sourceExtent);
            var sourceElement1 = sourceFactory.create(_DatenMeister.TheOne.CommonTypes.Default.__Package)
                .SetProperties(new Dictionary<string, object> {["name"] = "source1", ["test"] = "test"})
                .SetId("source1");
            var sourceElement1_1 = sourceFactory.create(_DatenMeister.TheOne.Actions.__EchoAction)
                .SetProperties(new Dictionary<string, object> {["name"] = "source1.1", ["test"] = "test"})
                .SetId("source1.1");
            var sourceElement1_2 = sourceFactory.create(_DatenMeister.TheOne.Actions.__CommandExecutionAction)
                .SetProperties(new Dictionary<string, object> {["name"] = "source1.2", ["test"] = "test"})
                .SetId("source1.2");
            var sourceElement2 = sourceFactory.create(_DatenMeister.TheOne.Actions.__EchoAction)
                .SetProperties(new Dictionary<string, object> {["name"] = "source2", ["test"] = "test"})
                .SetId("source2");
            
            sourceElement1.set(_UML._Packages._Package.packagedElement,
                new[] {sourceElement1_1, sourceElement1_2});
            sourceExtent.elements().add(sourceElement1);
            sourceExtent.elements().add(sourceElement2);

            return sourceExtent;
        }
    }
}