using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Forms;
using DatenMeister.Forms.FormCreator;
using NUnit.Framework;

namespace DatenMeister.Tests.Web
{
    [TestFixture]
    public class TestAutoColumnCreator1
    {
        [Test]
        public void TestSimpleAutoColumn()
        {
            var property1 = "zip";
            var property2 = "location";
            var extent = new MofUriExtent(new InMemoryProvider(), "dm:///");
            var factory = new MofFactory(extent);
            var mofObject = factory.create(null);
            mofObject.set(property1, "55130");
            mofObject.set(property2, "Mainz");

            var mofObject2 = factory.create(null);
            mofObject2.set(property1, "65474");
            mofObject2.set(property2, "Bischofsheim");
            
            extent.elements().add(mofObject);
            extent.elements().add(mofObject2);
            var creator = FormCreator.Create(null, null);
            var result = creator.CreateExtentForm(extent, new FormFactoryConfiguration());
            Assert.That(result, Is.Not.Null);
            var tab = result.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ExtentForm.tab).Select(x=> x as IElement).FirstOrDefault();
            Assert.That(tab, Is.Not.Null);
            Assert.That(tab
                .getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field)
                .OfType<IElement>()
                .Count(x => x.getMetaClass().ToString().Contains("TextFieldData")), Is.EqualTo(2));
            var firstColumn = tab
                .getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field)
                .OfType<IElement>()
                .FirstOrDefault(x => x.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) == "zip");
            var secondColumn =
                tab
                    .getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field)
                    .OfType<IElement>()
                    .FirstOrDefault(x => x.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) == "location");

            Assert.That(firstColumn, Is.Not.Null);
            Assert.That(secondColumn, Is.Not.Null);

            Assert.That(firstColumn.getOrDefault<bool>(_DatenMeister._Forms._FieldData.isEnumeration), Is.False);
        }

        [Test]
        public void TestEnumerationAutoColumn()
        {
            var property1 = "zip";
            var property2 = "location";
            var property3 = "other";

            var extent = new MofUriExtent(new InMemoryProvider(), "dm:///");
            var factory = new MofFactory(extent);
            var mofObject = factory.create(null);
            mofObject.set(property1, "55130");
            mofObject.set(property2, "Mainz");

            var mofObject2 = factory.create(null);
            mofObject2.set(property1, "65474");
            mofObject2.set(property2, "Bischofsheim");

            var valueList = new List<object> {factory.create(null)};
            mofObject2.set(property3, valueList);

            extent.elements().add(mofObject);
            extent.elements().add(mofObject2);

            // Execute the stuff
            var creator = FormCreator.Create(null, null);
            var result = creator.CreateExtentForm(extent, new FormFactoryConfiguration());
            Assert.That(result, Is.Not.Null);

            var tab = result
                .getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ExtentForm.tab)
                .Select(x => x as IElement).FirstOrDefault();

            Assert.That(tab
                    .getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field)
                    .OfType<IElement>()
                    .Count(x => x.getMetaClass()?.ToString()?.Contains("TextFieldData") == true),
                Is.EqualTo(2));

            Assert.That(tab
                    .getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field)
                    .OfType<IElement>()
                    .Count(x => x.getMetaClass()?.ToString()?.Contains("SubElementFieldData") == true
                    || x.getMetaClass()?.ToString()?.Contains("ReferenceFieldData") == true),
                Is.GreaterThanOrEqualTo(1));


            var firstColumn = tab
                .getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field)
                .OfType<IElement>()
                .FirstOrDefault(x => x.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) == "zip");
            var secondColumn =
                tab
                    .getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field)
                    .OfType<IElement>()
                    .FirstOrDefault(x => x.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) == "location");
            var thirdColumn = tab
                .getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field)
                .OfType<IElement>()
                .FirstOrDefault(x => x.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) == "other");


            Assert.That(firstColumn, Is.Not.Null);
            Assert.That(secondColumn, Is.Not.Null);
            Assert.That(thirdColumn, Is.Not.Null);

            Assert.That(firstColumn.getOrDefault<bool>(_DatenMeister._Forms._FieldData.isEnumeration), Is.False);
            Assert.That(secondColumn.getOrDefault<bool>(_DatenMeister._Forms._FieldData.isEnumeration), Is.False);
        }
    }
}