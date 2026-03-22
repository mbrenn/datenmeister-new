using System.Text.Json;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Tests.Json;
using DatenMeister.Web.Json;
using NUnit.Framework;

namespace DatenMeister.Tests.Integration;

[TestFixture]
public class DifferentIntegrationTests
{
    /// <summary>
    /// Tests the loading of a query statement from a JSON representation,
    /// converting it into an IElement, and copying it into a specified extent.
    /// Verifies the proper creation, addition, and validation of the query
    /// object in the target extent.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous operation of the test.
    /// The test ensures that the query object is correctly loaded, copied, and validated in the specified extent.
    /// </returns>
    [Test]
    public async Task TestLoadingOfQueryStatementAndCopyingIntoExtent()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        
        var json = DatenMeisterTestsResources.MofJsonTests_TestQueryObjectExample;

        var asJsonObject = JsonSerializer.Deserialize<MofObjectAsJson>(json);
        var deconverted = new DirectJsonDeconverter(dm.WorkspaceLogic, dm.ScopeStorage).ConvertToObject(asJsonObject!) as IElement;
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
        
        var queryStatement = deconverted!.getOrDefault<IElement>("query");
        Assert.That(queryStatement, Is.Not.Null);
        
        var targetObject = new MofFactory(loadedExtent!).create(_DataViews.TheOne.__QueryStatement);
        loadedExtent!.elements().add(targetObject);

        ObjectCopier.CopyPropertiesStatic(queryStatement, targetObject);

        var addedElement = loadedExtent.elements().OfType<IElement>().FirstOrDefault();
        Assert.That(addedElement, Is.Not.Null);
        MofJsonTests.ValidateQueryProperties(addedElement!);
    }
}