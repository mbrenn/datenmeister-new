using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Modules.ZipCodeExample.Model;

namespace DatenMeister.Modules.ZipCodeExample;

/// <summary>
/// Supports some methods for the example
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class ZipCodeExampleManager
{
    private static readonly ILogger Logger = new ClassLogger(typeof(ZipCodeExampleManager));
    private readonly ExtentManager _extentManager;

    private readonly IWorkspaceLogic _workspaceLogic;
    private readonly ZipCodeModel _zipCodeModel;

    /// <summary>
    ///     Initializes a new instance of the zip code example manager
    /// </summary>
    /// <param name="workspaceLogic">Workspace logic to be used</param>
    /// <param name="scopeStorage">Scope storage to be used</param>
    public ZipCodeExampleManager(
        IWorkspaceLogic workspaceLogic,
        IScopeStorage scopeStorage)
        : this(workspaceLogic, new ExtentManager(workspaceLogic, scopeStorage), scopeStorage)
    {
    }

    public ZipCodeExampleManager(
        IWorkspaceLogic workspaceLogic,
        ExtentManager extentManager,
        IScopeStorage scopeStorage)
        : this(workspaceLogic, extentManager, scopeStorage.Get<ZipCodeModel>())
    {
    }

    private ZipCodeExampleManager(
        IWorkspaceLogic workspaceLogic,
        ExtentManager extentManager,
        ZipCodeModel zipCodeModel)

    {
        _workspaceLogic = workspaceLogic;
        _extentManager = extentManager;
        _zipCodeModel = zipCodeModel;
    }

    /// <summary>
    /// Adds a zipcode example
    /// </summary>
    /// <param name="workspace">Workspace to which the zipcode example shall be added</param>
    /// <param name="exampleFilePath">Defines the path to the example file</param>
    public async Task<IUriExtent> AddZipCodeExample(IWorkspace workspace, string? exampleFilePath = null)
        => await AddZipCodeExample(workspace.id, exampleFilePath);

    public async Task<IUriExtent> AddZipCodeExample(string workspaceId, string? exampleFilePath = null)
    {
        var random = new Random();

        // Finds the file and copies the file to the given location
        var appBase = AppContext.BaseDirectory;

        // Creates directory, if it does not exist
        var directory = Path.Combine(appBase, "App_Data/Database");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string filename;
        var tries = 0;
        int randomNumber;
        do // while File.Exists
        {
            randomNumber = random.Next(int.MaxValue);
            filename = Path.Combine(appBase, "App_Data/Database", $"plz_{randomNumber}.csv");
            tries++;
            if (tries == 10000)
            {
                throw new InvalidOperationException("Did not find a unique name for zip extent");
            }
        } while (File.Exists(filename));

        var extentName = $"dm:///zipcodes/{randomNumber}";

        return await AddZipCodeExample(workspaceId, extentName, exampleFilePath, filename);
    }

    public string GetDefaultPathForExampleZipCodes()
    {
        var appBase = AppContext.BaseDirectory;
        return Path.Combine(appBase, "Examples", "plz.csv");
    }

    public async Task<IUriExtent> AddZipCodeExample(
        string workspaceId,
        string extentName,
        string? sourceFilename,
        string targetFilename)
    {
        // Copies the example file to a new extent
        var originalFilename = sourceFilename ?? GetDefaultPathForExampleZipCodes();
        if (!File.Exists(originalFilename))
        {
            throw new InvalidOperationException(
                $"The example files are not stored in folder: \r\n{originalFilename}");
        }

        var directoryPath = Path.GetDirectoryName(targetFilename);
        if (!Directory.Exists(directoryPath) && directoryPath != null)
        {
            Directory.CreateDirectory(directoryPath);
        }

        File.Delete(targetFilename);
        File.Copy(originalFilename, targetFilename);

        // Creates the configuration
        var csvSettings =
            InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__CsvSettings);
        csvSettings.set(_ExtentLoaderConfigs._CsvSettings.hasHeader, false);
        csvSettings.set(_ExtentLoaderConfigs._CsvSettings.separator, '\t');
        csvSettings.set(_ExtentLoaderConfigs._CsvSettings.encoding, "UTF-8");
        csvSettings.set(_ExtentLoaderConfigs._CsvSettings.metaclassUri, _zipCodeModel.ZipCode);
        csvSettings.set(_ExtentLoaderConfigs._CsvSettings.columns, new[]
        {
            nameof(ZipCode.id),
            nameof(ZipCode.zip),
            nameof(ZipCode.positionLong),
            nameof(ZipCode.positionLat),
            nameof(ZipCode.name)
        }.ToList());

        var defaultConfiguration =
            InMemoryObject.CreateEmpty(_ExtentLoaderConfigs.TheOne.__CsvExtentLoaderConfig);
        defaultConfiguration.set(
            _ExtentLoaderConfigs._CsvExtentLoaderConfig.extentUri, extentName);
        defaultConfiguration.set(
            _ExtentLoaderConfigs._CsvExtentLoaderConfig.filePath,
            targetFilename);
        defaultConfiguration.set(
            _ExtentLoaderConfigs._CsvExtentLoaderConfig.workspaceId,
            workspaceId);
        defaultConfiguration.set(
            _ExtentLoaderConfigs._CsvExtentLoaderConfig.settings,
            csvSettings);

        var loadedExtent = await _extentManager.LoadExtent(defaultConfiguration)
                           ?? throw new InvalidOperationException("defaultConfiguration could not be loaded");
        if (loadedExtent.LoadingState == ExtentLoadingState.Failed || loadedExtent.Extent == null)
        {
            throw new InvalidOperationException("Loading of zip extent failed");
        }

        loadedExtent.Extent.GetConfiguration().ExtentType = ZipCodePlugin.ZipCodeExtentType;

        if (_workspaceLogic.GetTypesWorkspace().FindObject(
                "dm:///_internal/types/internal?" + ZipCodeModel.PackagePath) is IElement zipCodeTypePackage)
        {
            loadedExtent.Extent.GetConfiguration().SetDefaultTypes(new[] {zipCodeTypePackage});
        }
        else
        {
            Logger.Warn("dm:///_internal/types/internal?" + ZipCodeModel.PackagePath + "not found");
        }

        return loadedExtent.Extent;
    }
}