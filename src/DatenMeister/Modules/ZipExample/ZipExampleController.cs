using System;
using System.IO;
using System.Linq;
using DatenMeister.Core;
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
    public class ZipExampleController
    {
        /// <summary>
        /// Gets or sets the instance for local type support
        /// </summary>
        private readonly LocalTypeSupport _localTypeSupport;

        public ZipExampleController(LocalTypeSupport localTypeSupport)
        {
            _localTypeSupport = localTypeSupport;
        }

        public static void AddZipCodeExample(IExtentManager extentManager, string workspaceId)
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
                    Columns = new[] {nameof(ZipCode.id), nameof(ZipCode.zip), nameof(ZipCode.positionLong), nameof(ZipCode.positionLat),nameof(ZipCode.name)}.ToList(),
                    MetaclassUri = $"{WorkspaceNames.UriInternalTypes}?Apps::ZipCode::ZipCode"
                }
            };

            var loadedExtent = extentManager.LoadExtent(defaultConfiguration, false);
            loadedExtent.SetExtentType("DatenMeister.Example.ZipCodes");
        }

        /// <summary>
        /// Performs the initialization for the zipcode class
        /// </summary>
        public void Initialize()
        {
            _localTypeSupport.AddInternalTypes(
                "Apps::ZipCode",
                typeof(ZipCode)
            );
        }

        public class ZipCode
        {
            public int id { get; set; }
            public int zip { get; set; }
            public double positionLong { get; set; }
            public double positionLat { get; set; }
            public string name { get; set; }
        }
    }
}