using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Runtime.ExtentStorage;
using NUnit.Framework;
using static DatenMeister.Core.Models._DatenMeister._ExtentLoaderConfigs;

namespace DatenMeister.Tests.Excel
{
    [TestFixture]
    public class ExcelHierarchicalTests
    {
        [Test]
        public void TestHierarchicalExcelLoad()
        {
            var dm = DatenMeisterTests.GetDatenMeisterScope();
            var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = Path.Combine(currentDirectory, "Excel/Hierarchical Test.xlsx");
            
            var localType = new LocalTypeSupport(dm.WorkspaceLogic, dm.ScopeStorage);
            var localTypes = localType.InternalTypes;
            var localTypeFactory = new MofFactory(localTypes);
            var type1 = localTypeFactory.create(_UML.TheOne.StructuredClassifiers.__Class);
            var type2 = localTypeFactory.create(_UML.TheOne.StructuredClassifiers.__Class);
            var type3 = localTypeFactory.create(_UML.TheOne.StructuredClassifiers.__Class);
            localTypes.elements().add(type1);
            localTypes.elements().add(type2);
            localTypes.elements().add(type3);
            
            var loaderConfig = InMemoryObject.CreateEmpty(
                _DatenMeister.TheOne.ExtentLoaderConfigs.__ExcelHierarchicalLoaderConfig);

            loaderConfig.set(_ExcelHierarchicalLoaderConfig.extentUri, "dm:///test");
            loaderConfig.set(_ExcelHierarchicalLoaderConfig.filePath, filePath);
            loaderConfig.set(_ExcelHierarchicalLoaderConfig.hasHeader, true);
            loaderConfig.set(_ExcelHierarchicalLoaderConfig.offsetRow, 1);
            loaderConfig.set(_ExcelHierarchicalLoaderConfig.offsetColumn, 0);
            loaderConfig.set(_ExcelHierarchicalLoaderConfig.workspaceId, "Data");

            var definition1 = InMemoryObject.CreateEmpty(
                    _DatenMeister.TheOne.ExtentLoaderConfigs.__ExcelHierarchicalColumnDefinition)
                .SetProperties(new Dictionary<string, object>
                    {
                        [_ExcelHierarchicalColumnDefinition.name] = "Liga",
                        [_ExcelHierarchicalColumnDefinition.metaClass] = type1,
                        [_ExcelHierarchicalColumnDefinition.property] = "team"
                    }
                );
            var definition2 = InMemoryObject.CreateEmpty(
                    _DatenMeister.TheOne.ExtentLoaderConfigs.__ExcelHierarchicalColumnDefinition)
                .SetProperties(new Dictionary<string, object>
                    {
                        [_ExcelHierarchicalColumnDefinition.name] = "Team",
                        [_ExcelHierarchicalColumnDefinition.metaClass] = type2,
                        [_ExcelHierarchicalColumnDefinition.property] = "player"
                    }
                );

            loaderConfig.set(_ExcelHierarchicalLoaderConfig.hierarchicalColumns,
                new[] {definition1, definition2});
            
            var extentManager = new ExtentManager(dm.WorkspaceLogic, dm.ScopeStorage);
            var inMemoryExtent = extentManager.LoadExtent(loaderConfig);

            Assert.That(inMemoryExtent, Is.Not.Null);
            Assert.That(inMemoryExtent.Extent, Is.Not.Null);

            Assert.That(inMemoryExtent.Extent.elements().Count(), Is.EqualTo(2));
            Assert.That(inMemoryExtent.Extent.elements().WhenPropertyHasValue("name", "Erste Liga").Any(), Is.True);
            Assert.That(inMemoryExtent.Extent.elements().WhenPropertyHasValue("name", "ZweiteLiga").Any(), Is.True);
            Assert.That(inMemoryExtent.Extent.elements().WhenPropertyHasValue("name", "Dritte Liga").Any(), Is.False);

            var firstLiga = inMemoryExtent.Extent.elements().WhenPropertyHasValue("name", "Erste Liga")
                .OfType<IElement>()
                .FirstOrDefault();
            Assert.That(firstLiga, Is.Not.Null);

            var teams = firstLiga.getOrDefault<IReflectiveSequence>("team")
                .OfType<IElement>()
                .ToList();
            
            Assert.That(teams.Count, Is.EqualTo(3));
            
            var frankfurt = new TemporaryReflectiveCollection(teams).WhenPropertyHasValue("name", "Frankfurt")
                .OfType<IElement>()
                .FirstOrDefault();
            
            Assert.That(frankfurt, Is.Not.Null);
            
            var players = frankfurt.getOrDefault<IReflectiveSequence>("player")
                .OfType<IElement>()
                .ToList();
            
            Assert.That(players, Is.Not.Null);
            Assert.That(players.Count, Is.EqualTo(6));
            
            var maier  = new TemporaryReflectiveCollection(players).WhenPropertyHasValue("Spieler", "Maier")
                .OfType<IElement>()
                .FirstOrDefault();

            Assert.That(maier, Is.Not.Null);
            
            var sebbel  = new TemporaryReflectiveCollection(players).WhenPropertyHasValue("Spieler", "Sebbel")
                .OfType<IElement>()
                .FirstOrDefault();

            Assert.That(sebbel, Is.Null);
        }
    }
}