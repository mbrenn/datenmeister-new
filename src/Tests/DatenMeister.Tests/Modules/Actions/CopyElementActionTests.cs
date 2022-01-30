using System.Collections.Generic;
using System.Threading.Tasks;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime;
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
            var (source, target) = ActionSetTests.CreateExtents(actionLogic);

            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CopyElementsAction)
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
            Assert.That(target.GetUriResolver().Resolve("?target1", ResolveType.NoWorkspace), Is.Not.Null);
        }

        [Test]
        public async Task TestCopyingFromExtentToExtentWithDeletion()
        {
            var actionLogic = ActionSetTests.CreateActionLogic();
            var (source, target) = ActionSetTests.CreateExtents(actionLogic);

            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CopyElementsAction)
                .SetProperties(new Dictionary<string, object>
                {
                    [_DatenMeister._Actions._CopyElementsAction.sourcePath] = "dm:///source/",
                    [_DatenMeister._Actions._CopyElementsAction.targetPath] = "dm:///target/",
                    [_DatenMeister._Actions._CopyElementsAction.emptyTarget] = true
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
            Assert.That(target.GetUriResolver().Resolve("?target1", ResolveType.NoWorkspace), Is.Null);
        }

        [Test]
        public async Task TestMovingFromExtentToExtent()
        {
            var actionLogic = ActionSetTests.CreateActionLogic();
            var (source, target) = ActionSetTests.CreateExtents(actionLogic);

            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CopyElementsAction)
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
            var (source, target) = ActionSetTests.CreateExtents(actionLogic);

            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CopyElementsAction)
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
            var (source, target) = ActionSetTests.CreateExtents(actionLogic);

            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CopyElementsAction)
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
            var (source, target) = ActionSetTests.CreateExtents(actionLogic);

            // Check that packagedElement is the property
            var sourceElement = source.GetUriResolver().Resolve("?source1", ResolveType.NoWorkspace) as IObject;
            Assert.That(sourceElement, Is.Not.Null);
            Assert.That(DefaultClassifierHints.GetDefaultPackagePropertyName(sourceElement!),
                Is.EqualTo("packagedElement"));

            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CopyElementsAction)
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
            var (source, target) = ActionSetTests.CreateExtents(actionLogic);

            // Check that packagedElement is the property
            var sourceElement = source.GetUriResolver().Resolve("?source1", ResolveType.NoWorkspace) as IObject;
            Assert.That(sourceElement, Is.Not.Null);
            Assert.That(DefaultClassifierHints.GetDefaultPackagePropertyName(sourceElement!),
                Is.EqualTo("packagedElement"));

            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CopyElementsAction)
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
    }
}