using System;
using System.Diagnostics;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Modules.Actions;
using DatenMeister.Modules.Actions.ActionHandler;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules
{
    [TestFixture]
    public class ActionSetTests
    {
        [Test]
        public void TestActionSetExecution()
        {
            var scopeStorage = new ScopeStorage();
            scopeStorage.Add(ActionLogicState.GetDefaultLogicState());
            scopeStorage.Add(WorkspaceLogic.InitDefault());

            var workspaceLogic = new WorkspaceLogic(scopeStorage);

            var actionLogic = new ActionLogic(workspaceLogic, scopeStorage);

            var actionSet = InMemoryObject.CreateEmpty(_Actions.TheOne.__ActionSet) as IElement;
            var action = InMemoryObject.CreateEmpty(_Actions.TheOne.__LoggingWriterAction) as IElement;
            if (actionSet == null || action == null) throw new InvalidOperationException("null");
            
            action.set(_Actions._LoggingWriterAction.message, "zyx");
            actionSet.get<IReflectiveCollection>(_Actions._ActionSet.action).add(action);

            actionLogic.ExecuteActionSet(actionSet).Wait();

            Assert.That(LoggingWriterActionHandler.LastMessage.Contains("zyx"), Is.True);
        }

        [Test]
        public void TestCreationAndDroppingOfWorkspaceByAction()
        {
            var scopeStorage = new ScopeStorage();
            scopeStorage.Add(ActionLogicState.GetDefaultLogicState());
            scopeStorage.Add(WorkspaceLogic.InitDefault());

            var workspaceLogic = new WorkspaceLogic(scopeStorage);

            var actionLogic = new ActionLogic(workspaceLogic, scopeStorage);
            
            
            var createWorkspaceAction = InMemoryObject.CreateEmpty(_Actions.TheOne.__CreateWorkspaceAction) as IElement;
            Debug.Assert(createWorkspaceAction != null, nameof(createWorkspaceAction) + " != null");
            createWorkspaceAction.set(_Actions._CreateWorkspaceAction.workspace, "ws");
            createWorkspaceAction.set(_Actions._CreateWorkspaceAction.annotation, "I'm the workspace");

            actionLogic.ExecuteAction(createWorkspaceAction).Wait();

            Assert.That(workspaceLogic.Workspaces.Any(x => x.id == "ws"), Is.True);
            Assert.That(workspaceLogic.Workspaces.Any(x => x.annotation == "I'm the workspace"), Is.True);
            
            
            var dropWorkspaceAction = InMemoryObject.CreateEmpty(_Actions.TheOne.__DropWorkspaceAction) as IElement;
            Debug.Assert(dropWorkspaceAction != null, nameof(createWorkspaceAction) + " != null");
            dropWorkspaceAction.set(_Actions._DropWorkspaceAction.workspace, "ws");
            actionLogic.ExecuteAction(dropWorkspaceAction).Wait();
            Assert.That(workspaceLogic.Workspaces.Any(x => x.id == "ws"), Is.False);
            Assert.That(workspaceLogic.Workspaces.Any(x => x.annotation == "I'm the workspace"), Is.False);
        }

        [Test]
        public void TestCreateWorkspace()
        {
            var scopeStorage = new ScopeStorage();
            scopeStorage.Add(ActionLogicState.GetDefaultLogicState());
            scopeStorage.Add(WorkspaceLogic.InitDefault());

            var workspaceLogic = new WorkspaceLogic(scopeStorage);

            var actionLogic = new ActionLogic(workspaceLogic, scopeStorage);
            
            
            var createWorkspaceAction = InMemoryObject.CreateEmpty(_Actions.TheOne.__CreateWorkspaceAction) as IElement;
            Debug.Assert(createWorkspaceAction != null, nameof(createWorkspaceAction) + " != null");
            createWorkspaceAction.set(_Actions._CreateWorkspaceAction.workspace, "ws");
            createWorkspaceAction.set(_Actions._CreateWorkspaceAction.annotation, "I'm the workspace");

            actionLogic.ExecuteAction(createWorkspaceAction).Wait();

            Assert.That(workspaceLogic.Workspaces.Any(x => x.id == "ws"), Is.True);
            Assert.That(workspaceLogic.Workspaces.Any(x => x.annotation == "I'm the workspace"), Is.True);
            
            
            var loadExtentAction = InMemoryObject.CreateEmpty(_Actions.TheOne.__LoadExtentAction) as IElement;
            Debug.Assert(loadExtentAction != null, nameof(createWorkspaceAction) + " != null");
            //var loadExtentAction = InMemoryObject.CreateEmpty() as IElement;

            actionLogic.ExecuteAction(loadExtentAction).Wait();
            
            
        }
    }
}