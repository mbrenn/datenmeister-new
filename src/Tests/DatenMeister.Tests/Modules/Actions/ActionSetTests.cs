using System.Diagnostics;
using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.Hooks;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Provider.Xmi.Provider.XMI.ExtentStorage;
using DatenMeister.Provider.Xmi.Provider.Xml;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions;

[TestFixture]
public class ActionSetTests
{
    [Test]
    public void TestActionSetExecution()
    {
        var actionLogic = CreateActionLogic();

        var actionSet = InMemoryObject.CreateEmpty(_Actions.TheOne.__ActionSet);
        var action = InMemoryObject.CreateEmpty(_Actions.TheOne.__LoggingWriterAction);
        if (actionSet == null || action == null) throw new InvalidOperationException("null");

        action.set(_Actions._LoggingWriterAction.message, "zyx");
        actionSet.get<IReflectiveCollection>(_Actions._ActionSet.action).add(action);

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
        ResolveHookContainer.AddDefaultHooks(scopeStorage);
            
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
            
        var createWorkspaceAction =
            InMemoryObject.CreateEmpty(_Actions.TheOne.__CreateWorkspaceAction);
        Debug.Assert(createWorkspaceAction != null, nameof(createWorkspaceAction) + " != null");
        createWorkspaceAction.set(_Actions._CreateWorkspaceAction.workspaceId, "ws");
        createWorkspaceAction.set(_Actions._CreateWorkspaceAction.annotation, "I'm the workspace");

        actionLogic.ExecuteAction(createWorkspaceAction).Wait();

        Assert.That(workspaceLogic.Workspaces.Any(x => x.id == "ws"), Is.True);
        Assert.That(workspaceLogic.Workspaces.Any(x => x.annotation == "I'm the workspace"), Is.True);


        var dropWorkspaceAction = InMemoryObject.CreateEmpty(_Actions.TheOne.__DropWorkspaceAction);
        Debug.Assert(dropWorkspaceAction != null, nameof(createWorkspaceAction) + " != null");
        dropWorkspaceAction.set(_Actions._DropWorkspaceAction.workspaceId, "ws");
        actionLogic.ExecuteAction(dropWorkspaceAction).Wait();
        Assert.That(workspaceLogic.Workspaces.Any(x => x.id == "ws"), Is.False);
        Assert.That(workspaceLogic.Workspaces.Any(x => x.annotation == "I'm the workspace"), Is.False);
    }


    public static ProviderToProviderLoaderMapper GetDefaultMapper()
    {
        var result = new ProviderToProviderLoaderMapper();
        result.AddMapping(_ExtentLoaderConfigs.TheOne.__InMemoryLoaderConfig,
            _ => new InMemoryProviderLoader());
        result.AddMapping(_ExtentLoaderConfigs.TheOne.__CsvExtentLoaderConfig,
            _ => new CsvProviderLoader());
        result.AddMapping(_ExtentLoaderConfigs.TheOne.__XmiStorageLoaderConfig,
            _ => new XmiStorageProviderLoader());
        result.AddMapping(_ExtentLoaderConfigs.TheOne.__XmlReferenceLoaderConfig,
            _ => new XmlReferenceLoader());
        return result;
    }

