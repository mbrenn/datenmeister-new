using System;
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
    }
}