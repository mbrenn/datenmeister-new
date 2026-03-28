using System.ComponentModel;
using System.Text.Json;
using Autofac;
using DatenMeister.Actions;
using DatenMeister.Actions.Forms;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Tests.Json;
using DatenMeister.Web.Json;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions;

[TestFixture]
public class ActionFormTests
{
    [Test]
    public async Task TestAddingOfActionButtonInActions()
    {
        var dm = await DatenMeisterTests.GetDatenMeisterScope();
        // TODO: Load the full extents
        var extentManager = new ExtentManager(dm.WorkspaceLogic, dm.ScopeStorage);
        var inMemoryExtent =( await extentManager.LoadExtent(
            new ExtentLoaderConfigs.InMemoryLoaderConfig_Wrapper(InMemoryObject.TemporaryFactory)
            {
                extentUri = "dm:///test",
                dropExisting = true
            }.GetWrappedElement()
            )).Extent ?? throw new InvalidOperationException("Extent was null");
        
        var form = InMemoryObject.CreateEmpty(_Forms.TheOne.__Form);
        inMemoryExtent.elements().add(form);
        var actionDirect = InMemoryObject.CreateEmpty(_Actions.TheOne.__Action);
        inMemoryExtent.elements().add(actionDirect);
        var actionIndirect1 = InMemoryObject.CreateEmpty(_Actions.TheOne.__ActionSet);
        inMemoryExtent.elements().add(actionIndirect1);
        var actionIndirect2 = InMemoryObject.CreateEmpty(_Actions.TheOne.__DocumentOpenAction);
        inMemoryExtent.elements().add(actionIndirect2);
        var actionNon = InMemoryObject.CreateEmpty(_Management.TheOne.__Extent);
        inMemoryExtent.elements().add(actionNon);

        var formHandler = new ActionFormPlugin.ActionFormModificationPlugin();
        var context = new FormCreationContext
        {
            Global = new FormCreationContext.GlobalContext
            {
                Factory = InMemoryObject.TemporaryFactory, 
                FactoryForForms = InMemoryObject.TemporaryFactory
            }
        };

        context.LocalScopeStorage.Get<ExtensionCreationMode>();
        
        var result = new FormCreationResultMultipleForms
        {
            Forms = [form]
        };

        // Test 1 
        formHandler.CreateRowForm(
            new RowFormFactoryParameter
                {
                    MetaClass = actionDirect.metaclass!
                    }, context, result);
        Assert.That(result.IsManaged, Is.True);
        result.IsManaged = false;
            
        // Test 2
        formHandler.CreateRowForm(
            new RowFormFactoryParameter
            {
                MetaClass = actionIndirect1.metaclass!
            }, context, result);
        Assert.That(result.IsManaged, Is.True);
        result.IsManaged = false;
            
        // Test 3
        formHandler.CreateRowForm(
            new RowFormFactoryParameter
            {
                MetaClass = actionIndirect2.metaclass!
            }, context, result);
        Assert.That(result.IsManaged, Is.True);
        result.IsManaged = false;
            
        // Test 4
        formHandler.CreateRowForm(
            new RowFormFactoryParameter
            {
                MetaClass = actionNon.metaclass!
            }, context, result);
        Assert.That(result.IsManaged, Is.False);
        result.IsManaged = false;
    }

