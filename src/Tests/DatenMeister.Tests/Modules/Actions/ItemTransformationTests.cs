using DatenMeister.Actions.Transformations;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions;

[TestFixture]
public class ItemTransformationTests
{
    [Test]
    public async Task TestItemTransformation()
    {
        var actionLogic = ActionSetTests.CreateActionLogic();
        var (source, _) = ActionSetTests.CreateExtents(actionLogic);

        // This is only required to let the right reflection type working
        var temp = new UpperCaseTransformation();
        Assert.That(temp is IItemTransformation);

        var action = InMemoryObject.CreateEmpty(_Actions.TheOne.__TransformItemsAction)
            .SetProperties(new Dictionary<string, object>
            {
                [_Actions._TransformItemsAction.path] = "dm:///source/",
                [_Actions._TransformItemsAction.workspaceId] = "Data",
                [_Actions._TransformItemsAction.runtimeClass] =
                    "DatenMeister.Actions.Transformations.UpperCaseTransformation"
            });

        var found = source.elements().OfType<IElement>()
            .FirstOrDefault(x => x.getOrDefault<string>("name") == "source1");
        Assert.That(found, Is.Not.Null);

        found = source.elements().OfType<IElement>()
            .FirstOrDefault(x => x.getOrDefault<string>("name") == "SOURCE1");
        Assert.That(found, Is.Null);

        await actionLogic.ExecuteAction(action);

        found = source.elements().OfType<IElement>()
            .FirstOrDefault(x => x.getOrDefault<string>("name") == "SOURCE1");
        Assert.That(found, Is.Not.Null);

        found = source.elements().OfType<IElement>()
            .FirstOrDefault(x => x.getOrDefault<string>("name") == "source1");
        Assert.That(found, Is.Null);
    }
}