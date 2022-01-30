using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Forms;
using DatenMeister.Forms.FormCreator;
using DatenMeister.Modules.ZipCodeExample;
using DatenMeister.Modules.ZipCodeExample.Model;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Forms
{
    [TestFixture]
    public class FormCreatorTests
    {
        [Test]
        public void TestListFormCreatorByMetaClass()
        {
            var dm = DatenMeisterTests.GetDatenMeisterScope();
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
    }
}