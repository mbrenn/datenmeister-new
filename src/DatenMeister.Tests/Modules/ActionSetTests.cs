﻿using System;
using System.Diagnostics;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Models.EMOF;
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

            var actionSet = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__ActionSet) as IElement;
            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__LoggingWriterAction) as IElement;
            if (actionSet == null || action == null) throw new InvalidOperationException("null");
            
            action.set(_DatenMeister._Actions._LoggingWriterAction.message, "zyx");
            actionSet.get<IReflectiveCollection>(_DatenMeister._Actions._ActionSet.action).add(action);

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
            
            
            var createWorkspaceAction = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CreateWorkspaceAction) as IElement;
            Debug.Assert(createWorkspaceAction != null, nameof(createWorkspaceAction) + " != null");
            createWorkspaceAction.set(_DatenMeister._Actions._CreateWorkspaceAction.workspace, "ws");
            createWorkspaceAction.set(_DatenMeister._Actions._CreateWorkspaceAction.annotation, "I'm the workspace");

            actionLogic.ExecuteAction(createWorkspaceAction).Wait();

            Assert.That(workspaceLogic.Workspaces.Any(x => x.id == "ws"), Is.True);
            Assert.That(workspaceLogic.Workspaces.Any(x => x.annotation == "I'm the workspace"), Is.True);
            
            
            var dropWorkspaceAction = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__DropWorkspaceAction) as IElement;
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
            scopeStorage.Add(WorkspaceLogic.InitDefault());

            var workspaceLogic = new WorkspaceLogic(scopeStorage);

            var actionLogic = new ActionLogic(workspaceLogic, scopeStorage);


            var createWorkspaceAction =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CreateWorkspaceAction) as IElement;
            Debug.Assert(createWorkspaceAction != null, nameof(createWorkspaceAction) + " != null");
            createWorkspaceAction.set(_DatenMeister._Actions._CreateWorkspaceAction.workspace, "ws");
            createWorkspaceAction.set(_DatenMeister._Actions._CreateWorkspaceAction.annotation, "I'm the workspace");

            actionLogic.ExecuteAction(createWorkspaceAction).Wait();

            var foundExtent = workspaceLogic.FindExtent("ws", "dm:///");
            Assert.That(foundExtent, Is.Null);

            var loadExtentAction =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__LoadExtentAction) as IElement;
            Debug.Assert(loadExtentAction != null, nameof(createWorkspaceAction) + " != null");
            var configuration =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__InMemoryLoaderConfig) as IElement;
            Debug.Assert(configuration != null, nameof(configuration) + " != null");
            configuration.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId, "ws");
            configuration.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///");
            loadExtentAction.set(_DatenMeister._Actions._LoadExtentAction.configuration, configuration);

            //var loadExtentAction = InMemoryObject.CreateEmpty() as IElement;

            actionLogic.ExecuteAction(loadExtentAction).Wait();

            foundExtent = workspaceLogic.FindExtent("ws", "dm:///");
            Assert.That(foundExtent, Is.Not.Null);
        }
    }
}