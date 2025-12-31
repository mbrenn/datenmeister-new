using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Provider;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Provider.Excel.Helper;

namespace DatenMeister.Provider.Excel.ProviderLoader;

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
    public async Task<LoadedProviderInfo> LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags)
    {
        return await Task.Run(() =>
        {
            // Now load the stuff
            var provider = new InMemoryProvider();
            var extent = new MofUriExtent(provider, ScopeStorage);

            ImportExcelIntoExtent(extent, configuration);

            // Returns the provider
            return new LoadedProviderInfo(provider);
        });
    }

    public Task StoreProvider(IProvider extent, IElement configuration)
    {
        // Nothing to store, since the Excel Reference Provider is just a read-only thing
        return Task.CompletedTask;
    }

    public ProviderLoaderCapabilities ProviderLoaderCapabilities { get; } = new()
    {
        IsPersistant = true,
        AreChangesPersistant = false
    };

    /// <summary>
    /// Imports the excel into the extent
    /// </summary>
    /// <param name="extent"></param>
    /// <param name="loaderConfig">Element of ExcelLoaderConfig</param>
    public static void ImportExcelIntoExtent(IExtent extent, IElement loaderConfig)
    {
        loaderConfig = ObjectCopier.CopyForTemporary(loaderConfig) as IElement
                       ?? throw new InvalidOperationException("Element is not of type IElement");

        var factory = new MofFactory(extent);
        var fixColumnCount =
            loaderConfig.getOrDefault<bool>(_ExtentLoaderConfigs._ExcelReferenceLoaderConfig
                .fixColumnCount);
        var onlySetColumns =
            loaderConfig.getOrDefault<bool>(_ExtentLoaderConfigs._ExcelReferenceLoaderConfig
                .onlySetColumns);
        var fixRowCount =
            loaderConfig.getOrDefault<bool>(_ExtentLoaderConfigs._ExcelReferenceLoaderConfig
                .fixRowCount);
        var countRows =
            loaderConfig.getOrDefault<int>(_ExtentLoaderConfigs._ExcelReferenceLoaderConfig
                .countRows);
        var countColumns =
            loaderConfig.getOrDefault<int>(_ExtentLoaderConfigs._ExcelReferenceLoaderConfig
                .countColumns);

        var excelImporter = new ExcelImporter(loaderConfig);
        excelImporter.LoadExcel();

        var columnNames = excelImporter.GetOriginalColumnNames().ToList();
        if (!fixColumnCount) countRows = excelImporter.GuessRowCount();
        if (!fixRowCount) countColumns = excelImporter.GuessColumnCount();

        for (var r = 0; r < countRows; r++)
        {
            var item = factory.create(null);
            var contentInElement = false;
            for (var c = 0; c < countColumns; c++)
            {
                var columnName = columnNames[c];

                var cellContent = excelImporter.GetCellContent(r, c);
                if (!string.IsNullOrEmpty(cellContent))
                {
                    contentInElement = true;
                }

                var usedColumnName =
                    onlySetColumns
                        ? excelImporter.ColumnTranslator.TranslateHeaderOrNull(columnName)
                        : excelImporter.ColumnTranslator.TranslateHeader(columnName);
                if (usedColumnName != null)
                {
                    item.set(usedColumnName, excelImporter.GetCellContent(r, c));
                }
            }

            if (contentInElement)
            {
                extent.elements().add(item);
            }
        }
    }
}