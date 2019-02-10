using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Excel.Helper;
using DatenMeister.Provider;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Excel.ProviderLoader
{
    /// <summary>
    /// Implements the loader which creates an InMemoryExtent out of an excel file
    /// If DatenMeister will be rebooted, the excel file will be loaded again
    /// </summary>
    [ConfiguredBy(typeof(ExcelReferenceSettings))]
    public class ExcelReferenceLoader : IProviderLoader
    {
        public LoadedProviderInfo LoadProvider(ExtentLoaderConfig configuration, bool createAlsoEmpty)
        {
            if (!(configuration is ExcelReferenceSettings settings))
            {
                throw new InvalidOperationException("Given configuration is not of type ExcelReferenceSettings");
            }

            // Now load the stuff
            var provider = new InMemoryProvider();
            var extent = new MofUriExtent(provider);

            ImportExcelIntoExtent(extent, settings);

            // Returns the provider
            return new LoadedProviderInfo(provider);
        }

        /// <summary>
        /// Imports the excel into the extent
        /// </summary>
        /// <param name="extent"></param>
        /// <param name="settings"></param>
        internal static void ImportExcelIntoExtent(MofExtent extent, ExcelSettings settings)
        {
            var factory = new MofFactory(extent);

            var excelImporter = new ExcelImporter(settings);
            excelImporter.LoadExcel();

            var columnNames = excelImporter.GetColumnNames();
            if (!settings.fixColumnCount) excelImporter.GuessRowCount();
            if (!settings.fixRowCount) excelImporter.GuessColumnCount();

            for (var r = 0; r < settings.countRows; r++)
            {
                var item = factory.create(null);
                for (var c = 0; c < settings.countColumns; c++)
                {
                    var columnName = columnNames[c];
                    if (columnName == null)
                    {
                        // Skip not set columns
                        continue;
                    }

                    item.set(columnName, excelImporter.GetCellContent(r, c));
                }

                extent.elements().add(item);
            }
        }

        public void StoreProvider(IProvider extent, ExtentLoaderConfig configuration)
        {
            // Nothing to store, since the Excel Reference Provider is just a read-only thing
        }
    }
}