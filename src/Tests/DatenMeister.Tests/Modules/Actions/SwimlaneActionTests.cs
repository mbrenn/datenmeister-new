using DatenMeister.Actions;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Reports.Swimlane;
using DatenMeister.Reports.Swimlane.Model;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions;

[TestFixture]
public class SwimlaneActionTests
{
    [Test]
    public async Task TestSwimlaneActionHandlerWithRenderVerb()
    {
        var actionLogic = ActionSetTests.CreateActionLogic();
        var handler = new SwimLaneViewDefinitionActionHandler();

        var viewDef = InMemoryObject.CreateEmpty(_Root.TheOne.__SwimlaneViewDefinition);
        viewDef.set(_Root._SwimlaneViewDefinition.name, "Sprint Overview");

        var config = InMemoryObject.CreateEmpty(_Root.TheOne.__SwimlaneConfiguration);
        config.set(_Root._SwimlaneConfiguration.verticalSwimlaneProperty, "Sprint");
        config.set(_Root._SwimlaneConfiguration.horizontalSwimlaneProperty, "Assigned To");
        viewDef.set(_Root._SwimlaneViewDefinition.swimlaneConfiguration, config);

        Assert.That(handler.IsResponsible(viewDef), Is.True);

        var result = await handler.Evaluate(actionLogic, viewDef, "render");
        Assert.That(result, Is.Not.Null);

        var actionResultWrapper = new DatenMeister.Core.Models.Actions.ActionResult_Wrapper(result!);
        var clientActionList = (actionResultWrapper.clientActions as IEnumerable<object>)?.ToList();
        Assert.That(clientActionList, Is.Not.Null);
        Assert.That(clientActionList!.Count, Is.EqualTo(1));

        var renderFormAction = clientActionList[0] as IElement;
        Assert.That(renderFormAction, Is.Not.Null);
        Assert.That(renderFormAction!.getMetaClass()?.equals(
            _Actions.TheOne.ClientActions.__RenderFormClientAction), Is.True);

        var form = renderFormAction.getOrDefault<IElement>(
            _Actions._ClientActions._RenderFormClientAction.form);
        Assert.That(form, Is.Not.Null);
        Assert.That(form!.getMetaClass()?.equals(_Root.TheOne.__SwimlaneForm), Is.True);

        var title = form.getOrDefault<string>(_Root._SwimlaneForm.title);
        Assert.That(title, Is.EqualTo("Swimlane: Sprint Overview"));
    }

    [Test]
    public async Task TestSwimlaneActionHandlerWithInvalidVerbReturnsAlert()
    {
        var actionLogic = ActionSetTests.CreateActionLogic();
        var handler = new SwimLaneViewDefinitionActionHandler();

        var viewDef = InMemoryObject.CreateEmpty(_Root.TheOne.__SwimlaneViewDefinition);
        viewDef.set(_Root._SwimlaneViewDefinition.name, "Sprint Overview");

        var result = await handler.Evaluate(actionLogic, viewDef, "execute");
        Assert.That(result, Is.Not.Null);

        var actionResultWrapper = new DatenMeister.Core.Models.Actions.ActionResult_Wrapper(result!);
        var clientActionList = (actionResultWrapper.clientActions as IEnumerable<object>)?.ToList();
        Assert.That(clientActionList, Is.Not.Null);
        Assert.That(clientActionList!.Count, Is.EqualTo(1));

        var alertAction = clientActionList[0] as IElement;
        Assert.That(alertAction, Is.Not.Null);
        Assert.That(alertAction!.getMetaClass()?.equals(
            _Actions.TheOne.ClientActions.__AlertClientAction), Is.True);
    }
}
