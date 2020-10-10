using System.Collections.Generic;
using System.Threading.Tasks;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Modules.Actions;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions
{
    [TestFixture]
    public class CopyElementActionTests
    {
        [Test]
        public async Task TestCopyingFromExtentToExtent()
        {
            var actionLogic = ActionSetTests.CreateActionLogic();
            var (source, target) = CreateExtents(actionLogic);

            var action = (IElement) InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CopyElementsAction)
                .SetProperties(new Dictionary<string, object>
                {
                    [_DatenMeister._Actions._CopyElementsAction.sourcePath] = "dm:///source/",
                    [_DatenMeister._Actions._CopyElementsAction.targetPath] = "dm:///target/"
                });

            await actionLogic.ExecuteAction(action);

            Assert.That(source.GetUriResolver().Resolve("?source1", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(source.GetUriResolver().Resolve("?source1::source1.1", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(source.GetUriResolver().Resolve("?source1::source1.2", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(source.GetUriResolver().Resolve("?source2", ResolveType.NoWorkspace), Is.Not.Null);

            Assert.That(target.GetUriResolver().Resolve("?source1", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(target.GetUriResolver().Resolve("?source1::source1.1", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(target.GetUriResolver().Resolve("?source1::source1.2", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(target.GetUriResolver().Resolve("?source2", ResolveType.NoWorkspace), Is.Not.Null);
        }

        [Test]
        public async Task TestMovingFromExtentToExtent()
        {
            var actionLogic = ActionSetTests.CreateActionLogic();
            var (source, target) = CreateExtents(actionLogic);

            var action = (IElement) InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CopyElementsAction)
                .SetProperties(new Dictionary<string, object>
                {
                    [_DatenMeister._Actions._CopyElementsAction.sourcePath] = "dm:///source/",
                    [_DatenMeister._Actions._CopyElementsAction.targetPath] = "dm:///target/",
                    [_DatenMeister._Actions._CopyElementsAction.moveOnly] = true
                });

            await actionLogic.ExecuteAction(action);

            Assert.That(source.GetUriResolver().Resolve("?source1", ResolveType.NoWorkspace), Is.Null);
            Assert.That(source.GetUriResolver().Resolve("?source1::source1.1", ResolveType.NoWorkspace), Is.Null);
            Assert.That(source.GetUriResolver().Resolve("?source1::source1.2", ResolveType.NoWorkspace), Is.Null);
            Assert.That(source.GetUriResolver().Resolve("?source2", ResolveType.NoWorkspace), Is.Null);

            Assert.That(target.GetUriResolver().Resolve("?source1", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(target.GetUriResolver().Resolve("?source1::source1.1", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(target.GetUriResolver().Resolve("?source1::source1.2", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(target.GetUriResolver().Resolve("?source2", ResolveType.NoWorkspace), Is.Not.Null);
        }

        [Test]
        public async Task TestCopyingFromElementToExtent()
        {
            var actionLogic = ActionSetTests.CreateActionLogic();
            var (source, target) = CreateExtents(actionLogic);

            var action = (IElement) InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CopyElementsAction)
                .SetProperties(new Dictionary<string, object>
                {
                    [_DatenMeister._Actions._CopyElementsAction.sourcePath] = "dm:///source/?fn=source1",
                    [_DatenMeister._Actions._CopyElementsAction.targetPath] = "dm:///target/"
                });

            await actionLogic.ExecuteAction(action);

            Assert.That(source.GetUriResolver().Resolve("?source1", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(source.GetUriResolver().Resolve("?source1::source1.1", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(source.GetUriResolver().Resolve("?source1::source1.2", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(source.GetUriResolver().Resolve("?source2", ResolveType.NoWorkspace), Is.Not.Null);

            Assert.That(target.GetUriResolver().Resolve("?source1", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(target.GetUriResolver().Resolve("?source1::source1.1", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(target.GetUriResolver().Resolve("?source1::source1.2", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(target.GetUriResolver().Resolve("?source2", ResolveType.NoWorkspace), Is.Null);
        }

        [Test]
        public async Task TestCopyingFromElementToElement()
        {
            var actionLogic = ActionSetTests.CreateActionLogic();
            var (source, target) = CreateExtents(actionLogic);

            var action = (IElement) InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CopyElementsAction)
                .SetProperties(new Dictionary<string, object>
                {
                    [_DatenMeister._Actions._CopyElementsAction.sourcePath] = "dm:///source/?fn=source1",
                    [_DatenMeister._Actions._CopyElementsAction.targetPath] = "dm:///target/?fn=target1"
                });

            await actionLogic.ExecuteAction(action);

            Assert.That(source.GetUriResolver().Resolve("?source1", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(source.GetUriResolver().Resolve("?source1::source1.1", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(source.GetUriResolver().Resolve("?source1::source1.2", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(source.GetUriResolver().Resolve("?source2", ResolveType.NoWorkspace), Is.Not.Null);

            Assert.That(target.GetUriResolver().Resolve("?target1::source1", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(target.GetUriResolver().Resolve("?target1::source1::source1.1", ResolveType.NoWorkspace),
                Is.Not.Null);
            Assert.That(target.GetUriResolver().Resolve("?target1::source1::source1.2", ResolveType.NoWorkspace),
                Is.Not.Null);
            Assert.That(target.GetUriResolver().Resolve("?target1::source2", ResolveType.NoWorkspace), Is.Null);
        }

        [Test]
        public async Task TestCopyingFromCollectionToCollection()
        {
            var actionLogic = ActionSetTests.CreateActionLogic();
            var (source, target) = CreateExtents(actionLogic);

            // Check that packagedElement is the property
            var sourceElement = source.GetUriResolver().Resolve("?source1", ResolveType.NoWorkspace) as IObject;
            Assert.That(sourceElement, Is.Not.Null);
            Assert.That(DefaultClassifierHints.GetDefaultPackagePropertyName(sourceElement),
                Is.EqualTo("packagedElement"));

            var action = (IElement) InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CopyElementsAction)
                .SetProperties(new Dictionary<string, object>
                {
                    [_DatenMeister._Actions._CopyElementsAction.sourcePath] =
                        "dm:///source/?fn=source1&prop=packagedElement",
                    [_DatenMeister._Actions._CopyElementsAction.targetPath] =
                        "dm:///target/?fn=target1&prop=packagedElement"
                });

            await actionLogic.ExecuteAction(action);

            Assert.That(source.GetUriResolver().Resolve("?source1", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(source.GetUriResolver().Resolve("?source1::source1.1", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(source.GetUriResolver().Resolve("?source1::source1.2", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(source.GetUriResolver().Resolve("?source2", ResolveType.NoWorkspace), Is.Not.Null);

            Assert.That(target.GetUriResolver().Resolve("?target1::source1", ResolveType.NoWorkspace), Is.Null);
            Assert.That(target.GetUriResolver().Resolve("?target1::source1.1", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(target.GetUriResolver().Resolve("?target1::source1.2", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(target.GetUriResolver().Resolve("?target1::source2", ResolveType.NoWorkspace), Is.Null);
        }

        [Test]
        public async Task TestCopyingFromElementToCollection()
        {
            var actionLogic = ActionSetTests.CreateActionLogic();
            var (source, target) = CreateExtents(actionLogic);

            // Check that packagedElement is the property
            var sourceElement = source.GetUriResolver().Resolve("?source1", ResolveType.NoWorkspace) as IObject;
            Assert.That(sourceElement, Is.Not.Null);
            Assert.That(DefaultClassifierHints.GetDefaultPackagePropertyName(sourceElement),
                Is.EqualTo("packagedElement"));

            var action = (IElement) InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CopyElementsAction)
                .SetProperties(new Dictionary<string, object>
                {
                    [_DatenMeister._Actions._CopyElementsAction.sourcePath] = "dm:///source/?fn=source1",
                    [_DatenMeister._Actions._CopyElementsAction.targetPath] =
                        "dm:///target/?fn=target1&prop=packagedElement"
                });

            await actionLogic.ExecuteAction(action);

            Assert.That(source.GetUriResolver().Resolve("?source1", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(source.GetUriResolver().Resolve("?source1::source1.1", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(source.GetUriResolver().Resolve("?source1::source1.2", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(source.GetUriResolver().Resolve("?source2", ResolveType.NoWorkspace), Is.Not.Null);

            Assert.That(target.GetUriResolver().Resolve("?target1::source1", ResolveType.NoWorkspace), Is.Not.Null);
            Assert.That(target.GetUriResolver().Resolve("?target1::source1::source1.1", ResolveType.NoWorkspace),
                Is.Not.Null);
            Assert.That(target.GetUriResolver().Resolve("?target1::source1::source1.2", ResolveType.NoWorkspace),
                Is.Not.Null);
            Assert.That(target.GetUriResolver().Resolve("?target1::source2", ResolveType.NoWorkspace), Is.Null);
        }

        public (IUriExtent source, IUriExtent target) CreateExtents(ActionLogic actionLogic)
        {
            var workspaceLogic = actionLogic.WorkspaceLogic;
            var scopeStorage = actionLogic.ScopeStorage;

            var sourceProvider = new InMemoryProvider();
            var targetProvider = new InMemoryProvider();
            var sourceExtent = new MofUriExtent(sourceProvider, "dm:///source/");
            var targetExtent = new MofUriExtent(targetProvider, "dm:///target/");
            var sourceFactory = new MofFactory(sourceExtent);
            var targetFactory = new MofFactory(targetExtent);

            workspaceLogic.AddExtent(workspaceLogic.GetDataWorkspace(), sourceExtent);
            workspaceLogic.AddExtent(workspaceLogic.GetDataWorkspace(), targetExtent);

            var sourceElement1 = sourceFactory.create(null)
                .SetProperties(new Dictionary<string, object> {["name"] = "source1"});
            var sourceElement1_1 = sourceFactory.create(null)
                .SetProperties(new Dictionary<string, object> {["name"] = "source1.1"});
            var sourceElement1_2 = sourceFactory.create(null)
                .SetProperties(new Dictionary<string, object> {["name"] = "source1.2"});
            var sourceElement1_3 = sourceFactory.create(null)
                .SetProperties(new Dictionary<string, object> {["name"] = "source1.3"});
            var sourceElement1_4 = sourceFactory.create(null)
                .SetProperties(new Dictionary<string, object> {["name"] = "source1.4"});

            sourceElement1.set(DefaultClassifierHints.GetDefaultPackagePropertyName(sourceElement1),
                new[] {sourceElement1_1, sourceElement1_2, sourceElement1_3, sourceElement1_4});

            var sourceElement2 = sourceFactory.create(null)
                .SetProperties(new Dictionary<string, object> {["name"] = "source2"});

            sourceExtent.elements().add(sourceElement1);
            sourceExtent.elements().add(sourceElement2);

            var targetElement1 = targetFactory.create(null)
                .SetProperties(new Dictionary<string, object> {["name"] = "target1"});
            var targetElement1_1 = sourceFactory.create(null)
                .SetProperties(new Dictionary<string, object> {["name"] = "target1.1"});
            targetElement1.set(DefaultClassifierHints.GetDefaultPackagePropertyName(targetElement1),
                new[] {targetElement1_1});
            targetExtent.elements().add(targetElement1);


            return (sourceExtent, targetExtent);
        }
    }
}
