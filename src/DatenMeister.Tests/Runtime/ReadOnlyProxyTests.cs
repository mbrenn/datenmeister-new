﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;
using DatenMeister.CSV.Runtime.Storage;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Proxies.ReadOnly;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime
{
    [TestFixture]
    public class ReadOnlyProxyTests
    {
        [Test]
        public void TestChainFromExtentToElement()
        {
            var csvExtent = CreateSimpleCsvExtent();

            var readOnly = new ReadOnlyUriExtent(csvExtent);
            Assert.Throws<ReadOnlyAccessException>(() => readOnly.elements().clear());

            var element = readOnly.elements().ElementAt(0) as IElement;
            Assert.That(element, Is.Not.Null);
            Assert.Throws<ReadOnlyAccessException>(() =>
                element.unset("Test"));

            Assert.That(readOnly.elements().size(), Is.GreaterThan(0));
            Assert.That(readOnly.elements().size(), Is.EqualTo(csvExtent.elements().size()));

            var property1  = ((IObjectAllProperties) element).getPropertiesBeingSet().ElementAt(0);
            Assert.That(element.get(property1), Is.Not.Null);

            Assert.Throws<ReadOnlyAccessException>(() => ((IElementSetMetaClass) element).setMetaClass(null));
        }   

        /// <summary>
        /// Creates a simple extent containing three elements with four properties
        /// </summary>
        /// <returns>The created uri extent</returns>
        private static IUriExtent CreateSimpleCsvExtent()
        {
            var csvFile = "eins 1 one\r\nzwei 2 two\r\ndrei 3 three\r\nvier 4 four\r\n";
            File.WriteAllText("data.txt", csvFile);

            var mapper = new ManualExtentStorageToConfigurationMap();
            mapper.AddMapping(typeof (CSVStorageConfiguration), () => new CSVStorage());

            var logic = new ExtentStorageLogic(mapper);
            var configuration = new CSVStorageConfiguration()
            {
                Path = "data.txt",
                ExtentUri = "dm:///local/",
                Settings =
                {
                    HasHeader = false,
                    Separator = ' '
                }
            };

            var csvExtent = logic.LoadExtent(configuration);
            return csvExtent;
        }
    }
}