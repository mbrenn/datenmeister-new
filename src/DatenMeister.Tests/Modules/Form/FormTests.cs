using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Models;
using DatenMeister.Forms;
using DatenMeister.Integration;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Form
{
    [TestFixture]
    public class FormTests
    {
        [Test]
        public void TestValidator()
        {
            using var scope = DatenMeisterTests.GetDatenMeisterScope();

            var extent = scope.CreateXmiExtent("dm:///test");

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
            newForm.set(_DatenMeister._Forms._ExtentForm.tab, new[]{form});
            Assert.That(FormMethods.ValidateForm(newForm), Is.False);
            
            form.set(_DatenMeister._Forms._DetailForm.field, new[] {field1, field2, field3, field4});
            Assert.That(FormMethods.ValidateForm(newForm), Is.True);
        }
    }
}