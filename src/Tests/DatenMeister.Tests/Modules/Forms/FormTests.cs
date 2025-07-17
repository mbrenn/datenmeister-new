using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Extent.Manager;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.Helper;
using DatenMeister.Forms.ObjectForm;
using DatenMeister.Forms.RowForm;
using DatenMeister.Forms.TableForms;
using DatenMeister.Types;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Forms;

[TestFixture]
public class FormTests
{
    [Test]
    public void TestProtocolOneForm()
    {
        var element = InMemoryObject.CreateEmpty();
        var formContext = new FormCreationResultOneForm()
        {
            Form = element
        };
            
        formContext.AddToFormCreationProtocol("test");
        Assert.That(
            element.getOrDefault<string>(_Forms._Form.creationProtocol)?.Contains("test") == true,
            Is.True);
            
        formContext.AddToFormCreationProtocol("hallo");
        Assert.That(
            element.getOrDefault<string>(_Forms._Form.creationProtocol)?.Contains("test") == true,
            Is.True);
        Assert.That(
            element.getOrDefault<string>(_Forms._Form.creationProtocol)?.Contains("hallo") == true,
            Is.True);
    }
    
    [Test]
    public void TestProtocolMultipleForm()
    {
        var element = InMemoryObject.CreateEmpty();
        var formContext = new FormCreationResultMultipleForms()
        {
            Forms = [element]
        };
            
        formContext.AddToFormCreationProtocol("test");
        Assert.That(
            element.getOrDefault<string>(_Forms._Form.creationProtocol)?.Contains("test") == true,
            Is.True);
            
        formContext.AddToFormCreationProtocol("hallo");
        Assert.That(
            element.getOrDefault<string>(_Forms._Form.creationProtocol)?.Contains("test") == true,
            Is.True);
        Assert.That(
            element.getOrDefault<string>(_Forms._Form.creationProtocol)?.Contains("hallo") == true,
            Is.True);
    }
        
    [Test]
    public async Task TestValidator()
    {
        await using var scope = await DatenMeisterTests.GetDatenMeisterScope();
        
        var extent = XmiExtensions.CreateXmiExtent("dm:///test");
        var factory = new MofFactory(extent);
        var context = new FormCreationContext
        {
            Global = new FormCreationContext.GlobalContext
            {
                Factory = factory
            }
        };

        var validateTableOrRowForm = new ValidateTableOrRowForm();
        var validateObjectOrCollectionForm = new ValidateObjectOrCollectionForm();

        var form = factory.create(_Forms.TheOne.__RowForm);

        var field1 = factory.create(_Forms.TheOne.__FieldData);
        field1.set("name", "A");
        var field2 = factory.create(_Forms.TheOne.__FieldData);
        field2.set("name", "B");
        var field3 = factory.create(_Forms.TheOne.__FieldData);
        field3.set("name", "C");
        var field4 = factory.create(_Forms.TheOne.__FieldData);
        field4.set("name", "D");
        var field5 = factory.create(_Forms.TheOne.__FieldData);
        field5.set("name", "A");

        form.set(_Forms._RowForm.field, new[] {field1, field2, field3, field4});

        var result = new FormCreationResultMultipleForms
        {
            Forms = [form],
            IsMainContentCreated = true
        };
        
        validateTableOrRowForm.CreateTableForm(new TableFormFactoryParameter(), context, result);
        
        Assert.That(context.LocalScopeStorage.Get<ValidationResult>().IsInvalid, Is.False);
        context.LocalScopeStorage.Remove<ValidationResult>();

        form.set(_Forms._RowForm.field, new[] {field1, field2, field3, field4, field5});
        
        validateTableOrRowForm.CreateTableForm(new TableFormFactoryParameter(), context, result);
        Assert.That(context.LocalScopeStorage.Get<ValidationResult>().IsInvalid, Is.True);
        context.LocalScopeStorage.Remove<ValidationResult>();
        
        var resultSingle = new FormCreationResultOneForm()
        {
            Form = form,
            IsMainContentCreated = true
        };
        
        var newForm = factory.create(_Forms.TheOne.__CollectionForm);
        newForm.set(_Forms._CollectionForm.tab, new[] {form});
        resultSingle.Form = newForm;
        validateObjectOrCollectionForm.CreateCollectionForm(new CollectionFormFactoryParameter(), context, resultSingle);
        Assert.That(context.LocalScopeStorage.Get<ValidationResult>().IsInvalid, Is.True);
        context.LocalScopeStorage.Remove<ValidationResult>();

        form.set(_Forms._RowForm.field, new[] {field1, field2, field3, field4});
        
        validateObjectOrCollectionForm.CreateCollectionForm(new CollectionFormFactoryParameter(), context, resultSingle);
        Assert.That(context.LocalScopeStorage.Get<ValidationResult>().IsInvalid, Is.False);
        context.LocalScopeStorage.Remove<ValidationResult>();    
    }

