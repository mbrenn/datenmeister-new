﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Web.PostModels;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.Web.Api
{
    [Route("api/datenmeister/example")]
    public class ExampleController : Controller
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(ExampleController));

        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IExtentManager _loader;

        private static readonly Random Random = new Random();

        public ExampleController(IWorkspaceLogic workspaceLogic,IExtentManager loader)
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

            var defaultConfiguration = new CSVExtentLoaderConfig
            {
                extentUri = $"datenmeister:///zipcodes/{randomNumber}",
                filePath = filename,
                workspaceId = workspace.ws,
                Settings =
                {
                    HasHeader = false,
                    Separator = '\t',
                    Encoding = "UTF-8",
                    Columns = new [] { "Id", "Zip", "PositionLong", "PositionLat", "CityName" }.ToList(),
                    // Columns = new object[] { idProperty, zipProperty, positionLongProperty, positionLatProperty, citynameProperty }.ToList(),
                    MetaclassUri = "datenmeister:///types#DatenMeister.Apps.ZipCode.Model.ZipCode"
                }
            };

            _loader.LoadExtent(defaultConfiguration, false);

            Logger.Info("Zip codes loaded");
        }
    }
}