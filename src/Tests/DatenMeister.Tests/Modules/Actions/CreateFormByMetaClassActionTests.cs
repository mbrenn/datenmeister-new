
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using NUnit.Framework;

#nullable disable

namespace DatenMeister.Tests.Modules.Actions
{
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
        public async Task TestCreationOfDetailForm()
        {
            await using var dm = DatenMeisterTests.GetDatenMeisterScope();
            Initialize(dm);

            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CreateFormByMetaClass);
            action.set(
                _DatenMeister._Actions._CreateFormByMetaClass.creationMode,
                CreateFormByMetaclassCreationMode.Detail);
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
        public async Task TestCreationOfListForm()
        {
            await using var dm = DatenMeisterTests.GetDatenMeisterScope();
            Initialize(dm);

            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CreateFormByMetaClass);
            action.set(
                _DatenMeister._Actions._CreateFormByMetaClass.creationMode,
                CreateFormByMetaclassCreationMode.List);
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
        public async Task TestCreationOfDetailAndListForm()
        {
            await using var dm = DatenMeisterTests.GetDatenMeisterScope();
            Initialize(dm);

            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CreateFormByMetaClass);
            action.set(
                _DatenMeister._Actions._CreateFormByMetaClass.creationMode,
                CreateFormByMetaclassCreationMode.DetailList);
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
        public async Task TestCreationOfDetailAssociationForm()
        {
            await using var dm = DatenMeisterTests.GetDatenMeisterScope();
            Initialize(dm);

            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__CreateFormByMetaClass);
            action.set(
                _DatenMeister._Actions._CreateFormByMetaClass.creationMode,
                CreateFormByMetaclassCreationMode.DetailAssociation);
            action.set(_DatenMeister._Actions._CreateFormByMetaClass.metaClass, _zipModel.ZipCode);

            var actionLogic = new ActionLogic(_workspaceLogic, _scopeStorage);
            await actionLogic.ExecuteAction(action);
            
            
            var updatedElements = _formMethods.GetUserFormExtent().elements().OfType<IObject>().ToList();
            var newElements = updatedElements.Where(x => !_existingElements.Contains(x)).ToList();

            var formFactory = new FormFactory(_workspaceLogic, _scopeStorage);
            var detailForm = formFactory.CreateDetailFormForItem(_zipModel.ZipCode, new FormFactoryConfiguration());
            
            Assert.That(detailForm.equals(newElements.First() as IObject));
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
    
}