    [Test]
    public void TestCreateExtent()
    {
        var scopeStorage = new ScopeStorage();
        scopeStorage.Add(ActionLogicState.GetDefaultLogicState());
        scopeStorage.Add(GetDefaultMapper());
        scopeStorage.Add(WorkspaceLogic.InitDefault());

        var workspaceLogic = new WorkspaceLogic(scopeStorage);

        var actionLogic = new ActionLogic(workspaceLogic, scopeStorage);


        var createWorkspaceAction =
            InMemoryObject.CreateEmpty(_Actions.TheOne.__CreateWorkspaceAction);
        Debug.Assert(createWorkspaceAction != null, nameof(createWorkspaceAction) + " != null");
        createWorkspaceAction.set(_Actions._CreateWorkspaceAction.workspaceId, "ws");
        createWorkspaceAction.set(_Actions._CreateWorkspaceAction.annotation, "I'm the workspace");

        actionLogic.ExecuteAction(createWorkspaceAction).Wait();

        var foundExtent = workspaceLogic.FindExtent("ws", "dm:///");
        Assert.That(foundExtent, Is.Null);

        var loadExtentAction =
            InMemoryObject.CreateEmpty(_Actions.TheOne.__LoadExtentAction);
        Debug.Assert(loadExtentAction != null, nameof(createWorkspaceAction) + " != null");
        var configuration =
            InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__InMemoryLoaderConfig);
        Debug.Assert(configuration != null, nameof(configuration) + " != null");
        configuration.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId, "ws");
        configuration.set(_ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///");
        loadExtentAction.set(_Actions._LoadExtentAction.configuration, configuration);

        actionLogic.ExecuteAction(loadExtentAction).Wait();

        foundExtent = workspaceLogic.FindExtent("ws", "dm:///");
        Assert.That(foundExtent, Is.Not.Null);
    }

    public static (IUriExtent source, IUriExtent target) CreateExtents(ActionLogic actionLogic)
    {
        var workspaceLogic = actionLogic.WorkspaceLogic;

        var sourceProvider = new InMemoryProvider();
        var targetProvider = new InMemoryProvider();
        var sourceExtent = new MofUriExtent(sourceProvider, "dm:///source/", actionLogic.ScopeStorage);
        var targetExtent = new MofUriExtent(targetProvider, "dm:///target/", actionLogic.ScopeStorage);
            
        var sourceFactory = new MofFactory(sourceExtent);
        var targetFactory = new MofFactory(targetExtent);

        workspaceLogic.AddExtent(workspaceLogic.GetDataWorkspace(), sourceExtent);
        workspaceLogic.AddExtent(workspaceLogic.GetDataWorkspace(), targetExtent);

        var sourceElement1 = sourceFactory.create(null)
            .SetProperties(new Dictionary<string, object> {["name"] = "source1"})
            .SetId("source1");
        var sourceElement1_1 = sourceFactory.create(null)
            .SetProperties(new Dictionary<string, object> {["name"] = "source1.1"})
            .SetId("source1.1");
        var sourceElement1_2 = sourceFactory.create(null)
            .SetProperties(new Dictionary<string, object> {["name"] = "source1.2"})
            .SetId("source1.2");
        var sourceElement1_3 = sourceFactory.create(null)
            .SetProperties(new Dictionary<string, object> {["name"] = "source1.3"})
            .SetId("source1.3");
        var sourceElement1_4 = sourceFactory.create(null)
            .SetProperties(new Dictionary<string, object> {["name"] = "source1.4"})
            .SetId("source1.4");

        sourceElement1.set(DefaultClassifierHints.GetDefaultPackagePropertyName(sourceElement1),
            new[] {sourceElement1_1, sourceElement1_2, sourceElement1_3, sourceElement1_4});

        var sourceElement2 = sourceFactory.create(null)
            .SetProperties(new Dictionary<string, object> { ["name"] = "source2" })
            .SetId("source2");

        sourceExtent.elements().add(sourceElement1);
        sourceExtent.elements().add(sourceElement2);

        var targetElement1 = targetFactory.create(null)
            .SetProperties(new Dictionary<string, object> {["name"] = "target1"})
            .SetId("target1");
        var targetElement1_1 = targetFactory.create(null)
            .SetProperties(new Dictionary<string, object> {["name"] = "target1.1"})
            .SetId("target1.1");
        targetElement1.set(DefaultClassifierHints.GetDefaultPackagePropertyName(targetElement1),
            new[] {targetElement1_1});
        targetExtent.elements().add(targetElement1);

        return (sourceExtent, targetExtent);
    }
}