using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DependencyInjection;
using DatenMeister.Integration;
using DatenMeister.Modules.Actions;
using DatenMeister.Modules.Actions.ActionHandler;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions
{
    [TestFixture]
    public class ActionSetTests
    {
        [Test]
        public void TestActionSetExecution()
        {
            var actionLogic = CreateActionLogic();

            var actionSet = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__ActionSet);
            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__LoggingWriterAction);
            if (actionSet == null || action == null) throw new InvalidOperationException("null");
            
            action.set(_DatenMeister._Actions._LoggingWriterAction.message, "zyx");
            actionSet.get<IReflectiveCollection>(_DatenMeister._Actions._ActionSet.action).add(action);

            actionLogic.ExecuteActionSet(actionSet).Wait();

            Assert.That(LoggingWriterActionHandler.LastMessage.Contains("zyx"), Is.True);
        }

        /// <summary>
        /// Creates the action logic for the test execution
        /// </summary>
        /// <returns></returns>
        public static ActionLogic CreateActionLogic()
        {
            var scopeStorage = new ScopeStorage();
            scopeStorage.Add(ActionLogicState.GetDefaultLogicState());
            scopeStorage.Add(WorkspaceLogic.InitDefault());

            var workspaceLogic = new WorkspaceLogic(scopeStorage);

            var actionLogic = new ActionLogic(workspaceLogic, scopeStorage);
            return actionLogic;
        }

        [Test]
        public void TestCreationAndDroppingOfWorkspaceByAction()
        {
            var scopeStorage = new ScopeStorage();
            scopeStorage.Add(ActionLogicState.GetDefaultLogicState());
            scopeStorage.Add(WorkspaceLogic.InitDefault());

            var workspaceLogic = new WorkspaceLogic(scopeStorage);

            var actionLogic = new ActionLogic(workspaceLogic, scopeStorage);
            
            
            var createWorkspaceAction = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CreateWorkspaceAction);
            Debug.Assert(createWorkspaceAction != null, nameof(createWorkspaceAction) + " != null");
            createWorkspaceAction.set(_DatenMeister._Actions._CreateWorkspaceAction.workspace, "ws");
            createWorkspaceAction.set(_DatenMeister._Actions._CreateWorkspaceAction.annotation, "I'm the workspace");

            actionLogic.ExecuteAction(createWorkspaceAction).Wait();

            Assert.That(workspaceLogic.Workspaces.Any(x => x.id == "ws"), Is.True);
            Assert.That(workspaceLogic.Workspaces.Any(x => x.annotation == "I'm the workspace"), Is.True);
            
            
            var dropWorkspaceAction = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__DropWorkspaceAction);
            Debug.Assert(dropWorkspaceAction != null, nameof(createWorkspaceAction) + " != null");
            dropWorkspaceAction.set(_DatenMeister._Actions._DropWorkspaceAction.workspace, "ws");
            actionLogic.ExecuteAction(dropWorkspaceAction).Wait();
            Assert.That(workspaceLogic.Workspaces.Any(x => x.id == "ws"), Is.False);
            Assert.That(workspaceLogic.Workspaces.Any(x => x.annotation == "I'm the workspace"), Is.False);
        }

        [Test]
        public void TestCreateExtent()
        {
            var scopeStorage = new ScopeStorage();
            scopeStorage.Add(ActionLogicState.GetDefaultLogicState());
            scopeStorage.Add(ConfigurationToExtentStorageMapper.GetDefaultMapper());
            scopeStorage.Add(WorkspaceLogic.InitDefault());

            var workspaceLogic = new WorkspaceLogic(scopeStorage);

            var actionLogic = new ActionLogic(workspaceLogic, scopeStorage);


            var createWorkspaceAction =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CreateWorkspaceAction);
            Debug.Assert(createWorkspaceAction != null, nameof(createWorkspaceAction) + " != null");
            createWorkspaceAction.set(_DatenMeister._Actions._CreateWorkspaceAction.workspace, "ws");
            createWorkspaceAction.set(_DatenMeister._Actions._CreateWorkspaceAction.annotation, "I'm the workspace");

            actionLogic.ExecuteAction(createWorkspaceAction).Wait();

            var foundExtent = workspaceLogic.FindExtent("ws", "dm:///");
            Assert.That(foundExtent, Is.Null);

            var loadExtentAction =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__LoadExtentAction);
            Debug.Assert(loadExtentAction != null, nameof(createWorkspaceAction) + " != null");
            var configuration =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__InMemoryLoaderConfig);
            Debug.Assert(configuration != null, nameof(configuration) + " != null");
            configuration.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId, "ws");
            configuration.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///");
            loadExtentAction.set(_DatenMeister._Actions._LoadExtentAction.configuration, configuration);

            actionLogic.ExecuteAction(loadExtentAction).Wait();

            foundExtent = workspaceLogic.FindExtent("ws", "dm:///");
            Assert.That(foundExtent, Is.Not.Null);
        }

        public static (IUriExtent source, IUriExtent target) CreateExtents(ActionLogic actionLogic)
        {
            var workspaceLogic = actionLogic.WorkspaceLogic;

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