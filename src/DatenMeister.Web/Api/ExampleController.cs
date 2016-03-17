using System;
using System.Diagnostics;
using System.IO;
using System.Web.Http;
using DatenMeister.CSV.Runtime.Storage;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Web.Models.PostModels;

namespace DatenMeister.Web.Api
{
    [RoutePrefix("api/datenmeister/example")]
    public class ExampleController : ApiController
    {
        private readonly IExtentStorageLoader _loader;

        private static readonly  Random Random = new Random();

        public ExampleController(IExtentStorageLoader loader)
        {
            _loader = loader;
        }

        [Route("addzipcodes")]
        public void AddZipExample([FromBody] WorkspaceReferenceModel workspace)
        {
            // Finds the file and copies the file to the given location
            var appBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string filename;
            var tries = 0;
            int randomNumber;
            do
            {
                randomNumber = Random.Next(1000000);
                filename = Path.Combine(appBase, "Data", $"plz_{randomNumber}.csv");
                tries++;
                if (tries == 10000)
                {
                    throw new InvalidOperationException("Did not find a unique name for zip extent");
                }
            } while (File.Exists(filename));


            var originalFilename = Path.Combine(
                appBase,
                "App_Data",
                "plz.csv");

            File.Copy(originalFilename, filename);

            //////////////////////
            // Loads the workspace
            var defaultConfiguration = new CSVStorageConfiguration
            {
                ExtentUri = $"datenmeister:///zipcodes/{randomNumber}",
                Path = filename,
                Workspace = workspace.ws,
                Settings =
                {
                    HasHeader = false,
                    Separator = '\t',
                    Encoding = "UTF-8"
                }
            };

            _loader.LoadExtent(defaultConfiguration, false);

            Debug.WriteLine("Zip codes loaded");
        }
    }
}