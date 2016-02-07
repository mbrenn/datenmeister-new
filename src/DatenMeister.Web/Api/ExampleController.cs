using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
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

        public ExampleController(IExtentStorageLoader loader)
        {
            _loader = loader;
        }

        [Route("addzipcodes")]
        public void AddZipExample([FromBody] WorkspaceReferenceModel workspace)
        {
            //////////////////////
            // Loads the workspace
            var file = Path.Combine(
                Path.Combine(
                    AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                    "App_Data"),
                "plz.csv");

            var defaultConfiguration = new CSVStorageConfiguration
            {
                ExtentUri = "datenmeister:///zipcodes",
                Path = file,
                Workspace = workspace.ws,
                Settings =
                {
                    HasHeader = false,
                    Separator = '\t',
                    Encoding = "UTF-8"
                }
            };

            _loader.LoadExtent(defaultConfiguration);

            Debug.WriteLine("Zip codes loaded");
        }
    }
}