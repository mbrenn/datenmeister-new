using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Excel.Integration;
using DatenMeister.Integration;
using NUnit.Framework;

namespace DatenMeister.Tests.Excel
{
    [TestFixture]
    public class ExcelTests
    {
        [Test]
        public void LoadExcel()
        {
            var dm = GiveMe.DatenMeister();
            var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var excelExtent = dm.LoadExcel("d:///excel", Path.Combine (currentDirectory, "Excel/Quadratzahlen.xlsx"));

            Console.WriteLine(excelExtent.ToString());
            foreach (var sheet in excelExtent.elements())
            {
                var sheetAsElement = (IElement)sheet;
                var allProperties = ((IEnumerable<object>)sheetAsElement.get("items")).First() as IObjectAllProperties;
                Assert.That(sheetAsElement.get("name").ToString(), Is.EqualTo("Tabelle1"));
                Assert.That(allProperties, Is.Not.Null);

                Assert.That(allProperties.getPropertiesBeingSet().Count(), Is.EqualTo(2));
                Assert.That(allProperties.getPropertiesBeingSet().ElementAt(0), Is.EqualTo("Wert"));

                foreach (var item in (IEnumerable<object>)sheetAsElement.get("items"))
                {
                    var itemAsElement = (IElement)item;

                    var value1 = Convert.ToInt32(itemAsElement.get("Wert"));
                    var value2 = Convert.ToInt32(itemAsElement.get("Quadratzahl"));
                    Assert.That(value1 * value1 - value2, Is.EqualTo(0));
                }

                Assert.That(((IEnumerable<object>) sheetAsElement.get("items")).Count(), Is.GreaterThan(10));
            }

        }
    }
}