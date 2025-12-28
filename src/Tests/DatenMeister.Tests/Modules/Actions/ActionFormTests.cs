using DatenMeister.Actions.Forms;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFactory;
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
                Factory = InMemoryObject.TemporaryFactory
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
}