    [Test]
    public async Task TestAddQueryInPackage()
    {
        var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var actionHandler = new ActionLogic(dm.WorkspaceLogic, dm.ScopeStorage);

        // Gets the command
        var json = DatenMeisterTestsResources.MofJsonTests_TestQueryObjectExample;

        var asJsonObject = JsonSerializer.Deserialize<MofObjectAsJson>(json);
        var deconverted =
            new DirectJsonDeconverter(dm.WorkspaceLogic, dm.ScopeStorage).ConvertToObject(asJsonObject!) as IElement;
        Assert.That(deconverted, Is.Not.Null);

        // Now copy that thing to an extent
        var extentManager = dm.Resolve<ExtentManager>();
        var loadedExtent = (await extentManager.LoadExtent(
            new ExtentLoaderConfigs.InMemoryLoaderConfig_Wrapper(InMemoryObject.TemporaryFactory)
            {
                name = "dm:///test",
                extentUri = "dm:///test",
                workspaceId = "Data"
            }.GetWrappedElement())).Extent;
        Assert.That(loadedExtent, Is.Not.Null);

        var action = new DatenMeister.Core.Models.Actions.Forms.AddQueryInPackageAction_Wrapper(
            new MofFactory(loadedExtent!))
        {
            query = new DataViews.QueryStatement_Wrapper(
                deconverted!.getOrDefault<IElement>(_Actions._Forms._AddQueryInPackageAction.query)),
            targetPackageUri = "dm:///test",
            targetPackageWorkspace = "Data"
        };
        
        MofJsonTests.ValidateQueryProperties(action.query.GetWrappedElement());

        var result =
            new DatenMeister.Core.Models.Actions.ParameterTypes.CreateFormUponViewResult_Wrapper(
                await actionHandler.ExecuteAction(action.GetWrappedElement())
                ?? throw new InvalidOperationException("Result was null"));

        // Now gets the new item
        var newElement = dm.WorkspaceLogic.FindElement(
            "Data",
            result.resultingPackageUrl ??
            throw new InvalidAsynchronousStateException(
                "Resulting package was null"));
        
        Assert.That(newElement, Is.Not.Null);
        MofJsonTests.ValidateQueryProperties(newElement!);
    }

    [Test]
    public async Task TestStoreAsElement()
    {
        var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var actionHandler = new ActionLogic(dm.WorkspaceLogic, dm.ScopeStorage);

        // Gets the command
        var json = DatenMeisterTestsResources.MofJsonTests_TestQueryObjectExample;

        var asJsonObject = JsonSerializer.Deserialize<MofObjectAsJson>(json);
        var deconverted =
            new DirectJsonDeconverter(dm.WorkspaceLogic, dm.ScopeStorage).ConvertToObject(asJsonObject!) as IElement;
        Assert.That(deconverted, Is.Not.Null);

        // Now copy that thing to an extent
        var extentManager = dm.Resolve<ExtentManager>();
        var loadedExtent = (await extentManager.LoadExtent(
            new ExtentLoaderConfigs.InMemoryLoaderConfig_Wrapper(InMemoryObject.TemporaryFactory)
            {
                name = "dm:///test",
                extentUri = "dm:///test",
                workspaceId = "Data"
            }.GetWrappedElement())).Extent;
        Assert.That(loadedExtent, Is.Not.Null);

        var action = new DatenMeister.Core.Models.Actions.StoreElementAction_Wrapper(
            new MofFactory(loadedExtent!))
        {
            element = 
                deconverted!.getOrDefault<IElement>(_Actions._Forms._AddQueryInPackageAction.query),
            url = "dm:///test",
            workspace = "Data"
        };

        var asElement = action.element as IElement;
        Assert.That(asElement, Is.Not.Null);
        MofJsonTests.ValidateQueryProperties(asElement!);

        var result =
            new DatenMeister.Core.Models.Actions.TargetReferenceResult_Wrapper(
                await actionHandler.ExecuteAction(action.GetWrappedElement())
                ?? throw new InvalidOperationException("Result was null"));

        // Now gets the new item
        var newElement = dm.WorkspaceLogic.FindElement(
            result.targetWorkspace ?? throw new InvalidAsynchronousStateException("Workspace was null"),
            result.targetUrl ??
            throw new InvalidAsynchronousStateException(
                "Resulting package was null"));
        
        Assert.That(newElement, Is.Not.Null);
        MofJsonTests.ValidateQueryProperties(newElement!);
    }
}