    [Test]
    public async Task TestAutoExtensionOfDropDownValueReferences()
    {
        await using var scope = await DatenMeisterTests.GetDatenMeisterScope();

        var localTypeSupport = new LocalTypeSupport(scope.WorkspaceLogic, scope.ScopeStorage);

        var comparisonType = localTypeSupport.InternalTypes.element(
            "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.ComparisonType");

        var extent = XmiExtensions.CreateXmiExtent("dm:///test");
        var factory = new MofFactory(extent);

        var form = factory.create(_Forms.TheOne.__RowForm);
        var field1 = factory.create(_Forms.TheOne.__DropDownFieldData);

        Assert.That(comparisonType, Is.Not.Null);

        field1.set(_Forms._DropDownFieldData.valuesByEnumeration,
            comparisonType);
        form.set(_Forms._RowForm.field, new[] {field1});

        var valuesBefore =
            field1.getOrDefault<IReflectiveCollection>(_Forms._DropDownFieldData.values);
        Assert.That(valuesBefore, Is.Null);
        
        var context = new FormCreationContext
        {
            Global = new FormCreationContext.GlobalContext { Factory = new MofFactory(form) }
        };
        
        var result = new FormCreationResultMultipleForms()
        {
            Forms = [form],
            IsMainContentCreated = true
        };

        var expand = new ExpandDropDownOfValueReference();
        expand.CreateRowForm(new RowFormFactoryParameter(), context, result);

        var values = field1.getOrDefault<IReflectiveCollection>(_Forms._DropDownFieldData.values);
        Assert.That(values, Is.Not.Null);

        Assert.That(values.OfType<IElement>().Any(
            x =>
                x.getOrDefault<string>(_Forms._ValuePair.name) ==
                _FastViewFilters._ComparisonType.GreaterThan));
    }

    [Test]
    public void TestRemoveDuplicateDefaultNewTypes()
    {
        var extent = new MofUriExtent(new InMemoryProvider(), "dm:///test", null);
        var factory = new MofFactory(extent);

        var form = factory.create(_Forms.TheOne.__TableForm);
        var defaultNewType1 = factory.create(_Forms.TheOne.__DefaultTypeForNewElement);
        defaultNewType1.set(_Forms._DefaultTypeForNewElement.metaClass, _Actions.TheOne.__Action);
        var defaultNewType2 = factory.create(_Forms.TheOne.__DefaultTypeForNewElement);
        defaultNewType2.set(_Forms._DefaultTypeForNewElement.metaClass, _Actions.TheOne.__ActionSet);
        var defaultNewType3 = factory.create(_Forms.TheOne.__DefaultTypeForNewElement);
        defaultNewType3.set(_Forms._DefaultTypeForNewElement.metaClass, _Actions.TheOne.__ClearCollectionAction);
        var defaultNewType4 = factory.create(_Forms.TheOne.__DefaultTypeForNewElement);
        defaultNewType4.set(_Forms._DefaultTypeForNewElement.metaClass, _Actions.TheOne.__ActionSet);
        var defaultNewType5 = factory.create(_Forms.TheOne.__DefaultTypeForNewElement);
        defaultNewType5.set(_Forms._DefaultTypeForNewElement.metaClass, _Actions.TheOne.__ClearCollectionAction);
            
        form.set(_Forms._TableForm.defaultTypesForNewElements, 
            new[]{defaultNewType1, defaultNewType2, defaultNewType3, defaultNewType4, defaultNewType5});

        var fields = form.getOrDefault<IReflectiveCollection>(_Forms._TableForm.defaultTypesForNewElements);
        Assert.That(fields.Count(), Is.EqualTo(5));

        var context = new FormCreationContext
        {
            Global = new FormCreationContext.GlobalContext { Factory = new MofFactory(form) }
        };
        
        var result = new FormCreationResultMultipleForms()
        {
            Forms = [form],
            IsMainContentCreated = true
        };

        
        var removeDuplicateHandler = new RemoveDuplicateDefaultTypes();
        removeDuplicateHandler.CreateTableForm(
            new TableFormFactoryParameter(),
            context,
            result);
        
        var fields2 = form.getOrDefault<IReflectiveCollection>(_Forms._TableForm.defaultTypesForNewElements).OfType<IObject>()
            .Select(x=>x.getOrDefault<IObject>(_Forms._DefaultTypeForNewElement.metaClass))
            .ToList();
        Assert.That(fields2.Count, Is.EqualTo(3));
        Assert.That(fields2.Any (x=> x.equals(_Actions.TheOne.__Action)));
        Assert.That(fields2.Any (x=> x.equals(_Actions.TheOne.__ActionSet)));
        Assert.That(fields2.Any (x=> x.equals(_Actions.TheOne.__ClearCollectionAction)));
    }

    [Test]
    public async Task TestGetViewModes()
    {
        await using var scope = await DatenMeisterTests.GetDatenMeisterScope();
        var formMethods = new FormMethods(scope.WorkspaceLogic);
        var viewModeMethods = new ViewModeMethods(scope.WorkspaceLogic);

        // Check, if default view mode is in
        var viewModes = viewModeMethods.GetViewModes().ToList();
        Assert.That(
            viewModes.Any(x => x.getOrDefault<string>(_Forms._ViewMode.id) == ViewModes.Default),
            Is.True);
        Assert.That(
            viewModes.Any(x => x.getOrDefault<string>(_Forms._ViewMode.id) == "Test"),
            Is.False);
            
        // Check, if we can add and find a new view mode
        var userFormExtent = formMethods.GetUserFormExtent();
        var factory = new MofFactory(userFormExtent);

        var viewMode = factory.create(_Forms.TheOne.__ViewMode);
        viewMode.set(_Forms._ViewMode.id, "Test");
        userFormExtent.elements().add(viewMode);
            
        viewModes = viewModeMethods.GetViewModes().ToList();
        Assert.That(
            viewModes.Any(x => x.getOrDefault<string>(_Forms._ViewMode.id) == "Test"),
            Is.True);
    }
}