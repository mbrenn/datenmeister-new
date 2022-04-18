using System.Linq;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
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
        public void TestProtocol()
        {
            var element = InMemoryObject.CreateEmpty();
            
            FormMethods.AddToFormCreationProtocol(element, "test");
            Assert.That(
                element.getOrDefault<string>(_DatenMeister._Forms._Form.creationProtocol)?.Contains("test") == true,
                Is.True);
            
            FormMethods.AddToFormCreationProtocol(element, "hallo");
            Assert.That(
                element.getOrDefault<string>(_DatenMeister._Forms._Form.creationProtocol)?.Contains("test") == true,
                Is.True);
            Assert.That(
                element.getOrDefault<string>(_DatenMeister._Forms._Form.creationProtocol)?.Contains("hallo") == true,
                Is.True);
        }
        
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

            FormMethods.ExpandDropDownValuesOfValueReference(form);

            var values = field1.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._DropDownFieldData.values);
            Assert.That(values, Is.Not.Null);

            Assert.That(values.OfType<IElement>().Any(
                x =>
                    x.getOrDefault<string>(_DatenMeister._Forms._ValuePair.name) ==
                    _DatenMeister._FastViewFilters._ComparisonType.GreaterThan));
        }

        [Test]
        public void TestRemoveDuplicateDefaultNewTypes()
        {
            var workspaceLogic = WorkspaceLogic.Create(new WorkspaceData());
            var extent = new MofUriExtent(new InMemoryProvider(), "dm:///test", null);
            var factory = new MofFactory(extent);

            var form = factory.create(_DatenMeister.TheOne.Forms.__ListForm);
            var defaultNewType1 = factory.create(_DatenMeister.TheOne.Forms.__DefaultTypeForNewElement);
            defaultNewType1.set(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass, _DatenMeister.TheOne.Actions.__Action);
            var defaultNewType2 = factory.create(_DatenMeister.TheOne.Forms.__DefaultTypeForNewElement);
            defaultNewType2.set(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass, _DatenMeister.TheOne.Actions.__ActionSet);
            var defaultNewType3 = factory.create(_DatenMeister.TheOne.Forms.__DefaultTypeForNewElement);
            defaultNewType3.set(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass, _DatenMeister.TheOne.Actions.__ClearCollectionAction);
            var defaultNewType4 = factory.create(_DatenMeister.TheOne.Forms.__DefaultTypeForNewElement);
            defaultNewType4.set(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass, _DatenMeister.TheOne.Actions.__ActionSet);
            var defaultNewType5 = factory.create(_DatenMeister.TheOne.Forms.__DefaultTypeForNewElement);
            defaultNewType5.set(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass, _DatenMeister.TheOne.Actions.__ClearCollectionAction);
            
            form.set(_DatenMeister._Forms._ListForm.defaultTypesForNewElements, 
                new[]{defaultNewType1, defaultNewType2, defaultNewType3, defaultNewType4, defaultNewType5});

            var fields = form.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ListForm.defaultTypesForNewElements);
            Assert.That(fields.Count(), Is.EqualTo(5));
            
            FormMethods.RemoveDuplicatingDefaultNewTypes(form);
            var fields2 = form.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ListForm.defaultTypesForNewElements).OfType<IObject>()
                .Select(x=>x.getOrDefault<IObject>(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass))
                .ToList();
            Assert.That(fields2.Count, Is.EqualTo(3));
            Assert.That(fields2.Any (x=> x.equals(_DatenMeister.TheOne.Actions.__Action)));
            Assert.That(fields2.Any (x=> x.equals(_DatenMeister.TheOne.Actions.__ActionSet)));
            Assert.That(fields2.Any (x=> x.equals(_DatenMeister.TheOne.Actions.__ClearCollectionAction)));
        }
        
    }
}