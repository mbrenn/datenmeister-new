using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using DatenMeister.Models.PostModels;
using DatenMeister.Provider.CSV.Runtime.Storage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.Web.Api
{
    [Route("api/datenmeister/example")]
    public class ExampleController : Controller
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IExtentStorageLoader _loader;

        private static readonly Random Random = new Random();

        public ExampleController(IWorkspaceLogic workspaceLogic,IExtentStorageLoader loader)
        {
            _workspaceLogic = workspaceLogic;
            _loader = loader;
        }

        [Route("addzipcodes")]
        public void AddZipExample([FromBody] WorkspaceReferenceModel workspace)
        {
            // Finds the file and copies the file to the given location
            var appBase = AppContext.BaseDirectory;
            string filename;
            var tries = 0;
            int randomNumber;
            do // while File.Exists
            {
                randomNumber = Random.Next(int.MaxValue);
                filename = Path.Combine(appBase, "App_Data/Database", $"plz_{randomNumber}.csv");
                tries++;
                if (tries == 10000)
                {
                    throw new InvalidOperationException("Did not find a unique name for zip extent");
                }
            } while (System.IO.File.Exists(filename));

            var originalFilename = Path.Combine(
                appBase,
                "App_Data/Example",
                "plz.csv");

            System.IO.File.Copy(originalFilename, filename);

            var defaultConfiguration = new CSVStorageConfiguration
            {
                ExtentUri = $"datenmeister:///zipcodes/{randomNumber}",
                Path = filename,
                Workspace = workspace.ws,
                Settings =
                {
                    HasHeader = false,
                    Separator = '\t',
                    Encoding = "UTF-8",
                    Columns = new [] { "Id", "Zip", "PositionLong", "PositionLat", "CityName" }.ToList(),
                    // Columns = new object[] { idProperty, zipProperty, positionLongProperty, positionLatProperty, citynameProperty }.ToList(),
                    MetaclassUri = "dm:///types#DatenMeister.Apps.ZipCode.Model.ZipCode"
                }
            };

            _loader.LoadExtent(defaultConfiguration, false);

            Debug.WriteLine("Zip codes loaded");
        }
    }
}