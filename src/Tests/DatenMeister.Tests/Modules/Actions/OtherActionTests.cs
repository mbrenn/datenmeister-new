using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions;

[TestFixture]
public class OtherActionTests
{
    [Test]
    public async Task TestExportXmi()
    {
        var actionLogic = ActionSetTests.CreateActionLogic();
        ActionSetTests.CreateExtents(actionLogic);

        var action = InMemoryObject.CreateEmpty(_Actions.TheOne.OSIntegration.__ConsoleWriteAction)
            .SetProperties(new Dictionary<string, object>
            {
                [_Actions._OSIntegration._ConsoleWriteAction.text] = "Hello World"
            });
        
        await actionLogic.ExecuteAction(action);
    }
}