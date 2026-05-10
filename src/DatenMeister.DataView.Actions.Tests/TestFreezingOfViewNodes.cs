using Autofac;
using DatenMeister.Actions;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DependencyInjection;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Tests;
using NUnit.Framework;

namespace DatenMeister.DataView.Actions.Tests;

[TestFixture]
public class TestFreezingOfViewNodes
{
    [Test]
    public async Task TestFreezeInMemoryObject()
    {
        // Creates a viewnode which gets all properties in the DatenMeister Type
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();

        var classes = await CreateViewNode(dm);

        // Now we got everything and we can create the action...
        var actionLogic = dm.Resolve<ActionLogic>();
        var action = InMemoryObject.TemporaryFactory.create(_Actions.TheOne.Data.__FreezeViewResultInMemory);
        action.set(_Actions._Data._FreezeViewResultInMemory.extentUri, "dm:///all_umlclasses");
        action.set(_Actions._Data._FreezeViewResultInMemory.viewNode, classes);

        await actionLogic.ExecuteAction(action);
        
        // We should now have a new extent with all the classes...
        var foundUmlClasses = dm.WorkspaceLogic.FindExtent("dm:///all_umlclasses");
        Assert.That(foundUmlClasses, Is.Not.Null);
        
        // Check, that at least the Property is in
        Assert.That(foundUmlClasses!.elements().Count(), Is.GreaterThan(0));
        Assert.That(
            foundUmlClasses.elements().OfType<IElement>().Count(
                x => x.getOrDefault<string>(_UML._CommonStructure._NamedElement.name) == "InMemoryLoaderConfig"),
            Is.GreaterThan(0));
    }
    [Test]
    public async Task TestFreezeInExtent()
    {
        // Creates a viewnode which gets all properties in the DatenMeister Type
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();

        var classes = await CreateViewNode(dm);

        var tempExtent = classes.GetExtentOf();
        Assert.That(tempExtent, Is.Not.Null);
        var tempFactory = new MofFactory(tempExtent!);  
        
        // Loading
        var temporaryFilePath = DatenMeisterTests.GetPathForTemporaryStorage("test.xmi");
        var newExtentLoaderConfig = tempFactory.create(_ExtentLoaderConfigs.TheOne.__XmiStorageLoaderConfig);
        newExtentLoaderConfig.set(_ExtentLoaderConfigs._XmiStorageLoaderConfig.extentUri, "dm:///all_umlclasses");
        newExtentLoaderConfig.set(_ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath, temporaryFilePath);
        newExtentLoaderConfig.set(_ExtentLoaderConfigs._XmiStorageLoaderConfig.dropExisting, true);
        tempExtent.elements().add(newExtentLoaderConfig);
        
        // Now we got everything and we can create the action...
        var actionLogic = dm.Resolve<ActionLogic>();
        var action = tempFactory.create(_Actions.TheOne.Data.__FreezeViewResultInExtent);
        action.set(_Actions._Data._FreezeViewResultInExtent.viewNode, classes);
        tempExtent.elements().add(action);
        action.set(_Actions._Data._FreezeViewResultInExtent.extentLoaderConfig, newExtentLoaderConfig);

        await actionLogic.ExecuteAction(action);
        
        // We should now have a new extent with all the classes...
        var foundUmlClasses = dm.WorkspaceLogic.FindExtent("dm:///all_umlclasses");
        Assert.That(foundUmlClasses, Is.Not.Null);
        
        // Check, that at least the Property is in
        Assert.That(foundUmlClasses!.elements().Count(), Is.GreaterThan(0));
        Assert.That(
            foundUmlClasses.elements().OfType<IElement>().Count(
                x => x.getOrDefault<string>(_UML._CommonStructure._NamedElement.name) == "InMemoryLoaderConfig"),
            Is.GreaterThan(0));
        
        // Checks, that the file is existing and also contains the InMemoryLoaderConfig
        var extentManager = dm.Resolve<ExtentManager>();
        await extentManager.StoreExtent(foundUmlClasses);
        
        var found = await File.ReadAllTextAsync(temporaryFilePath);
        Assert.That(found, Contains.Substring("InMemoryLoaderConfig"));
    }

    /// <summary>
    /// Creates the viewnode being used to query all classes
    /// </summary>
    /// <param name="dm">DatenMeisterScope to create the necessary management functions</param>
    /// <returns>The viewnode</returns>
    private static async Task<IElement> CreateViewNode(IDatenMeisterScope dm)
    {
        var extent = (await dm.Resolve<ExtentManager>().LoadExtent(
            new ExtentLoaderConfigs.InMemoryLoaderConfig_Wrapper(InMemoryObject.TemporaryFactory)
            {
                extentUri = "dm:///viewnode"
            }.GetWrappedElement()))?.Extent;
        
        Assert.That(extent, Is.Not.Null);
        var factory = new MofFactory(extent!);   
        
        // We got our extent, now we add our view
        var selectFromExtent = new DataViews.Source.SelectByExtentNode_Wrapper(factory)
        {
            workspaceId = WorkspaceNames.WorkspaceTypes,
            extentUri = WorkspaceNames.UriExtentInternalTypes
        };
        extent!.elements().add(selectFromExtent.GetWrappedElement());
        
        // Get all descendents
        var descendents = factory.create(_DataViews.TheOne.Row.__RowFlattenNode);
        descendents.set(
            _DataViews._Row._RowFlattenNode.input, selectFromExtent.GetWrappedElement());
        
        extent.elements().add(descendents);
        
        // Filter on classes
        var classes = factory.create(_DataViews.TheOne.Row.__RowFilterByMetaclassNode);
        classes.set(_DataViews._Row._RowFilterByMetaclassNode.metaClass, _UML.TheOne.StructuredClassifiers.__Class);
        classes.set(_DataViews._Row._RowFilterByMetaclassNode.input, descendents);
        
        extent.elements().add(classes);
        return classes;
    }
}