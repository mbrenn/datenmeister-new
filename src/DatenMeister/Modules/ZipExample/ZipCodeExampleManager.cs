using System;
using System.IO;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Models.Example.ZipCode;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.ZipExample
{
    /// <summary>
    /// Supports some methods for the example
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ZipCodeExampleManager
    {
        private static readonly ILogger Logger = new ClassLogger(typeof(ZipCodeExampleManager));
        
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly ExtentManager _extentManager;
        private readonly ZipCodeModel _zipCodeModel;

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
        public IUriExtent AddZipCodeExample(Workspace workspace, string? exampleFilePath = null)
            => AddZipCodeExample(workspace.id, exampleFilePath);

        public IUriExtent AddZipCodeExample(string workspaceId, string? exampleFilePath = null)
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

            // Copies the example file to a new extent
            var originalFilename = exampleFilePath ?? Path.Combine(appBase, "Examples", "plz.csv");
            if (!File.Exists(originalFilename))
            {
                throw new InvalidOperationException(
                    $"The example files are not stored in folder: \r\n{originalFilename}");
            }

            File.Copy(originalFilename, filename);

            // Creates the configuration
            

            var defaultConfiguration =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__CsvExtentLoaderConfig);
            defaultConfiguration.set(
                _DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.extentUri,
                "dm:///zipcodes/{randomNumber}");
            defaultConfiguration.set(
                _DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath,
                filename);
            defaultConfiguration.set(
                _DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.workspaceId,
                workspaceId);
            
            /*
            var defaultConfiguration2 = new CsvExtentLoaderConfig($"dm:///zipcodes/{randomNumber}")
            {
                filePath = filename,
                workspaceId = workspaceId,
                settings =
                {
                    HasHeader = false,
                    Separator = '\t',
                    Encoding = "UTF-8",
                    Columns = new[]
                    {
                        nameof(ZipCode.id),
                        nameof(ZipCode.zip),
                        nameof(ZipCode.positionLong),
                        nameof(ZipCode.positionLat),
                        nameof(ZipCode.name)
                    }.ToList(),
                    MetaclassUri = _zipCodeModel.ZipCodeUri ?? string.Empty
                }
            };*/
            
            throw new InvalidOperationException();

            var loadedExtent = _extentManager.LoadExtent(defaultConfiguration)
                               ?? throw new InvalidOperationException("defaultConfiguration could not be loaded");
            if (loadedExtent.LoadingState == ExtentLoadingState.Failed || loadedExtent.Extent == null)
            {
                throw new InvalidOperationException("Loading of zip extent failed");
            }
            
            loadedExtent.Extent.GetConfiguration().ExtentType = ZipCodePlugin.ExtentType;

            if (_workspaceLogic.GetTypesWorkspace().FindElementByUri(
                "dm:///_internal/types/internal?" + ZipCodeModel.PackagePath) is IElement zipCodeTypePackage)
            {
                loadedExtent.Extent.GetConfiguration().SetDefaultTypePackages(new[] {zipCodeTypePackage});
            }
            else
            {
                Logger.Warn("dm:///_internal/types/internal?" + ZipCodeModel.PackagePath + "not found");
            }

            return loadedExtent.Extent;
        }
    }
}