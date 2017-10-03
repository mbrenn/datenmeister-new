﻿using System.IO;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime
{
    [TestFixture]
    public class ExtentStorageTests
    {
        [Test]
        public void TestExtentStorageLogic()
        {
            var csvFile = "eins 1 one\r\nzwei 2 two\r\ndrei 3 three\r\nvier 4 four\r\n";
            File.WriteAllText("data.txt", csvFile);
            
            var mapper = new ManualConfigurationToExtentStorageMapper();
            mapper.AddMapping(typeof (CSVExtentLoaderConfig), scope => new CSVExtentLoader(null));
            var dataLayers = WorkspaceLogic.InitDefault();

            var data = new ExtentStorageData();
            var logic = new ExtentManager(data, mapper, null, new WorkspaceLogic(dataLayers));
            var configuration = new CSVExtentLoaderConfig
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
            Assert.That(csvExtent, Is.Not.Null);
            
            Assert.That(csvExtent.elements().Count(), Is.EqualTo(4));
            logic.StoreExtent(csvExtent);

            // Changes content, store it and check, if stored
            ((IObject) csvExtent.elements().ElementAt(0)).set(configuration.Settings.Columns[0], "eens");
            logic.StoreAll();

            var read = File.ReadAllText("data.txt");
            Assert.That(read.Contains("eens"), Is.True);
        }
    }
}