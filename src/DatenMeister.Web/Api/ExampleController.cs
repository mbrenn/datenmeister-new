using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Http;
using DatenMeister.CSV.Runtime.Storage;
using DatenMeister.DataLayer;
using DatenMeister.EMOF.Helper;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Web.Models.PostModels;

namespace DatenMeister.Web.Api
{
    [RoutePrefix("api/datenmeister/example")]
    public class ExampleController : ApiController
    {
        private readonly IDataLayerLogic _dataLayerLogic;
        private readonly IWorkspaceCollection _collection;
        private readonly IExtentStorageLoader _loader;

        private static readonly Random Random = new Random();

        public ExampleController(IDataLayerLogic dataLayerLogic, IWorkspaceCollection collection, IExtentStorageLoader loader)
        {
            _dataLayerLogic = dataLayerLogic;
            _collection = collection;
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
            var uml = _dataLayerLogic.Get<_UML>(DataLayers.Uml);
            var element = _collection.FindItem("dm:///types#DatenMeister.Apps.ZipCode.Model.ZipCode");
            var idProperty = element.GetByPropertyFromCollection(uml.Classification.Classifier.attribute, uml.CommonStructure.NamedElement.name, "Id").FirstOrDefault();
            var zipProperty = element.GetByPropertyFromCollection(uml.Classification.Classifier.attribute, uml.CommonStructure.NamedElement.name, "Zip").FirstOrDefault();
            var positionLongProperty = element.GetByPropertyFromCollection(uml.Classification.Classifier.attribute, uml.CommonStructure.NamedElement.name, "PositionLong").FirstOrDefault();
            var positionLatProperty = element.GetByPropertyFromCollection(uml.Classification.Classifier.attribute, uml.CommonStructure.NamedElement.name, "PositionLat").FirstOrDefault();
            var citynameProperty = element.GetByPropertyFromCollection(uml.Classification.Classifier.attribute, uml.CommonStructure.NamedElement.name, "CityName").FirstOrDefault();


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
                    Columns = new object[] { idProperty, zipProperty, positionLongProperty, positionLatProperty, citynameProperty }.ToList(),
                    MetaclassUri = "dm:///types#DatenMeister.Apps.ZipCode.Model.ZipCode"
                }
            };

            _loader.LoadExtent(defaultConfiguration, false);

            Debug.WriteLine("Zip codes loaded");
        }
    }
}