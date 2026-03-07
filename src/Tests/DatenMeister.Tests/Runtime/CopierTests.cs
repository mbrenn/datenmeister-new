using Autofac;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
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
    private static readonly ILogger Logger = new ClassLogger(typeof(CopierTests));
    private static string property1 = "Prop1";
    private static string property2 = "Prop2";

    [Test]
    public void TestCopyOfObjectJustForAttributes()
    {
        var mofExtent = new MofUriExtent(new InMemoryProvider(), "dm:///", null);
        var mofObject = new MofFactory(mofExtent).create(null);
        mofObject.set(property1, "55130");
        mofObject.set(property2, "Mainz");

        var mofObject2 = new MofFactory(mofExtent).create(null);
        mofObject2.set(property1, "65474");
        mofObject2.set(property2, "Bischofsheim");
        
        Logger.Info("Starting COPYING");

        var copier = new ObjectCopier(new MofFactory(mofExtent));
        var result1 = copier.Copy(mofObject);
        var result2 = copier.Copy(mofObject2);

        Assert.That(result1, Is.Not.Null);
        Assert.That(result1.get(property1)?.ToString(), Is.EqualTo("55130"));
        Assert.That(result1.get(property2)?.ToString(), Is.EqualTo("Mainz"));
        Assert.That(result2.get(property1)?.ToString(), Is.EqualTo("65474"));
        Assert.That(result2.get(property2)?.ToString(), Is.EqualTo("Bischofsheim"));
    }
    
    [Test]
    public void TestCopyIncludingSubItems()
    {
        var mofExtent = new MofUriExtent(new InMemoryProvider(), "dm:///", null);
        var mofObject = new MofFactory(mofExtent).create(null);
        var mofChild = new MofFactory(mofExtent).create(null);

        mofObject.set(property1, "55130");
        mofObject.set(property2, "Mainz");
        mofChild.set("Name", "Martin");
        mofObject.set("Mayor", mofChild);

        var copier = new ObjectCopier(new MofFactory(mofExtent));
        Logger.Info("Starting COPYING");
        var result1 = copier.Copy(mofObject);

        Assert.That(result1, Is.Not.Null);
        Assert.That(result1.get(property1)?.ToString(), Is.EqualTo("55130"));
        Assert.That(result1.get(property2)?.ToString(), Is.EqualTo("Mainz"));
        
        // Check that the mayor is copied. We expect this in case of unknown metaclasses
        var mayor = result1.getOrDefault<IElement>("Mayor");
        Assert.That(mayor, Is.Not.Null);
        Assert.That((mayor as MofObject)?.ProviderObject, Is.Not.Null);
        Assert.That((mayor as MofObject)?.ProviderObject, Is.Not.EqualTo((mofChild as MofObject)?.ProviderObject));
        Assert.That(
            mayor.getOrDefault<string>("Name"),
            Is.EqualTo("Martin"));
    }
    
    [Test]
    public async Task TestCopyOfCompositesWithCopyingAcrossExtents()
    {
        await InternalTestForCompositingObjects(true, false);
    }
    
    [Test]
    public async Task TestCopyOfCompositesWithoutCopyingAcrossExtents()
    {
        await InternalTestForCompositingObjects(false, false);
    }
    
    [Test]
    public async Task TestCopyOfCompositesWithCopyingAcrossExtentsWithinSameExtent()
    {
        await InternalTestForCompositingObjects(true, true);
    }
    
    [Test]
    public async Task TestCopyOfCompositesWithoutCopyingAcrossExtentsWithinSameExtent()
    {
        await InternalTestForCompositingObjects(false, true);
    }

    private static async Task InternalTestForCompositingObjects(bool copyAcrossExtents, bool withinSameExtent)
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

        IUriExtent? mofExtent2;

        if (withinSameExtent)
        {
            mofExtent2 = mofExtent;
        }
        else
        {
            mofExtent2 = (await extentManager.LoadExtent(
                new ExtentLoaderConfigs.InMemoryLoaderConfig_Wrapper(InMemoryObject.TemporaryFactory)
                {
                    extentUri = "dm:///extent2",
                    dropExisting = true
                }.GetWrappedElement())).Extent;
            Assert.That(mofExtent2, Is.Not.Null);
        }

        // Prepare a compositing object with two fields
        var factory = new MofFactory(mofExtent!);
        var rowForm = factory.create(_Forms.TheOne.__RowForm);
        var fieldCheckbox = factory.create(_Forms.TheOne.__CheckboxFieldData);
        fieldCheckbox.set(_Forms._CheckboxFieldData.name, "Checkbox");
        var fieldText = factory.create(_Forms.TheOne.__TextFieldData);
        fieldText.set(_Forms._TextFieldData.name, "Textbox");
        rowForm.set(_Forms._RowForm.field, new List<object> {fieldCheckbox, fieldText});
        mofExtent!.elements().add(rowForm);
        
        // Copy it
        ObjectCopier.FullDebug = true;
        Logger.Info("Starting COPYING");
        var copiedElement = ObjectCopier.Copy(
            new MofFactory(mofExtent2!),
            rowForm,
            new CopyOption
            {
                PredicateToClone = CopyOption.GetPredicateForUmlCopying(new CopyPredicateParameter {
                    CopyAcrossExtents = copyAcrossExtents
                })
            });
        Logger.Info("Ended COPYING");
        
        ObjectCopier.FullDebug = false;
        
        // Check that the new object is existing and of metaclass rowform
        Assert.That(copiedElement, Is.Not.Null);
        Assert.That(copiedElement.getMetaClass()?.equals(_Forms.TheOne.__RowForm), Is.True);
        
        // Check that there are two fields and identify each for checking of names by metaclass
        var field = copiedElement.getOrDefault<IReflectiveCollection>(_Forms._RowForm.field);
        Assert.That(field, Is.Not.Null);
        Assert.That(field.OfType<IElement>().Count(), Is.EqualTo(2));
        
        var fieldCheckbox2 = field.OfType<IElement>().FirstOrDefault(x => x.getMetaClass()?.Equals( _Forms.TheOne.__CheckboxFieldData) == true);
        Assert.That(fieldCheckbox2, Is.Not.Null);
        Assert.That(fieldCheckbox2!.getOrDefault<string>(_Forms._CheckboxFieldData.name), Is.EqualTo("Checkbox"));
        
        var fieldText2 = field.OfType<IElement>().FirstOrDefault(x => x.getMetaClass()?.Equals(_Forms.TheOne.__TextFieldData) == true);
        Assert.That(fieldText2, Is.Not.Null);
        Assert.That(fieldText2!.getOrDefault<string>(_Forms._TextFieldData.name), Is.EqualTo("Textbox"));
        
        // Confirm that the providerobjects behind each field are not the same
        Assert.That((fieldCheckbox2 as MofObject)?.ProviderObject, Is.Not.Null);
        Assert.That((fieldCheckbox2 as MofObject)?.ProviderObject, Is.Not.EqualTo((fieldCheckbox as MofObject)?.ProviderObject));
        Assert.That((fieldText2 as MofObject)?.ProviderObject, Is.Not.EqualTo((fieldText as MofObject)?.ProviderObject));
    }

    [Test]
    public async Task TestCopyOfReferencesWithCopyingAcrossExtents()
    {
        await InternalTestCopyOfReferences(true, false);
    }

    [Test]
    public async Task TestCopyOfReferencesWithoutCopyingAcrossExtents()
    {
        await InternalTestCopyOfReferences(false, false);
    }

    [Test]
    public async Task TestCopyOfReferencesWithCopyingAcrossExtentsInSameExtent()
    {
        await InternalTestCopyOfReferences(true, true);
    }

    [Test]
    public async Task TestCopyOfReferencesWithoutCopyingAcrossExtentsInSameExtent()
    {
        await InternalTestCopyOfReferences(false, true);
    }

    private static async Task InternalTestCopyOfReferences(bool copyAcrossExtents, bool withinSameExtent)
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

        IUriExtent? mofExtent2;
        if (withinSameExtent)
        {
            mofExtent2 = mofExtent;
        }
        else
        {
            mofExtent2 = (await extentManager.LoadExtent(
                new ExtentLoaderConfigs.InMemoryLoaderConfig_Wrapper(InMemoryObject.TemporaryFactory)
                {
                    extentUri = "dm:///extent2",
                    dropExisting = true
                }.GetWrappedElement())).Extent;
        }

        Assert.That(mofExtent2, Is.Not.Null);

        var factory = new MofFactory(mofExtent!);
        var mofObject = factory.create(_DataViews.TheOne.__QueryStatement);
        var mofResultNode = factory.create(_DataViews.TheOne.Source.__SelectFromAllWorkspacesNode);
        mofResultNode.set(_DataViews._Source._SelectByWorkspaceNode.name, "AllFromWorkspace");
        mofObject.set(_DataViews._QueryStatement.resultNode, mofResultNode);
        
        mofExtent!.elements().add(mofObject);
        
        // Copy it
        
        ObjectCopier.FullDebug = true;
        Logger.Info("Starting COPYING");
        var copiedElement = ObjectCopier.Copy(
            new MofFactory(mofExtent2!),
            mofObject,
            new CopyOption
            {
                PredicateToClone = CopyOption.GetPredicateForUmlCopying(new CopyPredicateParameter {
                    CopyAcrossExtents = copyAcrossExtents
                })
            });
        ObjectCopier.FullDebug = false;
        Logger.Info("Ended COPYING");

        
        // Depending on the copyAcross, different results could happen
        Assert.That(copiedElement, Is.Not.Null);
        Assert.That(copiedElement.getMetaClass()?.Equals(_DataViews.TheOne.__QueryStatement), Is.True);

        var copiedResultNode = copiedElement.getOrDefault<IElement>(_DataViews._QueryStatement.resultNode);
        Assert.That(copiedResultNode, Is.Not.Null);

        if (withinSameExtent)
        {
            // If same extent, then the result node is the same since the reference can be reused
            Assert.That(
                ((mofResultNode as IHasExtent)?.Extent as IUriExtent)?.contextURI(), 
                Is.EqualTo(mofExtent.contextURI()));
            Assert.That(copiedResultNode.GetUri(), Is.EqualTo(mofResultNode.GetUri()));
            Assert.That((copiedResultNode as MofObject)?.ProviderObject, Is.Not.Null);
            Assert.That((copiedResultNode as MofObject)?.ProviderObject, 
                Is.EqualTo((mofResultNode as MofObject)?.ProviderObject));
        }
        else
        {
            // If between different extents, it depends upon the copy across extent flag
            // In case we want to copy the element, then the result node shall be copied
            // In case we want not to copy the element, the result node shall be referenced
            Assert.That(
                (copiedResultNode as MofObject)?.ProviderObject,
                copyAcrossExtents
                    ? Is.Not.EqualTo((mofResultNode as MofObject)?.ProviderObject)
                    : Is.EqualTo((mofResultNode as MofObject)?.ProviderObject));
        }
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
        
        ObjectCopier.FullDebug = true;
        Logger.Info("Starting COPYING");
        var copiedElement = ObjectCopier.Copy(
            new MofFactory(mofOtherExtent!),
            mofObject1,
            new CopyOption
            {
                PredicateToClone = CopyOption.GetPredicateForUmlCopying(new CopyPredicateParameter {
                        CopyAcrossExtents = true
                    })
            });
        
        ObjectCopier.FullDebug = false;
        Logger.Info("Ended COPYING");
        
        mofOtherExtent!.elements().add(copiedElement);
        
        // STEP 3
        // Check that the element and ALL its children are copied and just referencing to new extent.
        // Assert also that the copied object in resultNode is exactly the same as the one in the attribute 'nodes'
        Assert.That((copiedElement as IHasExtent)?.Extent, Is.EqualTo(mofOtherExtent));
        var nodes = copiedElement.getOrDefault<IReflectiveCollection>(_DataViews._QueryStatement.nodes);
        Assert.That(nodes.OfType<IElement>().Count(), Is.EqualTo(3));

        IElement? foundFlattenNodeInNodes = null;
        foreach (var node in nodes.OfType<IElement>())
        {
            Assert.That(node.GetUri()?.Contains("extent2") == true, Is.True);
            if (node.getOrDefault<string>(_DataViews._Row._RowFlattenNode.name) == "RowFlatten")
            {
                foundFlattenNodeInNodes = node;
            }
        }
        
        Assert.That(foundFlattenNodeInNodes, Is.Not.Null);
        
        var checkNodeInputOfFlatten2 = copiedElement.getOrDefault<IElement>(_DataViews._QueryStatement.resultNode);
        Assert.That(checkNodeInputOfFlatten2, Is.Not.Null);
        Assert.That(checkNodeInputOfFlatten2.GetUri()?.Contains("extent2") == true, Is.True);

        // Check that the node and the result node are the same
        Assert.That(foundFlattenNodeInNodes!.GetUri(), Is.EqualTo(checkNodeInputOfFlatten2.GetUri()));
        Assert.That((foundFlattenNodeInNodes as MofObject)?.ProviderObject, Is.Not.Null);
        Assert.That((foundFlattenNodeInNodes as MofObject)?.ProviderObject, Is.EqualTo((checkNodeInputOfFlatten2 as MofObject)?.ProviderObject));
            
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