using System.Web;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.Helper;
using DatenMeister.Modules.ZipCodeExample;
using DatenMeister.Modules.ZipCodeExample.Model;
using DatenMeister.Types;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Forms;

[TestFixture]
public class FormCreatorTests
{
    [Test]
    public async Task TestListFormCreatorByMetaClass()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var workspaceLogic = dm.WorkspaceLogic;
        var scopeStorage = dm.ScopeStorage;

        var zipModel = scopeStorage.Get<ZipCodeModel>();
        Assert.That(zipModel, Is.Not.Null);
        Assert.That(zipModel.ZipCode, Is.Not.Null);
        
        var formCreationFactory = new NewFormCreationContextFactory(workspaceLogic, scopeStorage);
        var context = formCreationFactory.Create();

        var createdForm =
            FormCreation.CreateTableFormForMetaClass(
                new TableFormFactoryParameter()
                {
                    MetaClass = zipModel.ZipCode!
                }, context).Form;
        Assert.That(createdForm, Is.Not.Null);
        var fields =
            createdForm.getOrDefault<IReflectiveCollection>(_Forms._TableForm.field)
                ?.OfType<IElement>()
                .ToList();
        Assert.That(fields, Is.Not.Null);
        Assert.That(createdForm!.metaclass?.equals(_Forms.TheOne.__TableForm), Is.True);
        Assert.That(fields!.Any(x =>
            x.getOrDefault<string>(_Forms._FieldData.name) == nameof(ZipCode.name)));
        Assert.That(fields!.Any(x =>
            x.getOrDefault<string>(_Forms._FieldData.name) == nameof(ZipCode.zip)));
    }

    [Test]
    public async Task TestDataUrlOfTablesInCollectionForm()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var workspaceLogic = dm.WorkspaceLogic;
        var scopeStorage = dm.ScopeStorage;
            
        var zipModel = scopeStorage.Get<ZipCodeModel>();
        var extent = new MofUriExtent(new InMemoryProvider(),"dm:///test", scopeStorage);
        workspaceLogic.AddExtent(workspaceLogic.GetDataWorkspace(), extent);
            
        // Add example item
        var factory = new MofFactory(extent);
        var zip = factory.create(zipModel.ZipCode);
        extent.elements().add(zip);
        var unclassified = factory.create(null);
        extent.elements().add(unclassified);
            
        // Ok, data is prepared, now get the collection
        var formCreationFactory = new NewFormCreationContextFactory(workspaceLogic, scopeStorage);
        var context = formCreationFactory.Create();
        
        var collectionForm = FormCreation.CreateCollectionForm(
            new CollectionFormFactoryParameter
            {
                Collection = extent.elements(),
                Extent = extent,
                ExtentTypes = extent.GetConfiguration().ExtentTypes
            },
            context).Form;
        
        Assert.That(collectionForm, Is.Not.Null);
        var tableForms = FormMethods.GetTableForms(collectionForm!).ToList();
        Assert.That(tableForms.Count, Is.EqualTo(2));

        // Checks, that the dataurl of the first table is just referencing to the extent itself
        var firstTable = tableForms.First();
        Assert.That(firstTable.getOrDefault<bool>(_Forms._TableForm.noItemsWithMetaClass), Is.True);
        Assert.That(
            firstTable.getOrDefault<string>(_Forms._TableForm.dataUrl),
            Is.EqualTo("dm:///test"));
            
        // Checks, that the dataurl of the second table is referencing to the specific metaclass
        var secondTable = tableForms.ElementAt(1);
        Assert.That(secondTable.getOrDefault<bool>(_Forms._TableForm.noItemsWithMetaClass), Is.False);
        Assert.That(
            secondTable.getOrDefault<IElement>(_Forms._TableForm.metaClass), 
            Is.EqualTo(zipModel.ZipCode));
        Assert.That(
            secondTable.getOrDefault<string>(_Forms._TableForm.dataUrl),
            Is.EqualTo("dm:///test?metaclass=" + HttpUtility.UrlEncode(zipModel.ZipCode!.GetUri())));
    }

    [Test]
    public async Task TestNoReadOnlyOnDetailFormByMetaClass()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var workspaceLogic = dm.WorkspaceLogic;
        var scopeStorage = dm.ScopeStorage;

        var zipModel = scopeStorage.Get<ZipCodeModel>();

        var formCreationFactory = new NewFormCreationContextFactory(workspaceLogic, scopeStorage);
        var context = formCreationFactory.Create();

        var createdForm =
            FormCreation.CreateObjectForm(
                new ObjectFormFactoryParameter
                {
                    MetaClass = zipModel.ZipCode!
                }, context).Form;
        Assert.That(createdForm, Is.Not.Null);

        var detailForm = FormMethods.GetRowForms(createdForm!).FirstOrDefault();
        Assert.That(detailForm, Is.Not.Null);

        var fields = detailForm.getOrDefault<IReflectiveCollection>(_Forms._RowForm.field);
        Assert.That(fields, Is.Not.Null);

        var any = false;
        foreach (var field in fields.OfType<IElement>())
        {
            if (field.metaclass?.equals(_Forms.TheOne.__TextFieldData) != true)
                continue;

            // Testing only on TextFields
            any = true;
            Assert.That(field.getOrDefault<bool>(_Forms._FieldData.isReadOnly), Is.False);
        }

        Assert.That(any, Is.True);
    }

    [Test]
    public async Task TestNoReadOnlyOnDetailFormByObject()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var workspaceLogic = dm.WorkspaceLogic;
        var scopeStorage = dm.ScopeStorage;

        var zipModel = scopeStorage.Get<ZipCodeModel>();
        var instance = InMemoryObject.CreateEmpty(zipModel.ZipCode!);
        
        var formCreationFactory = new NewFormCreationContextFactory(workspaceLogic, scopeStorage);
        var context = formCreationFactory.Create();
        
        var createdForm =
            FormCreation.CreateObjectForm(
                new ObjectFormFactoryParameter
                {
                    Element =  instance!
                }, context).Form;
        Assert.That(createdForm, Is.Not.Null);
        
        var detailForm = FormMethods.GetRowForms(createdForm!).FirstOrDefault();
        Assert.That(detailForm, Is.Not.Null);

        var fields = detailForm.getOrDefault<IReflectiveCollection>(_Forms._RowForm.field);
        Assert.That(fields, Is.Not.Null);

        var any = false;
        foreach (var field in fields.OfType<IElement>())
        {
            if (field.metaclass?.equals(_Forms.TheOne.__TextFieldData) != true)
                continue;
                
            // Testing only on TextFields
            any = true;
            Assert.That(field.getOrDefault<bool>(_Forms._FieldData.isReadOnly), Is.False);
        }

        Assert.That(any, Is.True);
    }

    [Test]
    public async Task TestThatRowFormIsAlwaysExisting()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var workspaceLogic = dm.WorkspaceLogic;
        var scopeStorage = dm.ScopeStorage;

        var instance = InMemoryObject.CreateEmpty();
        
        var formCreationFactory = new NewFormCreationContextFactory(workspaceLogic, scopeStorage);
        var context = formCreationFactory.Create();
        
        var createdForm =
            FormCreation.CreateObjectForm(
                new ObjectFormFactoryParameter
                {
                    Element =  instance!
                }, context).Form;
        Assert.That(createdForm, Is.Not.Null);
            
        var rowForms = FormMethods.GetRowForms(createdForm!).ToList();
        Assert.That(rowForms.Count, Is.GreaterThan(0));
            
        var tableForms = FormMethods.GetTableForms(createdForm!).ToList();
        Assert.That(tableForms.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task TestThatPackagedElementsHaveATableFormPerInstanceType()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var workspaceLogic = dm.WorkspaceLogic;
        var scopeStorage = dm.ScopeStorage;

        // Prepare instance
        var packageModel = InMemoryObject.CreateEmpty(_UML.TheOne.Packages.__Package);
        var instance1 = InMemoryObject.CreateEmpty(_UML.TheOne.StructuredClassifiers.__Class);
        instance1.set(_UML._StructuredClassifiers._Class.name, "Instance1");
        var instance2 = InMemoryObject.CreateEmpty(_UML.TheOne.StructuredClassifiers.__Connector);
        instance2.set(_UML._StructuredClassifiers._Connector.name, "Instance2");
        var instance3 = InMemoryObject.CreateEmpty(_UML.TheOne.StructuredClassifiers.__Connector);
        instance3.set(_UML._StructuredClassifiers._Connector.name, "Instance3");
        packageModel.set(_UML._Packages._Package.packagedElement, new[] { instance1, instance2, instance3 });

        var formCreationFactory = new NewFormCreationContextFactory(workspaceLogic, scopeStorage);
        var context = formCreationFactory.Create();
        var createdForm =
            FormCreation.CreateObjectForm(
                new ObjectFormFactoryParameter
                {
                    Element = packageModel
                }, context).Form;

        Assert.That(createdForm, Is.Not.Null);

        var rowForms = FormMethods.GetRowForms(createdForm!).ToList();
        Assert.That(rowForms.Count, Is.EqualTo(1));

        // Checks, if the # of tableforms is bigger or equal to 2. 
        // We have to check for higher values since additional table forms might be created by metaclass
        var tableForms = FormMethods.GetTableForms(createdForm!).ToList();
        Assert.That(tableForms.Count, Is.GreaterThanOrEqualTo(2));

        // Now get all table forms within properties of packagedElements
        var tableFormsForPackagedElements = tableForms
            .Where(x => x.getOrDefault<string>(_Forms._TableForm.property) == "packagedElement")
            .ToList();

        // Here, we should now have two table forms, one for the class and one for the connector
        Assert.That(tableFormsForPackagedElements.Count, Is.EqualTo(2));

        // Checks that the metaclass is correctly 
        Assert.That(
            tableFormsForPackagedElements.Any(x =>
                x.getOrDefault<IElement>(_Forms._TableForm.metaClass)
                    ?.equals(_UML.TheOne.StructuredClassifiers.__Class) == true),
            Is.True);
        Assert.That(
            tableFormsForPackagedElements.Any(x =>
                x.getOrDefault<IElement>(_Forms._TableForm.metaClass)
                    ?.equals(_UML.TheOne.StructuredClassifiers.__Connector) == true),
            Is.True);
    }

    [Test]
    public async Task TestExtentTypeSettings()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var workspaceLogic = dm.WorkspaceLogic;
        var scopeStorage = dm.ScopeStorage;

        var formCreationFactory = new NewFormCreationContextFactory(workspaceLogic, scopeStorage);
        var context = formCreationFactory.Create();

        var extent = LocalTypeSupport.GetInternalTypeExtent(workspaceLogic);
        Assert.That(extent, Is.Not.Null);

        var createdForm = FormCreation.CreateCollectionForm(
            new CollectionFormFactoryParameter
            {
                Collection = extent.elements(),
                Extent = extent,
                ExtentTypes = extent.GetConfiguration().ExtentTypes
            }, context).Form;
        Assert.That(createdForm, Is.Not.Null);

        var listForm = FormMethods.GetTableForms(createdForm!).FirstOrDefault(
            x =>
                x.getOrDefault<IElement>(_Forms._TableForm.metaClass)
                    ?.Equals(_UML.TheOne.StructuredClassifiers.__Class) == true);
        Assert.That(listForm, Is.Not.Null);

        var defaultTypesForNewElements =
            listForm.getOrDefault<IReflectiveSequence>(_Forms._TableForm.defaultTypesForNewElements);
        Assert.That(
            defaultTypesForNewElements.OfType<IElement>().Any(
                x => x.getOrDefault<IElement>(_Forms._DefaultTypeForNewElement.metaClass)
                    .Equals(_UML.TheOne.StructuredClassifiers.__Class) == true),
            Is.True);
    }
}