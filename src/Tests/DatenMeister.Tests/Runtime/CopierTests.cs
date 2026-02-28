using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Extent.Manager.ExtentStorage;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime;

[TestFixture]
public class CopierTests
{
    private static string property1 = "Prop1";
    private static string property2 = "Prop2";

    [Test]
    public void TestCopyOfObject()
    {
        var mofExtent = new MofUriExtent(new InMemoryProvider(), "dm:///", null);
        var mofObject = new MofFactory(mofExtent).create(null);
        mofObject.set(property1, "55130");
        mofObject.set(property2, "Mainz");

        var mofObject2 = new MofFactory(mofExtent).create(null);
        mofObject2.set(property1, "65474");
        mofObject2.set(property2, "Bischofsheim");

        var copier = new ObjectCopier(new MofFactory(mofExtent));
        var result1 = copier.Copy(mofObject);
        var result2 = copier.Copy(mofObject2);

        Assert.That(result1, Is.Not.Null);
        Assert.That(result1.get(property1)?.ToString(), Is.EqualTo("55130"));
        Assert.That(result1.get(property2)?.ToString(), Is.EqualTo("Mainz"));
        Assert.That(result2.get(property1)?.ToString(), Is.EqualTo("65474"));
        Assert.That(result2.get(property2)?.ToString(), Is.EqualTo("Bischofsheim"));
    }

    /// <summary>
    /// We check the copy option that not only the Composites are copied but also
    /// all the references in case of a change of the extent or workspace
    /// </summary>
    [Test]
    public async Task TestCopyOfQueryObjectsIncludingCompositesAndReferences()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var extentManager = dm.Resolve<ExtentManager>();
        var mofExtent = (await extentManager.LoadExtent(
            new ExtentLoaderConfigs.InMemoryLoaderConfig_Wrapper(InMemoryObject.TemporaryFactory)
            {
                extentUri = "dm:///extent",
                dropExisting = true
            }.GetWrappedElement())).Extent;
        Assert.That(mofExtent, Is.Not.Null);
        var mofOtherExtent = (await extentManager.LoadExtent(
            new ExtentLoaderConfigs.InMemoryLoaderConfig_Wrapper(InMemoryObject.TemporaryFactory)
            {
                extentUri = "dm:///extent2",
                dropExisting = true
            }.GetWrappedElement())).Extent;
        Assert.That(mofOtherExtent, Is.Not.Null);

        var mofObject1 = new MofFactory(mofExtent!).create(_DataViews.TheOne.__QueryStatement);
        mofExtent!.elements().add(mofObject1);
        
        // Ok, now we need to create three data nodes and one result node
        var dataNodeSource = new MofFactory(mofExtent).create(_DataViews.TheOne.Source.__SelectFromAllWorkspacesNode);
        dataNodeSource.set(_DataViews._Source._SelectFromAllWorkspacesNode.name, "AllFromWorkspace");
        var dataNodeColumnFilter = new MofFactory(mofExtent).create(_DataViews.TheOne.Column.__ColumnFilterExcludeNode);
        dataNodeColumnFilter.set(_DataViews._Source._SelectFromAllWorkspacesNode.name, "ColumnFilter");
        var dataNodeRowFlatten = new MofFactory(mofExtent).create(_DataViews.TheOne.Row.__RowFlattenNode);
        dataNodeRowFlatten.set(_DataViews._Source._SelectFromAllWorkspacesNode.name, "RowFlatten");
        
        dataNodeColumnFilter.set(_DataViews._Column._ColumnFilterExcludeNode.input, dataNodeSource);
        dataNodeRowFlatten.set(_DataViews._Row._RowFlattenNode.input, dataNodeColumnFilter);
        
        mofObject1.set(_DataViews._QueryStatement.nodes,new List<object> {dataNodeSource, dataNodeColumnFilter, dataNodeRowFlatten});
        mofObject1.set(_DataViews._QueryStatement.resultNode, dataNodeRowFlatten);
        
        // STEP 1
        // Check that the connections are correct
        var checkNodeInputOfFlatten = dataNodeRowFlatten.getOrDefault<IElement>(_DataViews._Row._RowFlattenNode.input);
        Assert.That(checkNodeInputOfFlatten, Is.Not.Null);
        Assert.That(
            checkNodeInputOfFlatten.getOrDefault<string>(_DataViews._Source._SelectFromAllWorkspacesNode.name),
            Is.EqualTo("ColumnFilter"));


        var checkNodeInputOfFilter = dataNodeColumnFilter.getOrDefault<IElement>(_DataViews._Column._ColumnFilterExcludeNode.input);
        Assert.That(checkNodeInputOfFilter, Is.Not.Null);
        Assert.That(
            checkNodeInputOfFilter.getOrDefault<string>(_DataViews._Source._SelectFromAllWorkspacesNode.name),
            Is.EqualTo("AllFromWorkspace"));
        
        // STEP 2
        // Ok, now we copy it and check if we have done a full copy
        var copiedElement = ObjectCopier.Copy(new MofFactory(mofOtherExtent!), mofObject1, new CopyOption());
        mofOtherExtent!.elements().add(copiedElement);
        
        // STEP 3
        // Check that the element and ALL its children are copied and just referencing to new extent
        Assert.That((copiedElement as IHasExtent)?.Extent, Is.EqualTo(mofOtherExtent));
        var checkNodeInputOfFlatten2 = copiedElement.getOrDefault<IElement>(_DataViews._QueryStatement.resultNode);
        Assert.That(checkNodeInputOfFlatten2.GetUri()?.Contains("extent2") == true, Is.True);
        
        var nodes = copiedElement.getOrDefault<IReflectiveCollection>(_DataViews._QueryStatement.nodes);
        Assert.That(nodes.OfType<IElement>().Count(), Is.EqualTo(3));

        foreach (var node in nodes.OfType<IElement>())
        {
            Assert.That(node.GetUri()?.Contains("extent2") == true, Is.True);
        }
        
        // Now check the input chain
        var checkColumnFilter = checkNodeInputOfFlatten2.getOrDefault<IElement>(_DataViews._Row._RowFlattenNode.input);
        Assert.That(checkColumnFilter.GetUri()?.Contains("extent2") == true, Is.True);
        Assert.That(checkColumnFilter.getOrDefault<string>(
            _DataViews._Column._ColumnFilterExcludeNode.name), 
            Is.EqualTo("ColumnFilter"));
        
        // Check that the extent of the input source is correct
        var checkSource = checkColumnFilter.getOrDefault<IElement>(_DataViews._Column._ColumnFilterExcludeNode.input);
        Assert.That(checkSource.GetUri()?.Contains("extent2") == true, Is.True);
        Assert.That(checkSource.getOrDefault<string>(
            _DataViews._Source._SelectFromAllWorkspacesNode.name), 
            Is.EqualTo("AllFromWorkspace"));
    }
}