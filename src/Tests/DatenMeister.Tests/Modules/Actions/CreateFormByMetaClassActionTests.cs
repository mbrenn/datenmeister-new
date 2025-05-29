using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DependencyInjection;
using DatenMeister.Forms;
using DatenMeister.Modules.ZipCodeExample;
using DatenMeister.Modules.ZipCodeExample.Model;
using NUnit.Framework;

#nullable disable

namespace DatenMeister.Tests.Modules.Actions;

[TestFixture]
public class CreateFormByMetaClassActionTests
{
    private IWorkspaceLogic _workspaceLogic;
    private IScopeStorage _scopeStorage;
    private List<IObject> _existingElements;
    private FormMethods _formMethods;
    private ZipCodeModel _zipModel;

    /// <summary>
    /// This test just performs the creation of a detail form.
    /// Here, it is checked that an extent form will be created containing the detail form
    /// </summary>
    [Test]
    public async Task TestCreationOfObjectForm()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        Initialize(dm);

        var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CreateFormByMetaClass);
        action.set(
            _DatenMeister._Actions._CreateFormByMetaClass.creationMode,
            CreateFormByMetaclassCreationMode.Object);
        action.set(_DatenMeister._Actions._CreateFormByMetaClass.metaClass, _zipModel.ZipCode);

        var actionLogic = new ActionLogic(_workspaceLogic, _scopeStorage);
        await actionLogic.ExecuteAction(action);

        var updatedElements = _formMethods.GetUserFormExtent().elements().OfType<IObject>().ToList();
        var newElements = updatedElements.Where(x => !_existingElements.Contains(x)).ToList();
                
        // Check, that one item is created
        Assert.That(newElements.Count, Is.EqualTo(1));

        var form = newElements.First() as IElement;
        Assert.That(form, Is.Not.Null);
        Assert.That(form!.metaclass, Is.Not.Null);
        Assert.That(form.metaclass!.equals(_DatenMeister.TheOne.Forms.__ObjectForm), Is.True);

        var detailForms = form.getOrDefault<IReflectiveSequence>(_DatenMeister._Forms._CollectionForm.tab);
        Assert.That(detailForms, Is.Not.Null);
        Assert.That(detailForms.Count(), Is.EqualTo(1));

        var detailForm = detailForms.First() as IElement;
        Assert.That(detailForm, Is.Not.Null);
        Assert.That(detailForm!.metaclass, Is.Not.Null);
        Assert.That(detailForm!.metaclass!.equals(_DatenMeister.TheOne.Forms.__RowForm));
    }
        
    /// <summary>
    /// This test just performs the creation of a detail form.
    /// Here, it is checked that an extent form will be created containing the detail form
    /// </summary>
    [Test]
    public async Task TestCreationOfCollectionForm()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        Initialize(dm);

        var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CreateFormByMetaClass);
        action.set(
            _DatenMeister._Actions._CreateFormByMetaClass.creationMode,
            CreateFormByMetaclassCreationMode.Collection);
        action.set(_DatenMeister._Actions._CreateFormByMetaClass.metaClass, _zipModel.ZipCode);

        var actionLogic = new ActionLogic(_workspaceLogic, _scopeStorage);
        await actionLogic.ExecuteAction(action);

        var updatedElements = _formMethods.GetUserFormExtent().elements().OfType<IObject>().ToList();
        var newElements = updatedElements.Where(x => !_existingElements.Contains(x)).ToList();
                
        // Check, that one item is created
        Assert.That(newElements.Count, Is.EqualTo(1));

        var form = newElements.First() as IElement;
        Assert.That(form, Is.Not.Null);
        Assert.That(form!.metaclass, Is.Not.Null);
        Assert.That(form.metaclass!.equals(_DatenMeister.TheOne.Forms.__CollectionForm), Is.True);

        var detailForms = form.getOrDefault<IReflectiveSequence>(_DatenMeister._Forms._CollectionForm.tab);
        Assert.That(detailForms, Is.Not.Null);
        Assert.That(detailForms.Count(), Is.EqualTo(1));

        var detailForm = detailForms.First() as IElement;
        Assert.That(detailForm, Is.Not.Null);
        Assert.That(detailForm!.metaclass, Is.Not.Null);
        Assert.That(detailForm!.metaclass!.equals(_DatenMeister.TheOne.Forms.__RowForm));
    }
        
    /// <summary>
    /// This test just performs the creation of a detail form.
    /// Here, it is checked that an extent form will be created containing the detail form
    /// </summary>
    [Test]
    public async Task TestCreationOfObjectAndCollectionForm()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        Initialize(dm);

        var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CreateFormByMetaClass);
        action.set(
            _DatenMeister._Actions._CreateFormByMetaClass.creationMode,
            CreateFormByMetaclassCreationMode.ObjectCollection);
        action.set(_DatenMeister._Actions._CreateFormByMetaClass.metaClass, _zipModel.ZipCode);

        var actionLogic = new ActionLogic(_workspaceLogic, _scopeStorage);
        await actionLogic.ExecuteAction(action);

        var updatedElements = _formMethods.GetUserFormExtent().elements().OfType<IObject>().ToList();
        var newElements = updatedElements.Where(x => !_existingElements.Contains(x)).ToList();
                
        // Check, that one item is created
        Assert.That(newElements.Count, Is.EqualTo(2));
    }

    /// <summary>
    /// This test just performs the creation of a detail form.
    /// Here, it is checked that an extent form will be created containing the detail form
    /// </summary>
    [Test]
    public async Task TestCreationOfObjectAssociationForm()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        Initialize(dm);
            
        // First, check that the form for the demo class is autogenerated (and not polluted due to any other topic)
        var formFactory = new FormFactory(_workspaceLogic, _scopeStorage);
        var autoGeneratedForm = 
            formFactory.CreateObjectFormForMetaClass(_zipModel.ZipCodeWithState, new FormFactoryConfiguration());
        Assert.That(autoGeneratedForm.getOrDefault<bool>(_DatenMeister._Forms._ObjectForm.isAutoGenerated), Is.True);

        // Second, execute the creation of the association 
        var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CreateFormByMetaClass);
        action.set(
            _DatenMeister._Actions._CreateFormByMetaClass.creationMode,
            CreateFormByMetaclassCreationMode.ObjectAssociation);
        action.set(_DatenMeister._Actions._CreateFormByMetaClass.metaClass, _zipModel.ZipCodeWithState);

        var actionLogic = new ActionLogic(_workspaceLogic, _scopeStorage);
        await actionLogic.ExecuteAction(action);

        // Figure out that the new form is created
        var updatedElements = _formMethods.GetUserFormExtent().elements().OfType<IObject>().ToList();
        var newElements = updatedElements.Where(x => !_existingElements.Contains(x)).ToList();

        // Now figure out that object form can be found by mapping
        var objectForm = formFactory.CreateObjectFormForMetaClass(_zipModel.ZipCodeWithState, new FormFactoryConfiguration());

        var first = 
            newElements
                .OfType<IElement>()
                .Single(x => x.metaclass?.equals(_DatenMeister.TheOne.Forms.__ObjectForm) == true);
        var countDetailForm = objectForm.getOrDefault<IReflectiveSequence>(_DatenMeister._Forms._ObjectForm.tab)
            .Count();
        var countNewForm = first.getOrDefault<IReflectiveSequence>(_DatenMeister._Forms._ObjectForm.tab).Count();
        Assert.That(countDetailForm, Is.EqualTo(countNewForm));
        Assert.That(first.metaclass?.equals(_DatenMeister.TheOne.Forms.__ObjectForm), Is.True);
        Assert.That(
            first.getOrDefault<IReflectiveSequence>(_DatenMeister._Forms._ObjectForm.tab)
                .OfType<IElement>().Single()
                .metaclass?
                .equals(_DatenMeister.TheOne.Forms.__RowForm),
            Is.True);

        var rowForm = first.getOrDefault<IReflectiveSequence>(_DatenMeister._Forms._ObjectForm.tab)
            .OfType<IElement>().Single();
        Assert.That(rowForm.getOrDefault<IReflectiveSequence>(_DatenMeister._Forms._RowForm.field)
            .OfType<IElement>().Any(x =>
                x.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) == nameof(ZipCode.zip)));
    }

    private void Initialize(IDatenMeisterScope dm)
    {
        _workspaceLogic = dm.WorkspaceLogic;
        _scopeStorage = dm.ScopeStorage;

        _zipModel = _scopeStorage.Get<ZipCodeModel>();
        Assert.That(_zipModel, Is.Not.Null);
        Assert.That(_zipModel.ZipCode, Is.Not.Null);

        _formMethods = new FormMethods(_workspaceLogic, _scopeStorage);
        _existingElements = _formMethods.GetUserFormExtent().elements().OfType<IObject>().ToList();
    }
}