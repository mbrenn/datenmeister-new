using System;
using System.IO;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Excel.Helper;
using DatenMeister.Integration;
using DatenMeister.Provider;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using static DatenMeister.Models._DatenMeister._ExtentLoaderConfigs;

namespace DatenMeister.Excel.ProviderLoader
{
    public class ExcelHierarchicalLoader : IProviderLoader
    {
        public IWorkspaceLogic? WorkspaceLogic { get; set; }
        public IScopeStorage? ScopeStorage { get; set; }

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

            var excelImporter = new ExcelImporter(configuration);
            excelImporter.LoadExcel();

            var columnNames = excelImporter.GetColumnNames();
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
            
            for (var r = 0; r < countRows; r++)
            {
                var skipItem = false;
                var current = tempExtent.elements();

                foreach (var definition in definitions)
                {
                    var name = definition.getOrDefault<string>(_ExcelHierarchicalColumnDefinition.name);
                    var metaClass = definition.getOrDefault<IElement>(_ExcelHierarchicalColumnDefinition.metaClass);
                    var property = definition.getOrDefault<string>(_ExcelHierarchicalColumnDefinition.property);
                    var indexOfName = columnNames.IndexOf(name);
                    if (indexOfName == -1)
                    {
                        throw new InvalidOperationException($"Column name {name} is not found");
                    }

                    var cellContent = excelImporter.GetCellContent(r, indexOfName);
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
                    }

                    current = newCurrent.get<IReflectiveSequence>(property);
                }

                if (skipItem)
                {
                    continue;
                }

                var item = factory.create(null);
                for (var c = 0; c < countColumns; c++)
                {
                    var columnName = columnNames[c];
                    if (columnName == null) continue;
                    
                    item.set(columnName, excelImporter.GetCellContent(r, c));
                }

                current.add(item);
            }

            return new LoadedProviderInfo(provider);
        }

        public void StoreProvider(IProvider extent, IElement configuration)
        {
            // Storing is not supported
        }
    }
}