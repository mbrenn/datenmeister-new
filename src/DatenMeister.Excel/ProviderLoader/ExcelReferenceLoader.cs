using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Excel.Helper;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Provider;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Excel.ProviderLoader
{
    /// <summary>
    /// Implements the loader which creates an InMemoryExtent out of an excel file
    /// If DatenMeister will be rebooted, the excel file will be loaded again
    /// </summary>
    public class ExcelReferenceLoader : IProviderLoader
    {
        public IWorkspaceLogic? WorkspaceLogic { get; set; }
        
        public IScopeStorage? ScopeStorage { get; set; }

        /// <summary>
        /// Of Type ExcelReferenceLoaderConfig
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="extentCreationFlags"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public LoadedProviderInfo LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags)
        {
            // Now load the stuff
            var provider = new InMemoryProvider();
            var extent = new MofUriExtent(provider);

            ImportExcelIntoExtent(extent, configuration);

            // Returns the provider
            return new LoadedProviderInfo(provider);
        }

        /// <summary>
        /// Imports the excel into the extent
        /// </summary>
        /// <param name="extent"></param>
        /// <param name="loaderConfig">Element of ExcelLoaderConfig</param>
        internal static void ImportExcelIntoExtent(MofExtent extent, IElement loaderConfig)
        {
            loaderConfig = ObjectCopier.CopyForTemporary(loaderConfig) as IElement
                ?? throw new InvalidOperationException("Element is not of type IElement");
            
            var factory = new MofFactory(extent);
            var fixColumnCount =
                loaderConfig.getOrDefault<bool>(_DatenMeister._ExtentLoaderConfigs._ExcelReferenceLoaderConfig
                    .fixColumnCount);
            var fixRowCount =
                loaderConfig.getOrDefault<bool>(_DatenMeister._ExtentLoaderConfigs._ExcelReferenceLoaderConfig
                    .fixRowCount);
            var countRows =
                loaderConfig.getOrDefault<int>(_DatenMeister._ExtentLoaderConfigs._ExcelReferenceLoaderConfig
                    .countRows);
            var countColumns =
                loaderConfig.getOrDefault<int>(_DatenMeister._ExtentLoaderConfigs._ExcelReferenceLoaderConfig
                    .countColumns);
            
            var excelImporter = new ExcelImporter(loaderConfig);
            excelImporter.LoadExcel();

            var columnNames = excelImporter.GetColumnNames();
            if (!fixColumnCount) countRows = excelImporter.GuessRowCount();
            if (!fixRowCount) countColumns = excelImporter.GuessColumnCount();

            for (var r = 0; r < countRows; r++)
            {
                var item = factory.create(null);
                var contentInElement = false;
                for (var c = 0; c < countColumns; c++)
                {
                    var columnName = columnNames[c];
                    if (columnName == null)
                    {
                        // Skip not set columns
                        continue;
                    }

                    var cellContent = excelImporter.GetCellContent(r, c);
                    if (!string.IsNullOrEmpty(cellContent))
                    {
                        contentInElement = true;
                    }
                    
                    item.set(columnName, excelImporter.GetCellContent(r, c));
                }

                if (contentInElement)
                {
                    extent.elements().add(item);
                }
            }
        }

        public void StoreProvider(IProvider extent, IElement configuration)
        {
            // Nothing to store, since the Excel Reference Provider is just a read-only thing
        }
    }
}