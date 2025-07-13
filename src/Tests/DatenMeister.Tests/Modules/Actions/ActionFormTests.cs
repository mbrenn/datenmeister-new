using DatenMeister.Actions.Forms;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFactory;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions;

[TestFixture]
public class ActionFormTests
{
    [Test]
    public void TestAddingOfActionButtonInActions()
    {
        // TODO: Load the full extents
        var form = InMemoryObject.CreateEmpty(_Forms.TheOne.__Form);
        var actionDirect = InMemoryObject.CreateEmpty(_Actions.TheOne.__Action);
        var actionIndirect1 = InMemoryObject.CreateEmpty(_Actions.TheOne.__ActionSet);
        var actionIndirect2 = InMemoryObject.CreateEmpty(_Actions.TheOne.__DocumentOpenAction);
        var actionNon = InMemoryObject.CreateEmpty(_Management.TheOne.__Extent);

        var formHandler = new ActionFormPlugin.ActionFormModificationPlugin();
        var context = new FormCreationContext
        {
            Global = new FormCreationContext.GlobalContext
            {
                Factory = InMemoryObject.TemporaryFactory
            }
        };

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