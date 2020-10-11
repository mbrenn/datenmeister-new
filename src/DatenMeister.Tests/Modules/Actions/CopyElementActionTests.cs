using System.Collections.Generic;
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
        public void TestCopyingFromCollectionToCollection()
        {
            var actionLogic = ActionSetTests.CreateActionLogic();
            var (source, target) = CreateExtents(actionLogic);

            var action = (IElement) InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CopyElementsAction)
                .SetProperties(new Dictionary<string, object>
                {
                    [_DatenMeister._Actions._CopyElementsAction.sourcePath] = "dm:///source/",
                    [_DatenMeister._Actions._CopyElementsAction.targetPath] = "dm:///target/"
                });
            
            actionLogic.ExecuteAction(action);
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
            targetExtent.elements().add(targetElement1);

            return (sourceExtent, targetExtent);
        }
    }
}