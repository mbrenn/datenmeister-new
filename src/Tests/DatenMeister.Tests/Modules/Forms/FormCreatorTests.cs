using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
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
        public void TestListFormCreatorByMetaClass()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();
            var workspaceLogic = dm.WorkspaceLogic;
            var scopeStorage = dm.ScopeStorage;

            var zipModel = scopeStorage.Get<ZipCodeModel>();
            Assert.That(zipModel, Is.Not.Null);
            Assert.That(zipModel.ZipCode, Is.Not.Null);

            var formCreator = FormCreator.Create(workspaceLogic, scopeStorage);
            var createdForm =
                formCreator.CreateListFormForMetaClass(zipModel.ZipCode!, new FormFactoryConfiguration());
            Assert.That(createdForm, Is.Not.Null);
            var fields =
                createdForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ListForm.field)
                    ?.OfType<IElement>()
                    .ToList();
            Assert.That(fields, Is.Not.Null);
            Assert.That(createdForm.metaclass?.equals(_DatenMeister.TheOne.Forms.__ListForm), Is.True);
            Assert.That(fields!.Any(x =>
                x.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) == nameof(ZipCode.name)));
            Assert.That(fields!.Any(x =>
                x.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) == nameof(ZipCode.zip)));
        }

        [Test]
        public void TestExtentTypeSettings()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();
            var workspaceLogic = dm.WorkspaceLogic;
            var scopeStorage = dm.ScopeStorage;

            var formCreator = new FormFactory(workspaceLogic, scopeStorage);

            var extent = LocalTypeSupport.GetInternalTypeExtent(workspaceLogic);
            Assert.That(extent, Is.Not.Null);

            var createdForm = formCreator.CreateExtentFormForExtent(extent, new FormFactoryConfiguration()
            {
                AllowFormModifications = true
            });

            Assert.That(createdForm, Is.Not.Null);

            var listForm = FormMethods.GetListForms(createdForm!).FirstOrDefault(
                x =>
                    x.getOrDefault<IElement>(_DatenMeister._Forms._ListForm.metaClass)
                        ?.Equals(_UML.TheOne.StructuredClassifiers.__Class) == true);
            Assert.That(listForm, Is.Not.Null);

            var defaultTypesForNewElements =
                listForm.getOrDefault<IReflectiveSequence>(_DatenMeister._Forms._ListForm.defaultTypesForNewElements);
            Assert.That(
                defaultTypesForNewElements.OfType<IElement>().Any(
                    x => x?.getOrDefault<IElement>(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass)
                        .Equals(_UML.TheOne.StructuredClassifiers.__Class) == true),
                Is.True);
        }
    }
}