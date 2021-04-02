using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Excel.Helper;
using DatenMeister.Integration;
using DatenMeister.Provider;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Functions.Queries;
using static DatenMeister.Core.Models._DatenMeister._ExtentLoaderConfigs;

namespace DatenMeister.Excel.ProviderLoader
{
    public class ExcelHierarchicalLoader : IProviderLoader
    {
        public IWorkspaceLogic? WorkspaceLogic { get; set; }
        public IScopeStorage? ScopeStorage { get; set; }

        private class DefinitionColumn
        {
            public string Name { get; set; }
            public IElement MetaClass { get; set; }
            public string Property { get; set; }
            
        }

        public LoadedProviderInfo LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags)
        {
            configuration = ObjectCopier.CopyForTemporary(configuration) as IElement
                            ?? throw new InvalidOperationException("Element is not of type IElement");

            if (WorkspaceLogic == null) throw new InvalidOperationException("WorkspaceLogic == null");
            
            var filePath =
                configuration.getOrDefault<string>(_ExcelHierarchicalLoaderConfig.filePath);

            if (!File.Exists(filePath))
            {
                throw new IOException($"File not found: {filePath}");
            }

            var fixColumnCount =
                configuration.getOrDefault<bool>(_ExcelHierarchicalLoaderConfig.fixColumnCount);
            var fixRowCount =
                configuration.getOrDefault<bool>(_ExcelHierarchicalLoaderConfig.fixRowCount);
            var countRows =
                configuration.getOrDefault<int>(_ExcelHierarchicalLoaderConfig.countRows);
            var countColumns =
                configuration.getOrDefault<int>(_ExcelHierarchicalLoaderConfig.countColumns);
            var skipElementsForLastLevel =
                configuration.getOrDefault<bool>(_ExcelHierarchicalLoaderConfig.skipElementsForLastLevel);

            var excelImporter = new ExcelImporter(configuration);
            excelImporter.LoadExcel();

            var columnNames = excelImporter.GetColumnNames().ToList();
            if (!fixColumnCount) countRows = excelImporter.GuessRowCount();
            if (!fixRowCount) countColumns = excelImporter.GuessColumnCount();

            var provider = new InMemoryProvider();
            var tempExtent = new MofUriExtent(provider);
            var factory = new MofFactory(tempExtent);

            /* Gets the definitions */
            var definitions = configuration.getOrDefault<IReflectiveCollection>(
                    _ExcelHierarchicalLoaderConfig.hierarchicalColumns)
                .OfType<IElement>()
                .ToList();

            var definitionColumns = definitions
                .Select(
                    definition => definition.getOrDefault<string>(_ExcelHierarchicalColumnDefinition.name))
                .ToList();

            var definitionsCache = new List<DefinitionColumn>();

            foreach (var definition in definitions)
            {
                var name = definition.getOrDefault<string>(_ExcelHierarchicalColumnDefinition.name);
                var metaClass = definition.getOrDefault<IElement>(_ExcelHierarchicalColumnDefinition.metaClass);
                var property = definition.getOrDefault<string>(_ExcelHierarchicalColumnDefinition.property);
                definitionsCache.Add(new DefinitionColumn
                {
                    Name = name,
                    MetaClass = metaClass,
                    Property = property
                });
            }
            
            for (var r = 0; r < countRows; r++)
            {
                var skipItem = false;
                var current = tempExtent.elements();
                IElement lastCreated = null;
                var idBuilder = new StringBuilder();
                var firstDefinition = true;

                for (var d = 0; d < definitions.Count; d++)
                {
                    var name = definitionsCache[d].Name;
                    var metaClass = definitionsCache[d].MetaClass;
                    var property = definitionsCache[d].Property;

                    var indexOfName = columnNames.IndexOf(name);
                    if (indexOfName == -1)
                    {
                        throw new InvalidOperationException($"Column name {name} is not found");
                    }

                    var cellContent = excelImporter.GetCellContent(r, indexOfName);

                    // Creates the idBuilder
                    if (!firstDefinition) idBuilder.Append(".");
                    idBuilder.Append(cellContent);
                    firstDefinition = false;

                    // Checks content
                    if (string.IsNullOrEmpty(cellContent))
                    {
                        // Skips the item, if the navigation cannot be completed
                        skipItem = true;
                        break;
                    }

                    var newCurrent =
                        current.WhenPropertyHasValue("name", cellContent).OfType<IElement>()
                            .FirstOrDefault();
                    if (newCurrent == null)
                    {
                        newCurrent = factory.create(metaClass);
                        newCurrent.set("name", cellContent);
                        current.add(newCurrent);

                        if (newCurrent is ICanSetId canSetId)
                        {
                            canSetId.Id = idBuilder.ToString();
                        }
                    }

                    lastCreated = newCurrent;
                    current = newCurrent.get<IReflectiveSequence>(property);
                }

                if (skipItem || lastCreated == null)
                {
                    continue;
                }
    
                var item = skipElementsForLastLevel ? lastCreated : factory.create(null);
                for (var c = 0; c < countColumns; c++)
                {
                    var columnName = columnNames[c];
                    if (columnName == null || definitionColumns.Contains(columnName)) continue;
                    
                    item.set(columnName, excelImporter.GetCellContent(r, c));
                }

                if (!skipElementsForLastLevel)
                {
                    current.add(item);
                }
                else
                {
                }
            }

            return new LoadedProviderInfo(provider);
        }

        public void StoreProvider(IProvider extent, IElement configuration)
        {
            // Storing is not supported
        }
    }
}