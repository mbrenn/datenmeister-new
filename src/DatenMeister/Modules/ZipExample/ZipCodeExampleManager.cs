using System;
using System.IO;
using System.Linq;
using System.Net;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Example.ZipCode;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
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
        private readonly IExtentManager _extentManager;
        private readonly ZipCodeModel _zipCodeModel;

        public ZipCodeExampleManager(
            IWorkspaceLogic workspaceLogic,
            IExtentManager extentManager,
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
        public IUriExtent AddZipCodeExample(Workspace workspace)
            => AddZipCodeExample(workspace.id);

        public IUriExtent AddZipCodeExample(string workspaceId)
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
            var originalFilename = Path.Combine(appBase, "Examples", "plz.csv");
            if (!File.Exists(originalFilename))
            {
                throw new InvalidOperationException(
                    $"The example files are not stored in folder: \r\n{originalFilename}");
            }

            File.Copy(originalFilename, filename);

            // Creates the configuration
            var defaultConfiguration = new CsvExtentLoaderConfig($"datenmeister:///zipcodes/{randomNumber}")
            {
                filePath = filename,
                workspaceId = workspaceId,
                Settings =
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
            };

            var loadedExtent = _extentManager.LoadExtent(defaultConfiguration)
                               ?? throw new InvalidOperationException("defaultConfiguration could not be loaded");
            
            loadedExtent.GetConfiguration().ExtentType = ZipCodePlugin.ExtentType;

            if (_workspaceLogic.GetTypesWorkspace().FindElementByUri(
                "datenmeister:///_internal/types/internal?" + ZipCodeModel.PackagePath) is IElement zipCodeTypePackage)
            {
                loadedExtent.GetConfiguration().SetDefaultTypePackages(new[] {zipCodeTypePackage});
            }
            else
            {
                Logger.Warn("datenmeister:///_internal/types/internal?" + ZipCodeModel.PackagePath + "not found");
            }

            return loadedExtent;
        }
    }
}