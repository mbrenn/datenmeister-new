using System.Linq;
using System.Threading.Tasks;
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
using DatenMeister.Forms.FormCreator;
using DatenMeister.Modules.ZipCodeExample;
using DatenMeister.Modules.ZipCodeExample.Model;
using DatenMeister.Types;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Forms
{
    [TestFixture]
    public class FormCreatorTests
    {
        [Test]
        public async Task TestListFormCreatorByMetaClass()
        {
            using var dm = await DatenMeisterTests.GetDatenMeisterScope();
            var workspaceLogic = dm.WorkspaceLogic;
            var scopeStorage = dm.ScopeStorage;

            var zipModel = scopeStorage.Get<ZipCodeModel>();
            Assert.That(zipModel, Is.Not.Null);
            Assert.That(zipModel.ZipCode, Is.Not.Null);

            var formCreator = FormCreator.Create(workspaceLogic, scopeStorage);
            var createdForm =
                formCreator.CreateTableFormForMetaClass(zipModel.ZipCode!, new FormFactoryConfiguration());
            Assert.That(createdForm, Is.Not.Null);
            var fields =
                createdForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._TableForm.field)
                    ?.OfType<IElement>()
                    .ToList();
            Assert.That(fields, Is.Not.Null);
            Assert.That(createdForm.metaclass?.equals(_DatenMeister.TheOne.Forms.__TableForm), Is.True);
            Assert.That(fields!.Any(x =>
                x.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) == nameof(ZipCode.name)));
            Assert.That(fields!.Any(x =>
                x.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) == nameof(ZipCode.zip)));
        }

        [Test]
        public async Task TestDataUrlOfTablesInCollectionForm()
        {
            using var dm = await DatenMeisterTests.GetDatenMeisterScope();
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
            var formsLogic = new FormFactory(workspaceLogic, scopeStorage);
            var collectionForm = formsLogic.CreateCollectionFormForExtent(
                extent, 
                new FormFactoryConfiguration());
            Assert.That(collectionForm, Is.Not.Null);
            var tableForms = FormMethods.GetTableForms(collectionForm!).ToList();
            Assert.That(tableForms.Count, Is.EqualTo(2));

            // Checks, that the dataurl of the first table is just referencing to the extent itself
            var firstTable = tableForms.First();
            Assert.That(firstTable.getOrDefault<bool>(_DatenMeister._Forms._TableForm.noItemsWithMetaClass), Is.True);
            Assert.That(
                firstTable.getOrDefault<string>(_DatenMeister._Forms._TableForm.dataUrl),
                Is.EqualTo("dm:///test"));
            
            // Checks, that the dataurl of the second table is referencing to the specific metaclass
            var secondTable = tableForms.ElementAt(1);
            Assert.That(secondTable.getOrDefault<bool>(_DatenMeister._Forms._TableForm.noItemsWithMetaClass), Is.False);
            Assert.That(
                secondTable.getOrDefault<IElement>(_DatenMeister._Forms._TableForm.metaClass), 
                Is.EqualTo(zipModel.ZipCode));
            Assert.That(
                secondTable.getOrDefault<string>(_DatenMeister._Forms._TableForm.dataUrl),
                Is.EqualTo("dm:///test?metaclass=" + HttpUtility.UrlEncode(zipModel.ZipCode!.GetUri())));
        }

        [Test]
        public async Task TestNoReadOnlyOnDetailFormByMetaClass()
        {
            using var dm = await DatenMeisterTests.GetDatenMeisterScope();
            var workspaceLogic = dm.WorkspaceLogic;
            var scopeStorage = dm.ScopeStorage;

            var zipModel = scopeStorage.Get<ZipCodeModel>();

            var formCreator = FormCreator.Create(workspaceLogic, scopeStorage);
            var createdForm =
                formCreator.CreateObjectFormForMetaClass(zipModel.ZipCode!, new FormFactoryConfiguration());
            var detailForm = FormMethods.GetRowForms(createdForm).FirstOrDefault();
            Assert.That(detailForm, Is.Not.Null);

            var fields = detailForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._RowForm.field);
            Assert.That(fields, Is.Not.Null);

            var any = false;
            foreach (var field in fields.OfType<IElement>())
            {
                if (field.metaclass?.equals(_DatenMeister.TheOne.Forms.__TextFieldData) != true)
                    continue;
                
                // Testing only on TextFields
                any = true;
                Assert.That(field.getOrDefault<bool>(_DatenMeister._Forms._FieldData.isReadOnly), Is.False);
            }

            Assert.That(any, Is.True);
        }

        [Test]
        public async Task TestNoReadOnlyOnDetailFormByObject()
        {
            using var dm = await DatenMeisterTests.GetDatenMeisterScope();
            var workspaceLogic = dm.WorkspaceLogic;
            var scopeStorage = dm.ScopeStorage;

            var zipModel = scopeStorage.Get<ZipCodeModel>();
            var instance = InMemoryObject.CreateEmpty(zipModel.ZipCode!);

            var formCreator = FormCreator.Create(workspaceLogic, scopeStorage);
            var createdForm =
                formCreator.CreateObjectFormForItem(instance, new FormFactoryConfiguration());
            var detailForm = FormMethods.GetRowForms(createdForm).FirstOrDefault();
            Assert.That(detailForm, Is.Not.Null);

            var fields = detailForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._RowForm.field);
            Assert.That(fields, Is.Not.Null);

            var any = false;
            foreach (var field in fields.OfType<IElement>())
            {
                if (field.metaclass?.equals(_DatenMeister.TheOne.Forms.__TextFieldData) != true)
                    continue;
                
                // Testing only on TextFields
                any = true;
                Assert.That(field.getOrDefault<bool>(_DatenMeister._Forms._FieldData.isReadOnly), Is.False);
            }

            Assert.That(any, Is.True);
        }

        [Test]
        public async Task TestExtentTypeSettings()
        {
            using var dm = await DatenMeisterTests.GetDatenMeisterScope();
            var workspaceLogic = dm.WorkspaceLogic;
            var scopeStorage = dm.ScopeStorage;

            var formCreator = new FormFactory(workspaceLogic, scopeStorage);

            var extent = LocalTypeSupport.GetInternalTypeExtent(workspaceLogic);
            Assert.That(extent, Is.Not.Null);

            var createdForm = formCreator.CreateCollectionFormForExtent(extent, new FormFactoryConfiguration()
            {
                AllowFormModifications = true
            });

            Assert.That(createdForm, Is.Not.Null);

            var listForm = FormMethods.GetTableForms(createdForm!).FirstOrDefault(
                x =>
                    x.getOrDefault<IElement>(_DatenMeister._Forms._TableForm.metaClass)
                        ?.Equals(_UML.TheOne.StructuredClassifiers.__Class) == true);
            Assert.That(listForm, Is.Not.Null);

            var defaultTypesForNewElements =
                listForm.getOrDefault<IReflectiveSequence>(_DatenMeister._Forms._TableForm.defaultTypesForNewElements);
            Assert.That(
                defaultTypesForNewElements.OfType<IElement>().Any(
                    x => x?.getOrDefault<IElement>(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass)
                        .Equals(_UML.TheOne.StructuredClassifiers.__Class) == true),
                Is.True);
        }
    }
}