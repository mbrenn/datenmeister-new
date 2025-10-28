using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions;

[TestFixture]
public class ClearCollectionActionTests
{
    [Test]
    public async Task TestClearCollectionInExtent()
    {
        var actionLogic = ActionSetTests.CreateActionLogic();
        var (source, _) = ActionSetTests.CreateExtents(actionLogic);

        var action = InMemoryObject.CreateEmpty(_Actions.TheOne.__ClearCollectionAction)
            .SetProperties(new Dictionary<string, object>
            {
                [_Actions._ClearCollectionAction.path] = "dm:///source/",
            });

        Assert.That(source.elements().Count(), Is.GreaterThan(0));
        await actionLogic.ExecuteAction(action);
        Assert.That(source.elements().Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task TestClearCollectionInProperty()
    {
        var actionLogic = ActionSetTests.CreateActionLogic();
        var (source, _) = ActionSetTests.CreateExtents(actionLogic);

        var action = InMemoryObject.CreateEmpty(_Actions.TheOne.__ClearCollectionAction)
            .SetProperties(new Dictionary<string, object>
            {
                [_Actions._ClearCollectionAction.path] =
                    "dm:///source/?fn=source1&prop=packagedElement",
            });

        Assert.That(source.elements().Count(), Is.GreaterThan(0));
        var source1 = source.elements().First() as IElement;
        Assert.That(source1, Is.Not.Null);
        var packages = source1!.getOrDefault<IReflectiveCollection>("packagedElement");
        Assert.That(packages, Is.Not.Null);
        Assert.That(packages.Count(), Is.GreaterThan(0));
            
        await actionLogic.ExecuteAction(action);
        Assert.That(source.elements().Count(), Is.GreaterThan(0));
        Assert.That(packages.Count(), Is.EqualTo(0));
    }
}