using System;
using System.IO;
using System.Linq;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Provider.XMI;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.ZipExample
{
    /// <summary>
    /// Supports some methods for the example
    /// </summary>
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
                    Columns = new[] {"Id", "Zip", "PositionLong", "PositionLat", "CityName"}.ToList(),
                    MetaclassUri = $"{WorkspaceNames.UriInternalTypes}?Apps::ZipCode::ZipCode"
                }
            };

            extentManager.LoadExtent(defaultConfiguration, false);
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
            public int Id { get; set; }
            public int Zip { get; set; }
            public double PositionLong { get; set; }
            public double PositionLat { get; set; }
            public string CityName { get; set; }
        }
    }
}