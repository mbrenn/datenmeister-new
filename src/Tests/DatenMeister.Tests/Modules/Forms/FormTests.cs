using System.Linq;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Extent.Manager;
using DatenMeister.Forms;
using DatenMeister.Types;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Forms
{
    [TestFixture]
    public class FormTests
    {
        [Test]
        public void TestValidator()
        {
            using var scope = DatenMeisterTests.GetDatenMeisterScope();

            var extent = XmiExtensions.CreateXmiExtent("dm:///test");

            var factory = new MofFactory(extent);

            var form = factory.create(_DatenMeister.TheOne.Forms.__DetailForm);

            var field1 = factory.create(_DatenMeister.TheOne.Forms.__FieldData);
            field1.set("name", "A");
            var field2 = factory.create(_DatenMeister.TheOne.Forms.__FieldData);
            field2.set("name", "B");
            var field3 = factory.create(_DatenMeister.TheOne.Forms.__FieldData);
            field3.set("name", "C");
            var field4 = factory.create(_DatenMeister.TheOne.Forms.__FieldData);
            field4.set("name", "D");
            var field5 = factory.create(_DatenMeister.TheOne.Forms.__FieldData);
            field5.set("name", "A");

            form.set(_DatenMeister._Forms._DetailForm.field, new[] {field1, field2, field3, field4});

            Assert.That(FormMethods.ValidateForm(form), Is.True);

            form.set(_DatenMeister._Forms._DetailForm.field, new[] {field1, field2, field3, field4, field5});
            Assert.That(FormMethods.ValidateForm(form), Is.False);

            var newForm = factory.create(_DatenMeister.TheOne.Forms.__ExtentForm);
            newForm.set(_DatenMeister._Forms._ExtentForm.tab, new[] {form});
            Assert.That(FormMethods.ValidateForm(newForm), Is.False);

            form.set(_DatenMeister._Forms._DetailForm.field, new[] {field1, field2, field3, field4});
            Assert.That(FormMethods.ValidateForm(newForm), Is.True);
        }

        [Test]
        public void TestAutoExtensionOfDropDownValueReferences()
        {
            using var scope = DatenMeisterTests.GetDatenMeisterScope();

            var localTypeSupport = new LocalTypeSupport(scope.WorkspaceLogic, scope.ScopeStorage);

            var comparisonType = localTypeSupport.InternalTypes.element(
                "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.ComparisonType");

            var extent = XmiExtensions.CreateXmiExtent("dm:///test");
            var factory = new MofFactory(extent);

            var form = factory.create(_DatenMeister.TheOne.Forms.__DetailForm);
            var field1 = factory.create(_DatenMeister.TheOne.Forms.__DropDownFieldData);

            Assert.That(comparisonType, Is.Not.Null);

            field1.set(_DatenMeister._Forms._DropDownFieldData.valuesByEnumeration,
                comparisonType);
            form.set(_DatenMeister._Forms._DetailForm.field, new[] {field1});

            var formFactory = scope.Resolve<FormFactory>();
            Assert.That(formFactory, Is.Not.Null);

            var valuesBefore =
                field1.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._DropDownFieldData.values);
            Assert.That(valuesBefore, Is.Null);

            formFactory.ExpandDropDownValuesOfValueReference(form);

            var values = field1.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._DropDownFieldData.values);
            Assert.That(values, Is.Not.Null);

            Assert.That(values.OfType<IElement>().Any(
                x =>
                    x.getOrDefault<string>(_DatenMeister._Forms._ValuePair.name) ==
                    _DatenMeister._FastViewFilters._ComparisonType.GreaterThan));
        }
    }
}