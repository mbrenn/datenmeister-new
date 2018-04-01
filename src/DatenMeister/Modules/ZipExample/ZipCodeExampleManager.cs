using System;
using System.IO;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Provider.CSV.Runtime;
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
        /// <summary>
        /// Gets or sets the instance for local type support
        /// </summary>
        private readonly LocalTypeSupport _localTypeSupport;

        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IExtentManager _extentManager;

        public ZipCodeExampleManager(
            LocalTypeSupport localTypeSupport, 
            IWorkspaceLogic workspaceLogic, 
            IExtentManager extentManager)
        {
            _localTypeSupport = localTypeSupport;
            _workspaceLogic = workspaceLogic;
            _extentManager = extentManager;
        }

        /// <summary>
        /// Adds a zipcode example 
        /// </summary>
        /// <param name="extentManager">Extent manager to be used</param>
        /// <param name="workspace">Workspace to which the zipcode example shall be added</param>
        public IUriExtent AddZipCodeExample(Workspace workspace)
        {
            return AddZipCodeExample(workspace.id);
        }

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

            var originalFilename = Path.Combine(appBase, "Examples", "plz.csv");

            File.Copy(originalFilename, filename);

            var defaultConfiguration = new CSVExtentLoaderConfig
            {
                ExtentUri = $"datenmeister:///zipcodes/{randomNumber}",
                Path = filename,
                Workspace = workspaceId,
                Settings =
                {
                    HasHeader = false,
                    Separator = '\t',
                    Encoding = "UTF-8",
                    Columns = new[]
                    {
                        nameof(ZipCodeModel.id),
                        nameof(ZipCodeModel.zip),
                        nameof(ZipCodeModel.positionLong),
                        nameof(ZipCodeModel.positionLat),
                        nameof(ZipCodeModel.name)
                    }.ToList(),
                    MetaclassUri = $"{WorkspaceNames.UriInternalTypes}?Apps::ZipCodeModel::ZipCodeModel"
                }
            };

            var loadedExtent = _extentManager.LoadExtent(defaultConfiguration, false);
            loadedExtent.SetExtentType("DatenMeister.Example.ZipCodes");

            var zipCodeTypePackage =
                _workspaceLogic.GetTypesWorkspace().FindElementByUri(
                    "datenmeister:///_internal/types/internal?Apps::ZipCodeModel") as IElement;
            loadedExtent.SetDefaultTypePackage(zipCodeTypePackage);

            return loadedExtent;
        }

        /// <summary>
        /// Performs the initialization for the zipcode class
        /// </summary>
        public void Initialize()
        {
            _localTypeSupport.AddInternalTypes(
                "Apps::ZipCodeModel",
                typeof(ZipCodeModel)
            );
        }
    }
}