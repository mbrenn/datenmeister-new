using Autofac;
using DatenMeister.Actions;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.TemporaryExtent;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions;

[TestFixture]
public class MoveUpDownActionTests
{
    [Test]
    public async Task TestMovingUpDownInExtent()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var actionLogic = dm.Resolve<ActionLogic>();

        var extent = new MofUriExtent(new InMemoryProvider(), "dm:///test", dm.ScopeStorage);
        dm.WorkspaceLogic.AddExtent(dm.WorkspaceLogic.GetDataWorkspace(), extent);
        var factory = new MofFactory(extent);

        // Adds the three test items
        var i1 = factory.create(null);
        i1.set("name", "i1");
        (i1 as ICanSetId)!.Id = "i1";
        extent.elements().add(i1);
        var i2 = factory.create(null);
        i2.set("name", "i2");
        (i2 as ICanSetId)!.Id = "i2";
        extent.elements().add(i2);
        var i3 = factory.create(null);
        i3.set("name", "i3");
        (i3 as ICanSetId)!.Id = "i3";
        extent.elements().add(i3);

        // Check, that i2 is on 2nd position
        Assert.That(
            (extent.elements().ElementAt(1) as IElement).getOrDefault<string>("name"),
            Is.EqualTo("i2")
        );
        Assert.That(
            ((extent.elements().ElementAt(1) as IElement) as IHasId)?.Id,
            Is.EqualTo("i2")
        );
        //
        // Now, move it up!
        var temporaryExtentLogic = new TemporaryExtentLogic(dm.WorkspaceLogic, dm.ScopeStorage);
        var moveAction = temporaryExtentLogic.CreateTemporaryElement(
            _Actions.TheOne.__MoveUpDownAction);
        moveAction.set(
            _Actions._MoveUpDownAction.container, 
            extent);
        moveAction.set(
            _Actions._MoveUpDownAction.element, 
            i2);
        moveAction.set(
            _Actions._MoveUpDownAction.direction, 
            _Actions.___MoveDirectionType.Up);
        await actionLogic.ExecuteAction(moveAction);
            
        // Check, that i2 is on 1st position
        Assert.That(
            (extent.elements().ElementAt(0) as IElement).getOrDefault<string>("name"),
            Is.EqualTo("i2")
        );
            
        //
        // Now, move it down again. Twice
        moveAction = temporaryExtentLogic.CreateTemporaryElement(
            _Actions.TheOne.__MoveUpDownAction);
        moveAction.set(
            _Actions._MoveUpDownAction.container, 
            extent);
        moveAction.set(
            _Actions._MoveUpDownAction.element, 
            i2);
        moveAction.set(
            _Actions._MoveUpDownAction.direction, 
            _Actions.___MoveDirectionType.Down);
        await actionLogic.ExecuteAction(moveAction);
        await actionLogic.ExecuteAction(moveAction);
            
        // Check, that i2 is on 3rd position
        Assert.That(
            (extent.elements().ElementAt(2) as IElement).getOrDefault<string>("name"),
            Is.EqualTo("i2")
        );
    }
        
    [Test]
    public async Task TestMovingUpDownInCollection()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var actionLogic = dm.Resolve<ActionLogic>();

        var extent = new MofUriExtent(new InMemoryProvider(), "dm:///test", dm.ScopeStorage);
        dm.WorkspaceLogic.AddExtent(dm.WorkspaceLogic.GetDataWorkspace(), extent);
        var factory = new MofFactory(extent);
        var parent = factory.create(null);
        parent.set("name", "parent");
        (parent as ICanSetId)!.Id = "parent";
        extent.elements().add(parent);
        var collection = parent.get<IReflectiveSequence>("children");

        // Adds the three test items
        var i1 = factory.create(null);
        i1.set("name", "i1");
        (i1 as ICanSetId)!.Id = "i1";
        collection.add(i1);
        var i2 = factory.create(null);
        i2.set("name", "i2");
        (i2 as ICanSetId)!.Id = "i2";
        collection.add(i2);
        var i3 = factory.create(null);
        i3.set("name", "i3");
        (i3 as ICanSetId)!.Id = "i3";
        collection.add(i3);

        // Check, that i2 is on 2nd position
        Assert.That(
            (collection.ElementAt(1) as IElement).getOrDefault<string>("name"),
            Is.EqualTo("i2")
        );

        //
        // Now, move it up!
        var temporaryExtentLogic = new TemporaryExtentLogic(dm.WorkspaceLogic, dm.ScopeStorage);
        var moveAction = temporaryExtentLogic.CreateTemporaryElement(
            _Actions.TheOne.__MoveUpDownAction);
        moveAction.set(
            _Actions._MoveUpDownAction.container, 
            parent);
        moveAction.set(
            _Actions._MoveUpDownAction.property, 
            "children");
        moveAction.set(
            _Actions._MoveUpDownAction.element, 
            i2);
        moveAction.set(
            _Actions._MoveUpDownAction.direction, 
            _Actions.___MoveDirectionType.Up);
        await actionLogic.ExecuteAction(moveAction);
            
        // Check, that i2 is on 1st position
        Assert.That(
            (collection.ElementAt(0) as IElement).getOrDefault<string>("name"),
            Is.EqualTo("i2")
        );
            
        //
        // Now, move it down again. Twice
        moveAction = temporaryExtentLogic.CreateTemporaryElement(
            _Actions.TheOne.__MoveUpDownAction);
        moveAction.set(
            _Actions._MoveUpDownAction.container, 
            parent);
        moveAction.set(
            _Actions._MoveUpDownAction.property, 
            "children");
        moveAction.set(
            _Actions._MoveUpDownAction.element, 
            i2);
        moveAction.set(
            _Actions._MoveUpDownAction.direction, 
            _Actions.___MoveDirectionType.Down);
        await actionLogic.ExecuteAction(moveAction);
        await actionLogic.ExecuteAction(moveAction);
            
        // Check, that i2 is on 3rd position
        Assert.That(
            (collection.ElementAt(2) as IElement).getOrDefault<string>("name"),
            Is.EqualTo("i2")
        );
    }
}