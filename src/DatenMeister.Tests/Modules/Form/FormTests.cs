using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.Forms;
using DatenMeister.Runtime.Workspaces;
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

            var form = factory.create(_FormAndFields.TheOne.__DetailForm);

            var field1 = factory.create(_FormAndFields.TheOne.__FieldData);
            field1.set("name", "A");
            var field2 = factory.create(_FormAndFields.TheOne.__FieldData);
            field2.set("name", "B");
            var field3 = factory.create(_FormAndFields.TheOne.__FieldData);
            field3.set("name", "C");
            var field4 = factory.create(_FormAndFields.TheOne.__FieldData);
            field4.set("name", "D");
            var field5 = factory.create(_FormAndFields.TheOne.__FieldData);
            field5.set("name", "A");

            form.set(_FormAndFields._DetailForm.field, new[] {field1, field2, field3, field4});

            Assert.That(FormMethods.ValidateForm(form), Is.True);

            form.set(_FormAndFields._DetailForm.field, new[] {field1, field2, field3, field4, field5});
            Assert.That(FormMethods.ValidateForm(form), Is.False);

            var newForm = factory.create(_FormAndFields.TheOne.__ExtentForm);
            newForm.set(_FormAndFields._ExtentForm.tab, new[]{form});
            Assert.That(FormMethods.ValidateForm(newForm), Is.False);
            
            form.set(_FormAndFields._DetailForm.field, new[] {field1, field2, field3, field4});
            Assert.That(FormMethods.ValidateForm(newForm), Is.True);
        }
    }
}